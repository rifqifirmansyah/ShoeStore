using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoeStore.Models
{
    public class UserCart
    {
        public int CartId { get; set; }
        public int ShoeId { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }

        public Shoe Shoe { get; set; }
    }
}
