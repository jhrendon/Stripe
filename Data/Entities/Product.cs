﻿namespace StripeApi.Data.Entities
{
    public class Product
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
        public required string StripePriceId { get; set; }

    }
}