using CursusJapaneseLearningPlatform.Service.BusinessModels.SubcriptionModels;
using CursusJapaneseLearningPlatform.Service.Commons.BaseResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Service.Interfaces
{
    public interface ISubscriptionService
    {
        Task<BaseResponseModel<string>> CreateSubscription(SubscriptionRequestModel request, Guid userId, CancellationToken cancellationToken = default);
        Task<BaseResponseModel<SubscriptionResponseModel>> CompleteSubscription(string paymentId, string payerId, string token, CancellationToken cancellationToken = default);
    }
}
