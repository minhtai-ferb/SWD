﻿using CursusJapaneseLearningPlatform.Repository.Entities;
using CursusJapaneseLearningPlatform.Repository.Interfaces.GenericRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Repository.Interfaces.SubcriptionManagementRepositories
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<Subscription> GetByPaymentIdAsync(string paymentId);
        Task<IEnumerable<Subscription>> GetUserSubscriptionsAsync(Guid userId);
        Task<bool> HasActiveSubscriptionAsync(Guid userId);
    }
}