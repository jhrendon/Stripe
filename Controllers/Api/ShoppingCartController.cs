using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StripeApi.UsesCases.ShoppingCart;

namespace StripeApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController(AddToShoppingCart addToShoppingCart)
       : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post(AddItemRequest request)
        {
            if (addToShoppingCart.Execute(request.StripePriceId))
            {
                return Ok();
            }

            return UnprocessableEntity();
        }

        public record AddItemRequest(string StripePriceId);

    }
}
