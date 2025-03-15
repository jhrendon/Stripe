using StripeApi.Data.InMemory;
using System.Security.Claims;

namespace StripeApi.UsesCases.ShoppingCart
{
    public class AddToShoppingCart(IInMemoryShoppingCart inMemoryShoppingCart,
        IHttpContextAccessor httpContextAccessor)
    {
        public bool Execute(string stripePriceId)
            => inMemoryShoppingCart
            .Add(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email),
                stripePriceId);
    }
}
