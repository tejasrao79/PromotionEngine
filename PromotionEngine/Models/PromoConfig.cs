using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Models
{
    /// <summary>
    /// Represents the active promotions in the system.
    /// </summary>
    public class PromoConfig
    {
        public List<Rule> Promos { get; set; }
    }
}
