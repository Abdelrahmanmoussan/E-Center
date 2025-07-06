using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityText.Enums
{
    public enum ClassGroupStatus
    {
        NotPurchased , 
        Purchased 
    }
    public enum RoleStatus
    {
        Student,
        Teacher
    }

    public enum ParentStatus
    {
        Father,
        Mother,
        Others
    }

    public enum EnrollmentStatus
    {
        Active,
        Canceled,
        Completed,
        PendingPayment
    }

    public enum AttendanceStatus
    {
        [Display(Name = "Present")]
        Present,

        [Display(Name = "Absent")]
        Absent,

        [Display(Name = "Late")]
        Late
    }

    public enum SubscriptionStatus
    {
        Active,
        Expired
    }

    public enum PaymentMethod
    {
        Cash,
        Visa,
        MasterCard,
        BankTransfer,
        Other
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Cancelled
    }


    public enum TeacherStatus
    {
        Online,
        Offline
    }
    public enum SubjectType
    {
        General,    // مادة أساسية
        Optional,// مادة اختيارية
        Theoretical,
        Practical,
        Mixed
    }

}
