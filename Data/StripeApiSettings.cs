namespace StripeApi.Data
{
    public class StripeApiSettings
    {
        

        public string ConnectionString { get; set; } = null!;
        public string appStorageConnectionString { get; set; } = null!;
        public string StripeApiKey { get; set; } = null!;
        public string StripeWebhookSecret { get; set; } = null!;

        public int PagerPageSize { get; set; }

        public string EventBusConnection { get; set; } = null!;

    }
}
