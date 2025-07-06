using IdentityText.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityText.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }  // ربط الدفع بالتسجيل

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 1000000, ErrorMessage = "Amount must be positive.")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "varchar(20)")]
        public PaymentMethod PaymentMethod { get; set; } // مثال: Cash, Visa

        [MaxLength(500)]
        public string Notes { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal PlatformPercentage { get; set; }  // نسبة المنصة (مثلاً 10%)

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetAmountForTeacher { get; set; } // صافي المبلغ للمدرس بعد خصم نسبة المنصة

        [Required]
        [Column(TypeName = "varchar(20)")]
        public PaymentStatus PaymentStatus { get; set; } // مثال: Paid, Pending, Failed
    }
}
