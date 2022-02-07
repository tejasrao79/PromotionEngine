using PromotionEngine.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PromotionEngine.Interface
{
    public interface IPromotionManager
    {
        List<IPromoRule> GetRules(Order order);

        List<PromoResponse> ProcessRules(List<IPromoRule> rules);

    }
}
