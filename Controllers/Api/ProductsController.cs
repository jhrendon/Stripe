using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StripeApi.UseCases.Products;
using StripeApi.UsesCases.Products;

namespace StripeApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(AddProduct addProduct, GetProducts getProducts) : ControllerBase
    {
        [HttpPost]
        public async Task<bool> InsertProduct(CreateProductRequest request)
            => await addProduct.Execute(request);


    }
}
