
using QuestPDF.Fluent;
using QuestPDF.Helpers;
namespace IdentityText.Models.ViewModel
{
    public class PaymentsReportViewModel
    {
        public string TeacherName { get; set; }
        public int TotalStudents { get; set; }
        public decimal TotalRevenue { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? TeacherId { get; set; }

        public decimal NetAmountForTeacher => TotalRevenue * 0.7m;
        public decimal NetAmountForCenter => TotalRevenue * 0.3m;

        public decimal PlatformCut => TotalRevenue - NetAmountForTeacher;
        public int PaidCount { get; set; }
        public int PendingCount { get; set; }
        public int FailedCount { get; set; }
        public int CancelledCount { get; set; }
        public List<Payment> Payments { get; set; }
    }



    public class PaymentReportPdfGenerator
    {
        private readonly PaymentsReportViewModel _model;

        public PaymentReportPdfGenerator(PaymentsReportViewModel model)
        {
            _model = model;
        }

        public byte[] GeneratePdf()
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.Header().Text("تقرير المدفوعات").FontSize(20).SemiBold().AlignCenter();
                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(10).Text($"المدرس: {_model.TeacherName}");
                        col.Item().PaddingBottom(10).Text($"إجمالي الإيرادات: {_model.TotalRevenue:C}");
                        col.Item().PaddingBottom(10).Text($"صافي للمعلم: {_model.NetAmountForTeacher:C}");
                        col.Item().PaddingBottom(10).Text($"صافي للسنتر: {_model.NetAmountForCenter:C}");
                        col.Item().PaddingBottom(10).Text($"عدد الطلاب: {_model.TotalStudents}");

                        // جدول المدفوعات
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100); // الطالب
                                columns.ConstantColumn(80);  // المبلغ
                                columns.ConstantColumn(100); // صافي للمعلم
                                columns.ConstantColumn(100); // تاريخ الدفع
                                columns.RelativeColumn();    // الحالة
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Text("الطالب").Bold();
                                header.Cell().Text("المبلغ").Bold();
                                header.Cell().Text("صافي للمعلم").Bold();
                                header.Cell().Text("تاريخ الدفع").Bold();
                                header.Cell().Text("الحالة").Bold();
                            });

                            // Rows
                            foreach (var payment in _model.Payments)
                            {
                                table.Cell().Text(payment.Enrollment.Student.FullName);
                                table.Cell().Text(payment.Amount.ToString("C"));
                                table.Cell().Text(payment.NetAmountForTeacher.ToString("C"));
                                table.Cell().Text(payment.PaymentDate.ToString("yyyy-MM-dd") ?? "-");
                                table.Cell().Text(payment.PaymentStatus.ToString());
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("تم التوليد بواسطة النظام - ");
                        txt.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm")).FontSize(10);
                    });
                });
            });

            return document.GeneratePdf();
        }
    }

}
