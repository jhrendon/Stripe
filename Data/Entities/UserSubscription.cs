namespace StripeApi.Data.Entities
{
    public class UserSubscription
    {
        public required string UserId { get; set; }
        public required string SubscriptionId { get; set; }
        public required bool IsActive { get; set; }
        public DateTime? ValidUntil { get; set; }
    }
}
