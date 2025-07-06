// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using IdentityText.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing.Constraints;
using IdentityText.Data;
using IdentityText.Repository.IRepository;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using IdentityText.Validation;

namespace IdentityText.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubjectRepository subjectRepository,
            ISubscriptionRepository subscriptionRepository,
            IAcademicYearRepository academicYearRepository
            )
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
            _subscriptionRepository = subscriptionRepository;
            _academicYearRepository = academicYearRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }


        public class InputModel
        {
            [Display(Name = "Email")]
            [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
            [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
            [UniqueEmail]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "LastName")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Address")]
            public string Address { get; set; }
            public IFormFile? Photo { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

            // student.
            [Required]
            [MaxLength(100)]
            public string ParentName { get; set; }

            [Required]
            [Phone]
            public string ParentPhone { get; set; }

            [Display(Name = "Email")]
            [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
            [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
            [UniqueEmail]
            public string ParentMail { get; set; }
            [DataType(DataType.Date)]
            public DateTime? StudentDB { get; set; }
            [DataType(DataType.Date)]
            public DateTime EnrollmentDate { get; set; }
            [MaxLength(50)]
       
            public string EmergencyContact { get; set; }

            [MaxLength(500)]
            public string StudentNotes { get; set; }
            public int AcademicYearId { get; set; }
            public IEnumerable<SelectListItem> AcademicYearsList { get; set; } = Enumerable.Empty<SelectListItem>();


            //techer.
            public DateTime TeacherHireDate { get; set; }

            public DateTime? TeacherDB { get; set; }

            [Column(TypeName = "decimal(18,2)")]
            public decimal? TeacherNetAmount { get; set; }

            public string TeacherNotes { get; set; }

            public int SubjectId { get; set; }
            public IEnumerable<SelectListItem> SubjectsList { get; set; } = Enumerable.Empty<SelectListItem>();
            
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            Input = new InputModel
            {

                EnrollmentDate = DateTime.Now,
                TeacherHireDate = DateTime.Now,
                StudentDB = DateTime.Now,
                TeacherDB = DateTime.Now,
                AcademicYearId = 1,
                ParentName = "",
                ParentPhone = "",
                ParentMail = "",
                EmergencyContact = "",
                StudentNotes = "",
                TeacherNotes = "",
            };

            Input.AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync();
            Input.SubjectsList = await _subjectRepository.SelectListSubjectAsync();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            // إزالة الحقول غير المستخدمة حسب الدور
            if (Input.Role == "Student")
            {
                ModelState.Remove("Input.TeacherNetAmount");
                ModelState.Remove("Input.TeacherNotes");
                ModelState.Remove("Input.TeacherHireDate");
                ModelState.Remove("Input.TeacherDB");
                ModelState.Remove("Input.Specialty");
                ModelState.Remove("Input.SubjectId");
            }
            else if (Input.Role == "Teacher")
            {
                ModelState.Remove("Input.ParentName");
                ModelState.Remove("Input.ParentPhone");
                ModelState.Remove("Input.ParentMail");
                ModelState.Remove("Input.StudentDB");
                ModelState.Remove("Input.EnrollmentDate");
                ModelState.Remove("Input.EmergencyContact");
                ModelState.Remove("Input.StudentNotes");
                ModelState.Remove("Input.AcademicYearId");
            }


            if (ModelState.IsValid)
            {
                var user = CreateUser();
                // Assign the additional properties before creating the user
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.Address = Input.Address;
                IFormFile file = Input.Photo;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }
                    user.Photo = fileName;
                }

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, Input.Role);

                    // إنشاء بيانات إضافية حسب الدور
                    if (Input.Role == "Student")
                    {
                        var student = new Student
                        {
                            UserId = user.Id,
                            ParentName = Input.ParentName,
                            ParentPhone = Input.ParentPhone,
                            ParentMail = Input.ParentMail,
                            StudentDB = Input.StudentDB,
                            EnrollmentDate = Input.EnrollmentDate,
                            EmergencyContact = Input.EmergencyContact,
                            StudentNotes = Input.StudentNotes,
                            AcademicYearId = Input.AcademicYearId,
                        };

                        _studentRepository.Create(student);
                       _studentRepository.Commit();
                    }
                    else if (Input.Role == "Teacher")
                    {
                        var teacher = new Teacher
                        {
                            UserId = user.Id,
                            TeacherHireDate = Input.TeacherHireDate,
                            TeacherDB = Input.TeacherDB,
                            TeacherNetAmount = Input.TeacherNetAmount,
                            TeacherNotes = Input.TeacherNotes,
                            SubjectId = Input.SubjectId

                        };

                        _teacherRepository.Create(teacher);
                        _teacherRepository.Commit();
                    }
                    // Add the additional properties to the user

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");



                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
