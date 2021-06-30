using System;

namespace TheCuriousReaders.Models.ResponseModels
{
    public class ApprovedSubscriptionResponse
    {
        public string BookTitle { get; set; }
        public DateTime ReturnBookDate { get; set; }
        public int RemainingDays { get; set; }
    }
}
