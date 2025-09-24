using Microsoft.EntityFrameworkCore;
using System;

namespace MyEcomApi.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; } 
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int MinQuantity { get; set; }
        [Precision(5, 2)]
        public decimal DiscountRate { get; set; }
    }
}
