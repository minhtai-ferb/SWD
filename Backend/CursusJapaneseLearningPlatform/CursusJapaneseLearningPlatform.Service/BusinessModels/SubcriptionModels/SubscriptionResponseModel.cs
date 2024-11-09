﻿using CursusJapaneseLearningPlatform.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Service.BusinessModels.SubcriptionModels
{
    public class SubscriptionResponseModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TokenLimit { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; }
        public bool Status { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public SubscriptionResponseModel(Subscription subscription)
        {
            Id = subscription.Id;
            UserId = subscription.UserId;
            PackageId = subscription.PackageId;
            StartDate = subscription.StartDate;
            EndDate = subscription.EndDate;
            TokenLimit = subscription.TokenLimit;
            Amount = subscription.Amount;
            PaymentStatus = subscription.PaymentStatus;
            Status = subscription.IsActive;
            CreatedTime = subscription.CreatedTime;
            LastUpdatedTime = subscription.LastUpdatedTime;
        }
    }
}