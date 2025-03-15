using Microsoft.EntityFrameworkCore;
using StripeApi.Data;
using StripeApi.Data.Entities;

namespace StripeApi.UsesCases.user
{
    public class SetPremium(ApplicationDbContext applicationDbContext)
    {
        public async Task Execute(string userId, string subscriptionId)
        {
            if (applicationDbContext.UserClaims.Any(a => a.UserId == userId
            && a.ClaimType == UserConstants.PREMIUM_CLAIM))
                return;

            applicationDbContext.UserClaims.Add(new Microsoft.AspNetCore.Identity.IdentityUserClaim<string>()
            {
                UserId = userId,
                ClaimType = UserConstants.PREMIUM_CLAIM,
                ClaimValue = "enabled"
            });

            UserSubscription? subscriptionEntity = await applicationDbContext.UserSubscriptions
                .FirstOrDefaultAsync(a => a.SubscriptionId == subscriptionId && a.UserId == userId);

            if (subscriptionEntity is not null)
            {
                subscriptionEntity.IsActive = true;
                applicationDbContext.UserSubscriptions.Update(subscriptionEntity);
            }
            else
            {
                await applicationDbContext.UserSubscriptions
                    .AddAsync(new UserSubscription()
                    {
                        IsActive = true,
                        SubscriptionId = subscriptionId,
                        UserId = userId,
                        ValidUntil = null
                    });
            }



            await applicationDbContext.SaveChangesAsync();
        }
    }
}
