using PromotionEngine.Interface;
using PromotionEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromotionEngine.RuleEngine
{
    /// <summary>
    /// This class holds the implementation for applying promotions for items with fixed price discount.
    /// Ex: 100 pens at $10.
    /// </summary>
    public class FixedPriceDiscountProcessor : IPromoRule
    {
        public Order Order { get; set; }
        public Rule Rule { get; set; }
        public PromoResponse PromoResponse { get; set; }

        public FixedPriceDiscountProcessor(Order order, Rule rule)
        {
            this.Order = order;
            this.Rule = rule;
        }

        public bool IsRuleApplicable()
        {
            DateTime now = DateTime.Now;
            if (now < Rule.StartDate || now > Rule.EndDate)
                return false;

            return this.Order.OrderLines.Any(ol => this.Rule.SKU.Contains(ol.SKU) && this.Rule.Quantity <= ol.Quantity && this.Rule.RuleType == RuleType.FixedPriceDiscount);
        }

        public PromoResponse ProcessRule()
        {
            PromoResponse response = new PromoResponse();

            var orderLines = this.Order.OrderLines.Where(ol => this.Rule.SKU.Contains(ol.SKU) && this.Rule.Quantity <= ol.Quantity && this.Rule.RuleType == RuleType.FixedPriceDiscount);

            foreach (var orderLine in orderLines)
            {
                var count = orderLine.Quantity / this.Rule.Quantity;

                PromoResult result = new PromoResult();
                result.Quantity = count * this.Rule.Quantity;
                result.Price = count * this.Rule.Price;
                result.OrderlineId = orderLine.OrderLineId;
                result.SKU = orderLine.SKU;

                orderLine.Discount = (result.Quantity * orderLine.ItemCost) - result.Price;

                response.Results.Add(result);
            }

            this.PromoResponse = response;
            return response;
        }

    }
}
