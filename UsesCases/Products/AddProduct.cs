using StripeApi.Data;
using Stripe;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace StripeApi.UseCases.Products
{
    public class AddProduct(ApplicationDbContext _dbContext)
    {
        public async Task<bool> Execute(CreateProductRequest request)
        {
            //TODO: validate all pr

            string priceId = await CreateStripePriceId(request);


            _dbContext.Products.Add(new Data.Entities.Product()
            {
                ImageUrl = request.ImageUrl,
                Name = request.Name,
                Price = request.Price,
                StripePriceId = priceId
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }


        private async Task<string> CreateStripePriceId(CreateProductRequest createProductRequest)
        {
            var options = new ProductCreateOptions()
            {
                Name = createProductRequest.Name,
                Images = new List<string>
            {
                createProductRequest.ImageUrl
            }
            };

            var productService = new ProductService();
            Product product = await productService.CreateAsync(options);

            var priceOptions = new PriceCreateOptions()
            {
                Active = true,
                Currency = "usd",
                UnitAmount = Convert.ToInt64(createProductRequest.Price * 100),
                Product = product.Id,
            };

            var priceService = new PriceService();
            var price = await priceService.CreateAsync(priceOptions);
            return price.Id;
        }

    }



}

public record CreateProductRequest(string Name, string ImageUrl, decimal Price);