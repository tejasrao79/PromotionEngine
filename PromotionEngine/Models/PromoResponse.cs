using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Models
{
    /// <summary>
    /// Represents the promotion results after processing a promotion for an order.
    /// </summary>
    public class PromoResponse
    {
        public List<PromoResult> Results { get; private set; }

        public PromoResponse()
        {
            this.Results = new List<PromoResult>();
        }
    }

    public class PromoResult
    {
        public string OrderlineId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string SKU { get; set; }
    }
}