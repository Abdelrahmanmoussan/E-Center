using IdentityText.Data;
using IdentityText.Enums;
using IdentityText.Models;
using IdentityText.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace IdentityText.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _dbContext = appDbContext;
        }


        public async Task<List<Payment>> GetPaymentsReportAsync(DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.Payments
                .Include(p => p.Enrollment)
                .ThenInclude(e => e.Student) // لو عاوز الطالب
                .AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(p => p.PaymentDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(p => p.PaymentDate <= toDate.Value);

            return await query.ToListAsync();
        }

        public async Task<List<Payment>> GetFilteredPaymentsAsync(int? teacherId, DateTime? from, DateTime? to)
        {
            var query = _dbContext.Payments
                .Include(p => p.Enrollment)
                    .ThenInclude(e => e.Student) // ضروري علشان نحسب عدد الطلاب
                .Include(p => p.Enrollment.ClassGroup)
                    .ThenInclude(c => c.Teacher)
                .AsQueryable();

            if (teacherId.HasValue)
                query = query.Where(p => p.Enrollment.ClassGroup.TeacherId == teacherId.Value);

            if (from.HasValue)
                query = query.Where(p => p.PaymentDate >= from.Value);

            if (to.HasValue)
                query = query.Where(p => p.PaymentDate <= to.Value);

            return await query.ToListAsync();
        }




        public async Task<decimal> GetTodaySalesAsync()
        {
            var today = DateTime.Today;

            return await _dbContext.Payments
                .Where(p => p.PaymentDate.Date == today && p.PaymentStatus == PaymentStatus.Paid)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;
        }




        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _dbContext.Payments.SumAsync(p => p.Amount);
        }



        public async Task<decimal> GetTotalRevenueByMonthAsync(int month, int year)
        {
            return await _dbContext.Payments
                .Where(p => p.PaymentDate.Month == month && p.PaymentDate.Year == year)
                .SumAsync(p => (decimal?)p.Amount) ?? 0;
        }



    }
}
