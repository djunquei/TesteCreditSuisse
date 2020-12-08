using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static TesteCreditSuisse.Program;

namespace TesteCreditSuisse.UnitTest
{
    [TestClass]
    public class UnitTestCreditSuisse
    {
        [TestMethod]
        public void TestMethodAddTrade()
        {
            Trade trade = AddTrade(2000000, "Private");
        }

        [TestMethod]
        public void TestMethodAddRuleTrade()
        {
            List<RuleTrade> rulesTrade = new List<RuleTrade>();
            RuleTrade ruleTrade = new RuleTrade();
            ruleTrade.AddRuleTrade(ref rulesTrade, new Guid("a72a6c78-00f8-4cef-b5d1-60017623c1e0"), "Rule LOWRISK", 0, 1000000, "Public", "LOWRISK");
            Assert.AreEqual(1, rulesTrade.Count);
        }

        [TestMethod]
        public void TestMethodDeleteRuleTrade()
        {
            List<RuleTrade> rulesTrade = new List<RuleTrade>();
            RuleTrade ruleTrade = new RuleTrade();

            ruleTrade.AddRuleTrade(ref rulesTrade, new Guid("a72a6c78-00f8-4cef-b5d1-60017623c1e0"), "Rule LOWRISK", 0, 1000000, "Public", "LOWRISK");
            Assert.AreEqual(1, rulesTrade.Count);

            ruleTrade.DeleteRuleTrade(ref rulesTrade, new Guid("a72a6c78-00f8-4cef-b5d1-60017623c1e0"));
            Assert.AreEqual(0, rulesTrade.Count);
        }

        [TestMethod]
        public void TestMethodGetRulesTrade()
        {
            RuleTrade ruleTrade = new RuleTrade();
            List<RuleTrade> rulesTrade = ruleTrade.GetRulesTrade();
            Assert.AreEqual(3, rulesTrade.Count);
        }

        [TestMethod]
        public void TestMethodGetTradeCateroies()
        {
            List<Trade> trades = new List<Trade>();

            List<string> tradeCategories = GetTradeCateroies(trades);
            Assert.AreEqual(0, tradeCategories.Count);

            trades.Add(AddTrade(2000000, "Private"));
            trades.Add(AddTrade(400000, "Public"));
            trades.Add(AddTrade(500000, "Public"));
            trades.Add(AddTrade(3000000, "Public"));

            tradeCategories = GetTradeCateroies(trades);
            Assert.AreEqual(4, tradeCategories.Count);

            List<string> resultTradeCategories = new List<string>();
            resultTradeCategories.Add("HIGHRISK");
            resultTradeCategories.Add("LOWRISK");
            resultTradeCategories.Add("LOWRISK");
            resultTradeCategories.Add("MEDIUMRISK");

            CollectionAssert.AreEqual(resultTradeCategories, tradeCategories);
        }
    }
}
