using PromotionEngine.Interface;
using PromotionEngine.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PromotionEngine.RuleEngine;

namespace PromotionEngine
{
    public class PromotionManager : IPromotionManager
    {
        public List<Rule> Promos { get; set; }
        public PromotionManager()
        {
            LoadRules();
        }

        private void LoadRules()
        {
            using (StreamReader reader = new StreamReader(@".\Data\Promos.json"))
            {
                string json = reader.ReadToEnd();
                PromoConfig config = JsonConvert.DeserializeObject<PromoConfig>(json);
                this.Promos = config.Promos;
            }
        }

        private IPromoRule GetPromo(Order order, Rule rule)
        {
            switch (rule.RuleType)
            {
                case RuleType.FixedPriceDiscount:
                    return new FixedPriceDiscountProcessor(order, rule);
                case RuleType.MultiSkuDiscount:
                    return new MultiSKUDiscountProcessor(order, rule);
                default:
                    throw new InvalidOperationException($"RuleType {rule.RuleType} is not valid.");
            }
        }

        public List<IPromoRule> GetRules(Order order)
        {
            List<IPromoRule> rules = new List<IPromoRule>();
            try
            {
                List<string> SkuswithPromo = new List<string>();

                foreach (var item in Promos)
                {
                    if (SkuswithPromo.Any(x => item.SKU.Contains(x)))
                        continue;

                    IPromoRule rule = GetPromo(order, item);
                    if (rule.IsRuleApplicable())
                    {
                        rules.Add(rule);
                        SkuswithPromo.AddRange(item.SKU);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while getting rules. Error details: {ex.Message}, {ex.StackTrace}");
            }


            return rules;
        }

        public List<PromoResponse> ProcessRules(List<IPromoRule> rules)
        {
            List<PromoResponse> response = new List<PromoResponse>();

            try
            {
                foreach (var rule in rules)
                {
                    response.Add(rule.ProcessRule());
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while processing rules. Error details: {ex.Message}, {ex.StackTrace}");
            }

            return response;
        }
    }
}
