using IdentityText.Models;

namespace IdentityText.Repository.IRepository
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<decimal> GetTodaySalesAsync();
        Task<decimal> GetTotalRevenueAsync();

        Task<List<Payment>> GetPaymentsReportAsync(DateTime? fromDate, DateTime? toDate);
        Task<List<Payment>> GetFilteredPaymentsAsync(int? teacherId, DateTime? from, DateTime? to);

        Task<decimal> GetTotalRevenueByMonthAsync(int month, int year);

    }
}
