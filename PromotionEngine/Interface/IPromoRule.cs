using PromotionEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Interface
{
    public interface IPromoRule
    {
        public Order Order { get; set; }
        public Rule Rule { get; set; }
        bool IsRuleApplicable();
        PromoResponse ProcessRule();
    }
}
