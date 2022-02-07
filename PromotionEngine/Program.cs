using PromotionEngine.Models;
using System;
using System.Collections.Generic;

namespace PromotionEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            PromotionManager engine = new PromotionManager();

            List<OrderLine> orderLines = new List<OrderLine>();

            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "A", Quantity = 3, ItemCost = 50 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "B", Quantity = 5, ItemCost = 30 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "C", Quantity = 1, ItemCost = 20 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "D", Quantity = 1, ItemCost = 15 });


            Order order = new Order();
            order.OrderId = Guid.NewGuid().ToString();
            order.OrderLines = orderLines;
            order.CreatedDate = DateTime.Now;

            var rules = engine.GetRules(order);
            var response = engine.ProcessRules(rules);

            foreach (var ol in orderLines)
            {
                Console.WriteLine($"Total cost for Orderline with Sku \"{ol.SKU}\" is {ol.TotalCost}, discount given is {ol.Discount}");
            }

        }
    }
}
