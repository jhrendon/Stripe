using StripeApi.Data.InMemory;
using System.Security.Claims;

namespace StripeApi.UsesCases.ShoppingCart
{
    public class GetShoppingCart(IInMemoryShoppingCart inMemoryShoppingCart,
     IHttpContextAccessor httpContextAccessor)
    {
        public ShoppingCartDto Execute()
            => inMemoryShoppingCart
            .Get(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email));
    }
}
