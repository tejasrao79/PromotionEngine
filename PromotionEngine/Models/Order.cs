using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Models
{
    /// <summary>
    /// Represents an order in the cart.
    /// </summary>
    public class Order
    {
        public string OrderId { get; set; }

        public List<OrderLine> OrderLines { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
