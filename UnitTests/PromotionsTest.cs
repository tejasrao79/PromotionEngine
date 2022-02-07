using NUnit.Framework;
using PromotionEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class Tests
    {
        private PromotionEngine.PromotionManager engine;
        private Order order1;
        private Order order2;
        private Order order3;

        [SetUp]
        public void Setup()
        {
            engine = new PromotionEngine.PromotionManager();
            DateTime now = DateTime.Now;
            List<OrderLine> orderLines = new List<OrderLine>();

            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "A", Quantity = 1, ItemCost = 50 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "B", Quantity = 1, ItemCost = 30 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "C", Quantity = 1, ItemCost = 20 });

            order1 = new Order() { OrderId = Guid.NewGuid().ToString("n"), CreatedDate = now, OrderLines = orderLines };

            orderLines = new List<OrderLine>();
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "A", Quantity = 5, ItemCost = 50 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "B", Quantity = 5, ItemCost = 30 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "C", Quantity = 1, ItemCost = 20 });

            order2 = new Order() { OrderId = Guid.NewGuid().ToString("n"), CreatedDate = now, OrderLines = orderLines };

            orderLines = new List<OrderLine>();
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "A", Quantity = 3, ItemCost = 50 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "B", Quantity = 5, ItemCost = 30 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "C", Quantity = 1, ItemCost = 20 });
            orderLines.Add(new OrderLine { OrderLineId = Guid.NewGuid().ToString("n"), SKU = "D", Quantity = 1, ItemCost = 15 });

            order3 = new Order() { OrderId = Guid.NewGuid().ToString("n"), CreatedDate = now, OrderLines = orderLines };
        }

        [Test]
        public void CouponEngine_NoPromos()
        {
            var rules = engine.GetRules(order1);

            Assert.IsTrue(rules.Count == 0, "Rules should be empty.");
        }

        [Test]
        public void CouponEngine_FixedPriceDiscount()
        {
            var rules = engine.GetRules(order2);
            engine.ProcessRules(rules);
            OrderLine orderLineA = order2.OrderLines.Where(ol => ol.SKU == "A").FirstOrDefault();
            OrderLine orderLineB = order2.OrderLines.Where(ol => ol.SKU == "B").FirstOrDefault();

            Assert.IsTrue(rules.Count == 2, "Invalid number of rules returned.");
            Assert.IsNotNull(orderLineA, "OrderLineA should not be null.");
            Assert.IsNotNull(orderLineB, "OrderLineB should not be null.");
            Assert.AreEqual(20, orderLineA.Discount, "Discount amount is incorrect for Orderline A.");
            Assert.AreEqual(30, orderLineB.Discount, "Discount amount is incorrect for Orderline B.");

        }

        [Test]
        public void CouponEngine_MultiSkuDiscount()
        {
            var rules = engine.GetRules(order3);
            engine.ProcessRules(rules);
            OrderLine orderLineC = order3.OrderLines.Where(ol => ol.SKU == "C").FirstOrDefault();
            OrderLine orderLineD = order3.OrderLines.Where(ol => ol.SKU == "D").FirstOrDefault();

            Assert.IsTrue(rules.Count == 3, "Invalid number of rules returned.");
            Assert.IsNotNull(orderLineC, "OrderLineC should not be null.");
            Assert.IsNotNull(orderLineD, "OrderLineD should not be null.");
            Assert.AreEqual(20, orderLineC.Discount, "Discount amount is incorrect for Orderline C.");
            Assert.AreEqual(-15, orderLineD.Discount, "Discount amount is incorrect for Orderline D.");

        }
    }
}