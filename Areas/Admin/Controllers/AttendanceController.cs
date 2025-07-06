using IdentityText.Enums;
using IdentityText.Models;
using IdentityText.Models.ViewModel;
using IdentityText.Repository;
using IdentityText.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityText.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Teacher")]
    public class AttendanceController : Controller
    {
        private readonly IClassGroupRepository _classGroupRepository;
        private readonly ILectureRepository _lectureRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public AttendanceController(
            IClassGroupRepository classGroupRepository,
            ILectureRepository lectureRepository,
            ITeacherRepository teacherRepository,
            IAttendanceRepository attendanceRepository,
            IStudentRepository studentRepository,
            IEnrollmentRepository enrollmentRepository,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            _classGroupRepository = classGroupRepository;
            _lectureRepository = lectureRepository;
            _teacherRepository = teacherRepository;
            _attendanceRepository = attendanceRepository;
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TakeAttendance()
        {
            var userId = _userManager.GetUserId(User);
            var teacher = _teacherRepository.GetOne(t => t.UserId == userId);

            if (teacher == null)
                return RedirectToAction("NotFoundPage", "Home");

            var model = new StudentAttendanceVM
            {
                ClassGroups = await _classGroupRepository.SelectListClassGroupByTeacherIdAsync(teacher.TeacherId),
                Lectures = new List<SelectListItem>(),
                AttendanceStatusList = _attendanceRepository.GetAttendanceStatusSelectList(),
                Students = new List<StudentVM>(),
                Attendances = new List<AttendanceEntry>()
            };

            return View(model);
        }

        // AJAX endpoint to get lectures for class group
        [HttpGet]
        public IActionResult GetLecturesByClassGroup(int classGroupId)
        {
            var lectures = _lectureRepository.Get(filter: l => l.ClassGroupId == classGroupId)
                .Select(l => new { id = l.LectureId, title = l.Title });


            return Json(lectures);
        }

        // AJAX endpoint to get students
        [HttpGet]
        public IActionResult GetStudentsByClassGroup(int classGroupId)
        {
            var students = _enrollmentRepository.Get(filter: e => e.ClassGroupId == classGroupId, includes: [e => e.Student.ApplicationUser])
                .Select(e => new {
                    id = e.Student.StudentId,
                    fullName = e.Student.ApplicationUser.FirstName + " " + e.Student.ApplicationUser.LastName,
                    email = e.Student.ApplicationUser.Email
                });

            return Json(students);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAttendanceAsync(StudentAttendanceVM model)
        {
            foreach (var entry in model.Attendances)
            {
                // Get EnrollmentId for this student and class group
                var enrollment = _enrollmentRepository
                    .Get(filter: e => e.StudentId == entry.StudentId && e.ClassGroupId == model.ClassGroupId,
                    includes: [e => e.ClassGroup.Lectures])
                    .FirstOrDefault();

                if (enrollment == null)
                    continue; // Skip if enrollment not found

                // check if attendance already exists
                var existingAttendance = _attendanceRepository.GetOne(
                    filter: a => a.StudentId == entry.StudentId && a.LectureId == model.LectureId);

                if (existingAttendance != null)
                {
                    // update existing attendance
                    existingAttendance.AttendanceStatus = entry.AttendanceStatus;
                    existingAttendance.Remark = entry.Remark;
                    existingAttendance.Date = DateTime.Now;

                    _attendanceRepository.Edit(existingAttendance);

                    if (entry.AttendanceStatus == AttendanceStatus.Absent)
                    {
                        var student = _studentRepository.GetOne(
                            filter: s => s.StudentId == entry.StudentId,
                            includes: [s => s.ApplicationUser]);

                        var currentLecture = enrollment.ClassGroup?.Lectures
                            ?.FirstOrDefault(l => l.LectureId == model.LectureId);

                        if (student != null
                            && !string.IsNullOrEmpty(student.ParentMail)
                            && currentLecture != null)
                        {
                            string studentName = $"{student.ApplicationUser.FirstName} {student.ApplicationUser.LastName}";
                            string lectureDate = currentLecture.LectureDate.ToString("yyyy-MM-dd");

                            string subject = $"Absence Warning: {studentName} Missed a Lecture";
                            string message = $"Dear Parent/{student.ParentName},\n We would like to inform you that your child, {studentName}, was absent from the lecture {currentLecture.Title} conducted on {lectureDate:MMMM dd, yyyy}.\n\nIf you have any questions or concerns, please do not hesitate to contact us.\n\nBest regards,\n[Your Institution Name]";

                            await _emailSender.SendEmailAsync(student.ParentMail, subject, message);
                        }
                    }
                }
                else
                {
                    var attendance = new Attendance
                    {
                        StudentId = entry.StudentId,
                        LectureId = model.LectureId,
                        AttendanceStatus = entry.AttendanceStatus,
                        Remark = entry.Remark,
                        Date = DateTime.Now,
                        CreatedAt = DateTime.UtcNow,
                        EnrollmentId = enrollment.EnrollmentId
                    };
                    _attendanceRepository.Create(attendance);

                    if (entry.AttendanceStatus == AttendanceStatus.Absent)
                    {
                        var student = _studentRepository.GetOne(
                            filter: s => s.StudentId == entry.StudentId,
                            includes: [s => s.ApplicationUser]);

                        var currentLecture = enrollment.ClassGroup?.Lectures
                            ?.FirstOrDefault(l => l.LectureId == model.LectureId);

                        if (student != null
                            && !string.IsNullOrEmpty(student.ParentMail)
                            && currentLecture != null)
                        {
                            string studentName = $"{student.ApplicationUser.FirstName} {student.ApplicationUser.LastName}";
                            string lectureDate = currentLecture.LectureDate.ToString("yyyy-MM-dd");

                            string subject = $"Absence Warning: {studentName} Missed a Lecture";
                            string message = $"Dear Parent/{student.ParentName},\n We would like to inform you that your child, {studentName}, was absent from the lecture {currentLecture.Title} conducted on {lectureDate:MMMM dd, yyyy}.\n\nIf you have any questions or concerns, please do not hesitate to contact us.\n\nBest regards,\n[Your Institution Name]";

                            await _emailSender.SendEmailAsync(student.ParentMail, subject, message);
                        }
                    }
                }
            }

            _attendanceRepository.Commit();

            return RedirectToAction("ShowAttendance");

        }
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public IActionResult ShowAttendance(int? classGroupId, int? lectureId)
        {
            var allAttendances = _attendanceRepository.Get(includes: [
                a => a.Student.ApplicationUser,
                a => a.Lecture,
                a => a.Enrollment.ClassGroup
            ]);

            if (allAttendances == null || !allAttendances.Any())
            {
                TempData["Error"] = "لا توجد سجلات حضور.";
                return RedirectToAction("TakeAttendance");  
            }

            if (classGroupId.HasValue)
                TempData["SelectedClassGroupId"] = classGroupId.Value;

            if (lectureId.HasValue)
                TempData["SelectedLectureId"] = lectureId.Value;

            var filteredAttendances = allAttendances
                .Where(a => (!classGroupId.HasValue || a.Enrollment.ClassGroupId == classGroupId)
                         && (!lectureId.HasValue || a.LectureId == lectureId))
                .ToList();

            return View(filteredAttendances);
        }

       
        [Authorize(Roles = "Teacher,Admin")]
        [HttpGet]
        public async Task<IActionResult> AbsenceSummaryAsync(int? classGroupId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Index", "Home");
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {

                var allAttendances = _attendanceRepository.Get(includes: [
                    a => a.Student.ApplicationUser,
                    a => a.Lecture,
                    a => a.Enrollment.ClassGroup
                ]);

                if (allAttendances == null || !allAttendances.Any())
                {
                    TempData["Error"] = "No attendance records found.";
                    return RedirectToAction("TakeAttendance");
                }

                ViewBag.ClassGroups = allAttendances
                    .Where(a => a.Enrollment != null && a.Enrollment.ClassGroup != null)
                    .Select(a => a.Enrollment.ClassGroup)
                    .GroupBy(cg => cg.ClassGroupId)
                    .Select(g => g.First())
                    .ToList();

                if (classGroupId.HasValue)
                {
                    ViewBag.SelectedClassGroupId = classGroupId.Value;
                    var grouped = allAttendances
                        .Where(a => a.Enrollment.ClassGroupId == classGroupId.Value &&
                                    a.AttendanceStatus == AttendanceStatus.Absent)
                        .GroupBy(a => a.Student)
                        .Select(g => new StudentAttendanceReportVM
                        {
                            Student = g.Key,
                            AbsenceCount = g.Count()
                        }).ToList();

                    return View(grouped);
                }

                return View(new List<StudentAttendanceReportVM>());
            }
            else
            {
               
                var teacher = _teacherRepository.GetOne(t => t.UserId == user.Id);
                if (teacher == null)
                {
                    TempData["Error"] = "Teacher profile not found.";
                    return RedirectToAction("Index", "Home");
                }

                var teacherClassGroups = _classGroupRepository.Get(
                     filter: cg => cg.TeacherId == teacher.TeacherId , includes: [cg => cg.Enrollments] );

                if (!teacherClassGroups.Any())
                {
                    TempData["Error"] = "You don't have any class groups assigned.";
                    return View(new List<StudentAttendanceReportVM>());
                }

                ViewBag.ClassGroups = teacherClassGroups;

                if (classGroupId.HasValue)
                {

                    var selectedClassGroup = teacherClassGroups
                        .FirstOrDefault(cg => cg.ClassGroupId == classGroupId.Value);

                    if (selectedClassGroup == null)
                    {
                        TempData["Error"] = "You are not authorized to view this class group.";
                        return RedirectToAction("Index", "Home");
                    }

                    ViewBag.SelectedClassGroupId = classGroupId.Value;

                    var absences = _attendanceRepository.Get(
                        a => a.Enrollment.ClassGroupId == classGroupId.Value &&
                             a.AttendanceStatus == AttendanceStatus.Absent,
                        includes: [a => a.Student.ApplicationUser]
                    ).ToList();

                    var grouped = absences
                        .GroupBy(a => a.Student)
                        .Select(g => new StudentAttendanceReportVM
                        {
                            Student = g.Key,
                            AbsenceCount = g.Count()
                        }).ToList();

                    return View(grouped);
                }

                return View(new List<StudentAttendanceReportVM>());
            }
        }


        [HttpPost]
        public async Task<IActionResult> NotifyParent(int studentId, int classGroupId)
        {
            var absences = _attendanceRepository.Get(
                filter: a => a.AttendanceStatus == AttendanceStatus.Absent && a.StudentId == studentId && a.Enrollment.ClassGroupId == classGroupId,
                includes: [a => a.Student.ApplicationUser, a => a.Enrollment.ClassGroup]
            );

            var student = absences.FirstOrDefault()?.Student;
            var classGroup = absences.FirstOrDefault()?.Enrollment.ClassGroup;

            if (student != null && !string.IsNullOrEmpty(student.ParentMail) && classGroup != null)
            {
                string subject = "Alert: Number of absences exceeded";
                string message = $"Dear Parent/ {student.ParentName} student {student.ApplicationUser.FirstName} {student.ApplicationUser.LastName} <br> has been absent {absences.Count()} times from the course {classGroup.Title}.";


                await _emailSender.SendEmailAsync(student.ParentMail, subject, message);

                TempData["Success"] = $"The alert has been successfully sent to the parent of student {student.ApplicationUser.FirstName}.";
            }

            return RedirectToAction("AbsenceSummary", new { classGroupId });
        }


    }
}
