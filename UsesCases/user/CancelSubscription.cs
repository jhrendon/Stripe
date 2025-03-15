using Microsoft.EntityFrameworkCore;
using Stripe;
using StripeApi.Data;
using StripeApi.Data.Entities;

namespace StripeApi.UsesCases.user
{
    public class CancelSubscription(ApplicationDbContext applicationDbContext)
    {

        public async Task Execute(string userId)
        {
            UserSubscription subscription = await applicationDbContext.UserSubscriptions
                .SingleAsync(a => a.UserId == userId && a.IsActive == true);

            var options = new SubscriptionUpdateOptions()
            {
                CancelAtPeriodEnd = true
            };

            var service = new SubscriptionService();
            await service.UpdateAsync(subscription.SubscriptionId, options);
        }
    }
}
