using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; } = 0;
        public int Stock { get; private set; }

        public Product(string name, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Ürün adı boş olamaz");

            if (price <= 0)
                throw new ArgumentException("Fiyat 0'dan büyük olmalı");

            if (stock < 0)
                throw new ArgumentException("Stok negatif olamaz");

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
            Stock = stock;
        }

        public void DecreaseStock(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Adet 0'dan büyük olmalı");

            if (quantity > Stock)
                throw new InvalidOperationException("Yetersiz stok");

            Stock -= quantity;
        }

        public void ChangePrice(decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Fiyat 0'dan büyük olmalı");

            if (Price > newPrice)
            {
                if ((Price / 2) < (Price - newPrice))
                    throw new ArgumentException("Fiyat %50 den fazla düşemez");
            }
            


        }
    }
}
