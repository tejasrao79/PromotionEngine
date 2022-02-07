using PromotionEngine.Interface;
using PromotionEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromotionEngine.RuleEngine
{
    /// <summary>
    /// This class holds the implementation for applying promotions for multiple items in cart.
    /// Ex: Price of 10 pens + notebooks is $15.
    /// </summary>
    public class MultiSKUDiscountProcessor : IPromoRule
    {
        public Order Order { get; set; }
        public Rule Rule { get; set; }

        public MultiSKUDiscountProcessor(Order order, Rule rule)
        {
            this.Order = order;
            this.Rule = rule;
        }

      
        public bool IsRuleApplicable()
        {
            DateTime now = DateTime.Now;
            if (now < Rule.StartDate || now > Rule.EndDate)
                return false;

            foreach (var sku in this.Rule.SKU)
            {
                if (!this.Order.OrderLines.Any(ol => ol.SKU == sku && this.Rule.Quantity <= ol.Quantity))
                    return false;
            }

            return true;
        }


        public PromoResponse ProcessRule()
        {
            PromoResponse response = new PromoResponse();

            var orderLines = this.Order.OrderLines.Where(ol => this.Rule.SKU.Contains(ol.SKU));
            var olToApply = orderLines.Last();

            var quantity = orderLines.Min(ol => ol.Quantity);
            var count = quantity / this.Rule.Quantity;

            PromoResult result = new PromoResult();
            result.OrderlineId = olToApply.OrderLineId;
            result.Quantity = quantity;
            result.Price = count * this.Rule.Price;
            result.SKU = olToApply.SKU;

            olToApply.Discount = (result.Quantity * olToApply.ItemCost) - (count * this.Rule.Price);

            response.Results.Add(result);

            foreach (var ol in orderLines)
            {
                if (ol.Equals(olToApply))
                    continue;

                result = new PromoResult();
                result.OrderlineId = ol.OrderLineId;
                result.Quantity = quantity;
                result.Price = -(count * ol.ItemCost);
                result.SKU = ol.SKU;

                ol.Discount = result.Quantity * ol.ItemCost;

                response.Results.Add(result);
            }

            return response;
        }
    }
}
