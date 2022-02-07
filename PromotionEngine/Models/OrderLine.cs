using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Models
{
    /// <summary>
    /// Represents an item in shoping cart.
    /// </summary>
    public class OrderLine
    {
        public string OrderLineId { get; set; }

        public string SKU { get; set; }

        public string OrderLineType { get; set; }

        public int Quantity { get; set; }

        public decimal ItemCost { get; set; }

        public decimal ItemTax { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal ShippingTax { get; set; }

        public decimal Discount { get; set; }

        public decimal TotalCost { get { return ((ItemCost + ItemTax) * Quantity)  + ShippingCost + ShippingTax - Discount; } }
    }
}
