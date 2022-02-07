using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Models
{
    /// <summary>
    /// Holds the promotion details.
    /// </summary>
    public class Rule
    {
        public List<String> SKU { get; set; }
        public RuleType RuleType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public enum RuleType
    {
        FixedPriceDiscount,
        MultiSkuDiscount
    }
}
