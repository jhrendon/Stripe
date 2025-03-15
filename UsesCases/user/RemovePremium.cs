using Microsoft.AspNetCore.Identity;
using StripeApi.Data;
using StripeApi.Data.Entities;

namespace StripeApi.UsesCases.user
{
    public class RemovePremium(ApplicationDbContext applicationDbContext)
    {
        public async Task Execute(string subcriptionId)
        {
            UserSubscription? subscritpion = applicationDbContext.UserSubscriptions
                .FirstOrDefault(a => a.SubscriptionId == subcriptionId);

            if (subscritpion is null)
                return;

            subscritpion.IsActive = false;
            applicationDbContext.UserSubscriptions.Update(subscritpion);


            IdentityUserClaim<string>? claim = applicationDbContext.UserClaims
                .FirstOrDefault(a => a.UserId == subscritpion.UserId
                && a.ClaimType == UserConstants.PREMIUM_CLAIM);

            if (claim is not null)
                applicationDbContext.UserClaims.Remove(claim);

            await applicationDbContext.SaveChangesAsync();
        }
    }
}
