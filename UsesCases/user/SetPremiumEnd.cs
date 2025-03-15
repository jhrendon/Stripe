using Microsoft.EntityFrameworkCore;
using StripeApi.Data;
using StripeApi.Data.Entities;

namespace StripeApi.UsesCases.user
{
    public class SetPremiumEnd(ApplicationDbContext applicationDbContext)
    {

        public async Task Execute(string subscriptionId, DateTime subscriptionEndDate)
        {
            UserSubscription subscription = await applicationDbContext.UserSubscriptions
                .SingleAsync(a => a.SubscriptionId == subscriptionId);

            subscription.ValidUntil = subscriptionEndDate;
            applicationDbContext.UserSubscriptions.Update(subscription);

            await applicationDbContext.SaveChangesAsync();
        }

    }
}
