using CursusJapaneseLearningPlatform.Repository.Entities;
using CursusJapaneseLearningPlatform.Repository.Implementations.GenericRepositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CursusJapaneseLearningPlatform.Repository.Interfaces.GenericRepositories;
using CursusJapaneseLearningPlatform.Repository.Interfaces.SubcriptionManagementRepositories;

namespace CursusJapaneseLearningPlatform.Repository.Implementations.EntitiesRepositories
{
    

    // Repository/SubscriptionRepository.cs
    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<Subscription> GetByPaymentIdAsync(string paymentId)
        {
            return await _context.Subscriptions
                .FirstOrDefaultAsync(s => s.PaymentId == paymentId);
        }

        public async Task<IEnumerable<Subscription>> GetUserSubscriptionsAsync(Guid userId)
        {
            return await _context.Subscriptions
                .Where(s => s.UserId == userId && s.IsActive && !s.IsDelete)
                .Include(s => s.Package)
                .ToListAsync();
        }
        public async Task<bool> HasActiveSubscriptionAsync(Guid userId)
        {
            var currentDate = DateTime.UtcNow;

            // First check if user has any subscriptions at all
            var hasAnySubscription = await _context.Subscriptions
                .AnyAsync(s => s.UserId == userId);

            if (!hasAnySubscription)
            {
                return false;
            }

            // Then check for active and valid subscription
            return await _context.Subscriptions
                .AnyAsync(s =>
                    s.UserId == userId &&
                    s.IsActive &&
                    !s.IsDelete &&
                    s.PaymentStatus == "Completed" &&
                    s.StartDate <= currentDate &&
                    s.EndDate >= currentDate);
        }
    }
}
