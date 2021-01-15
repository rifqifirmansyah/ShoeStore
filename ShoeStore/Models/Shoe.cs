using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeStore.Models
{
    public class Shoe
    {
        public Shoe()
        {
            UserCart = new HashSet<UserCart>();
        }

        public int ShoeId { get; set; }
        public string ShoeName { get; set; }
        public byte[] ShoeImage { get; set; }
        public decimal Price { get; set; }

        public ICollection<UserCart> UserCart { get; set; }
    }
}
