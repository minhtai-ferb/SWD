﻿using CursusJapaneseLearningPlatform.Repository.Bases.BaseEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Repository.Entities
{
    public class Subscription : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TokenLimit { get; set; }
        public decimal Amount { get; set; }
        public string PaymentStatus { get; set; } // Pending, Completed, Failed
        public string PaymentId { get; set; } // PayPal payment ID
        public bool IsActive { get; set; }

        public User User { get; set; }
        public Package Package { get; set; }

        // Base entity properties
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsDelete { get; set; }
    }
}