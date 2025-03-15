using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using StripeApi.Data;
using StripeApi.UsesCases.user;

namespace StripeApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController(IConfiguration configuration,IOptions<StripeApiSettings>  options  ,SetPremium setPremium,
          RemovePremium removePremium, SetPremiumEnd setPremiumEnd)
          : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Post()
        {
             var x = options.Value.StripeWebhookSecret;


            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
            
                Event? stripeEvent = EventUtility.ConstructEvent(json,
                        Request.Headers["Stripe-Signature"], configuration["StripeWebhookSecret"]);

       
                // Handle the event
                switch (stripeEvent.Type)
                {
                    case EventTypes.CheckoutSessionCompleted:
                        await HandleSeessionCompleted(stripeEvent.Data.Object as Session);
                        break;
                    case EventTypes.CustomerSubscriptionUpdated:
                        await HandleSubscriptionUpdated(stripeEvent.Data.Object as Subscription);
                        break;
                    case EventTypes.CustomerSubscriptionDeleted:
                        await HandleSubscriptionDeleted(stripeEvent.Data.Object as Subscription);
                        break;
                    default:
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                        break;
                }
                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        private async Task HandleSubscriptionUpdated(Subscription subscription)
        {
            if (subscription.CancelAtPeriodEnd && subscription.CancelAt is not null)
            {
                await setPremiumEnd.Execute(subscription.Id, (DateTime)subscription.CancelAt);
            }
        }


        private async Task HandleSubscriptionDeleted(Subscription subscription)
        {
            await removePremium.Execute(subscription.Id);
        }

        private async Task HandleSeessionCompleted(Session session)
        {
            Console.WriteLine("EVENTO");

            Console.WriteLine($"EMAIL: {session.CustomerDetails.Email}");

            var options = new SessionGetOptions();
            options.AddExpand("line_items");
            var service = new SessionService();

            Session sessionWtihLineItems = await service.GetAsync(session.Id, options);
            foreach (var item in sessionWtihLineItems.LineItems)
            {
                Console.WriteLine($"PriceId {item.Price.Id} - QTY: {item.Quantity}");

                if (item.Price.Id == "price_1PQlYfL78xk9bMx0XxBZMSQO")
                {
                    if (session.Metadata.TryGetValue("userid", out string userId))
                    {
                        Console.WriteLine("HERE IT GOES A SUBSCRIPTION");
                        await setPremium.Execute(userId, session.SubscriptionId);
                    }

                }
                else
                {
                    Console.WriteLine("HERE IT SHOULD SEND THE BOOK BY EMAIL");
                }


            }



        }
    }
}
