using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStudentRepository _studentRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IAcademicYearRepository _academicYearRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserController(
            UserManager<ApplicationUser> userManager,
            IStudentRepository studentRepository,
            ITeacherRepository teacherRepository,
            ISubscriptionRepository subscriptionRepository,
            IAcademicYearRepository academicYearRepository,
            ISubjectRepository subjectRepository,
            IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager,
            IClassGroupRepository classGroupRepository
            )
        {
            _userManager = userManager;
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            _subscriptionRepository = subscriptionRepository;
            _academicYearRepository = academicYearRepository;
            _subjectRepository = subjectRepository;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _classGroupRepository = classGroupRepository;
        }
        public string ReturnUrl { get; set; }

        public async Task<IActionResult> Index(string searchName, int page = 1, int pageSize = 4)
        {
            var users = _userManager.Users.ToList();
            var filteredUsers = new List<ApplicationUser>();

            if (!string.IsNullOrEmpty(searchName))
            {
                filteredUsers = users.Where(u =>
                    u.FirstName.StartsWith(searchName, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.StartsWith(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                filteredUsers = users;
            }

            var userWithRoles = new List<UserWithRoleVM>();
            var paginatedUsers = filteredUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var user in paginatedUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new UserWithRoleVM
                {
                    User = user,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)filteredUsers.Count / pageSize);
            ViewBag.SearchName = searchName;

            return View(userWithRoles);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new UserVM
            {
                AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync(),
                SubjectsList = await _subjectRepository.SelectListSubjectAsync()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserVM userVM, IFormFile file, string returnUrl = null)
        {


            if (userVM.Role == "Student")
            {
                ModelState.Remove("TeacherNetAmount");
                ModelState.Remove("TeacherNotes");
                ModelState.Remove("TeacherHireDate");
                ModelState.Remove("TeacherDB");
                ModelState.Remove("Specialty");
                ModelState.Remove("SubjectId");
            }
            else if (userVM.Role == "Teacher")
            {
                ModelState.Remove("ParentName");
                ModelState.Remove("ParentPhone");
                ModelState.Remove("ParentMail");
                ModelState.Remove("StudentDB");
                ModelState.Remove("EnrollmentDate");
                ModelState.Remove("EmergencyContact");
                ModelState.Remove("StudentNotes");
                ModelState.Remove("AcademicYearId");
            }
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(stream);
                    }

                    userVM.Photo = fileName;
                }
                if (userVM != null)
                {
                    var user = new ApplicationUser
                    {
                        FirstName = userVM.FirstName,
                        LastName = userVM.LastName,
                        Address = userVM.Address,
                        UserName = userVM.Email,
                        Email = userVM.Email,
                        Photo = userVM.Photo,

                    };

                    var result = await _userManager.CreateAsync(user, userVM.Password);
                    if (result.Succeeded)
                    {
                        // Assign role to the user
                        if (!string.IsNullOrEmpty(userVM.Role))
                        {
                            await _userManager.AddToRoleAsync(user, userVM.Role);
                        }

                        if (userVM.Role == "Student")
                        {
                            var student = new Student
                            {
                                UserId = user.Id,
                                ParentName = userVM.ParentName,
                                ParentPhone = userVM.ParentPhone,
                                ParentMail = userVM.ParentMail,
                                StudentDB = userVM.StudentDB,
                                EnrollmentDate = userVM.EnrollmentDate,
                                EmergencyContact = userVM.EmergencyContact,
                                StudentNotes = userVM.StudentNotes,
                                AcademicYearId = userVM.AcademicYearId,
                            };

                            _studentRepository.Create(student);
                            _studentRepository.Commit();
                        }
                        else if (userVM.Role == "Teacher")
                        {
                            var teacher = new Teacher
                            {
                                UserId = user.Id,
                                TeacherHireDate = userVM.TeacherHireDate,
                                TeacherDB = userVM.TeacherDB,
                                TeacherNetAmount = userVM.TeacherNetAmount,
                                TeacherNotes = userVM.TeacherNotes,
                                SubjectId = userVM.SubjectId
                            };

                            _teacherRepository.Create(teacher);
                            _teacherRepository.Commit();
                        }
                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(userVM.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToAction(
                                actionName: "RegisterConfirmation",
                                controllerName: "Account",
                                routeValues: new { area = "Identity", email = userVM.Email, returnUrl = returnUrl }
                            );

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
                    TempData["notification"] = "User created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction("NotFoundPage", "Home");
            }
            userVM.AcademicYearsList = await _academicYearRepository.SelectListAcademicYearAsync();
            userVM.SubjectsList = await _subjectRepository.SelectListSubjectAsync();
            return View(userVM);

        }

        [HttpGet]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var teacher = _teacherRepository.GetOne(e => e.UserId == id, tracked: false);
            var student = _studentRepository.GetOne(e => e.UserId == id, tracked: false);

            if (user != null)
            {
                if (teacher != null)
                {
                    var teacherClassGroups = _classGroupRepository.Get(e => e.TeacherId == teacher.TeacherId, tracked: false);
                    if (teacherClassGroups != null && teacherClassGroups.Any())
                    {
                        _classGroupRepository.DeleteAll(teacherClassGroups.ToList());
                        _classGroupRepository.Commit();
                    }

                    _teacherRepository.Delete(teacher);
                    _teacherRepository.Commit();
                }
                if (student != null)
                {
                    _studentRepository.Delete(student);
                    _studentRepository.Commit();
                }

                await _userManager.DeleteAsync(user);
                TempData["notification"] = "User deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }



    }
}
