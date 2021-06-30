using System;

namespace TheCuriousReaders.Models.ServiceModels
{
    public class ApprovedSubscriptionModel
    {
        public string BookTitle { get; set; }
        public DateTime ReturnBookDate { get; set; }
        public int RemainingDays { get; set; }
    }
}
