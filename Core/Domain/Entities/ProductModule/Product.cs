using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; }

        // 1-M Product - ProductType
        public ProductType ProductType { get; set; }
        public int TypeId { get; set; }

        // 1-M Product - Product Brand
        public ProductBrand ProductBrand { get; set; }
        public int BrandId { get; set; }

    }
}
