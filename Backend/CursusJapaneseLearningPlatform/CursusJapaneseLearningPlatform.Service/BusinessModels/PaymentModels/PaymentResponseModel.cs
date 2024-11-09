using CursusJapaneseLearningPlatform.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursusJapaneseLearningPlatform.Service.BusinessModels.PaymentModels
{
    public class PaymentResponseModel
    {
        private Payment createdPayment;

        public PaymentResponseModel(Payment createdPayment)
        {
            this.createdPayment = createdPayment;
        }

        public Guid PaymentId { get; set; }           // ID of the payment transaction
        public Guid UserId { get; set; }              // ID of the user who made the payment
        public decimal Amount { get; set; }           // Payment amount
        public string PaymentMethod { get; set; }     // Method of payment (e.g., "CreditCard", "PayPal")
        public string PaymentStatus { get; set; }     // Status of payment (e.g., "Success", "Pending", "Failed")
        public DateTimeOffset Timestamp { get; set; } // Timestamp when the payment was processed

        // Additional response details can be added here if needed
    }
}
