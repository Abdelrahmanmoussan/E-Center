using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using IdentityText.Models;
namespace IdentityText.Services
{
  
    public class SubscriptionPdfService
    {
        public byte[] GenerateSubscriptionPdf(Subscription subscription)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Header().Text("بيانات الاشتراك").SemiBold().FontSize(20).AlignCenter();

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text($"📄 كود الاشتراك: {subscription.Code}");
                        col.Item().Text($"👤 الطالب: {(subscription.Enrollment?.Student?.ApplicationUser != null
                            ? subscription.Enrollment.Student.ApplicationUser.FirstName + " " + subscription.Enrollment.Student.ApplicationUser.LastName
                            : "غير معروف")}");
                        col.Item().Text($"📘 الكورس: {subscription.Enrollment?.ClassGroup?.Title ?? "غير معروف"}");
                        col.Item().Text($"📅 من: {subscription.StartDate:yyyy-MM-dd} إلى {subscription.EndDate:yyyy-MM-dd}");
                        col.Item().Text($"🟢 الحالة: {subscription.SubscriptionStatus}");
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("تم توليد الاشتراك بتاريخ: ");
                        x.Span(DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    });
                });
            });

            return document.GeneratePdf();
        }
    }

}
