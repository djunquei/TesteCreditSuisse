using System;
using System.Collections.Generic;
using System.Linq;

namespace TesteCreditSuisse
{
    public class Program
    {
        public static void Main()
        {
            List<Trade> trades = new List<Trade>();

            try
            {
                trades.Add(AddTrade(2000000, "Private"));
                trades.Add(AddTrade(400000, "Public"));
                trades.Add(AddTrade(500000, "Public"));
                trades.Add(AddTrade(3000000, "Public"));

                List<string> tradeCategories = GetTradeCateroies(trades);

                Console.WriteLine("[{0}]", string.Join(", ", tradeCategories.ToArray()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("O seguinte erro aconteceu: {0}", ex.Message);
            }
            Console.Read();
        }

        #region ITrade

        interface ITrade
        {
            double Value { get; set; }
            string ClientSector { get; set; }
        }

        #endregion

        #region Trade

        public class Trade : ITrade
        {
            public double Value { get; set; }

            public string ClientSector { get; set; }

        }

        /// <summary>
        /// Cria um objeto trade
        /// </summary>
        /// <param name="Value">Valor do trade</param>
        /// <param name="ClientSector">Tipo de cliente do trade</param>
        /// <returns>Retorna um objeto trade</returns>
        public static Trade AddTrade(double Value, string ClientSector)
        {
            Trade trade = new Trade();
            try
            {
                trade.Value = Value;
                trade.ClientSector = ClientSector;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(string.Format("Erro ao tentar adicionar um Trade. Mensagem de erro: {0}", ex.Message));
            }
            return trade;
        }

        /// <summary>
        /// Método para ter o resultado final das categorias
        /// </summary>
        /// <param name="trades">List de trade</param>
        /// <returns>List de string com as categorias de cada trade</returns>
        public static List<string> GetTradeCateroies(List<Trade> trades)
        {
            List<string> tradeCategories = new List<string>();

            try
            {
                List<RuleTrade> rulesTrade = new RuleTrade().GetRulesTrade();

                foreach (var item in trades)
                {
                    foreach (var ruleTradeItem in rulesTrade)
                    {
                        if (item.Value > ruleTradeItem.ValueInit && item.Value < ruleTradeItem.ValueEnd && item.ClientSector == ruleTradeItem.ClientSector)
                        {
                            tradeCategories.Add(ruleTradeItem.Category);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(string.Format("Erro ao tentar recuperar as categorias. Mensagem de erro: {0}", ex.Message));
            }

            return tradeCategories;
        }

        #endregion

        #region IRuleTrade
        
        interface IRuleTrade
        {
            Guid Id { get; set; }
            string Name { get; set; }
            double ValueInit { get; set; }
            double ValueEnd { get; set; }
            string ClientSector { get; set; }
            string Category { get; set; }

            List<RuleTrade> GetRulesTrade();
            void AddRuleTrade(ref List<RuleTrade> rules, Guid id, string name, double valueInit, double valueEnd, string clientSector, string category);
            void DeleteRuleTrade(ref List<RuleTrade> rules, Guid idRuleTrade);
        }

        #endregion

        #region RuleTrade
        
        public class RuleTrade : IRuleTrade
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public double ValueInit { get; set; }
            public double ValueEnd { get; set; }
            public string ClientSector { get; set; }
            public string Category { get; set; }

            /// <summary>
            /// Método para adicionar uma ruleTrade na lista de RulesTrade
            /// </summary>
            /// <param name="rulesTrade">List de RuleTrade</param>
            /// <param name="id">Guid do id da regra</param>
            /// <param name="name">String com o nome da regra</param>
            /// <param name="valueInit">Double com o valor inicial para a regra</param>
            /// <param name="valueEnd">Double com o valor final para a regra</param>
            /// <param name="clientSector">String com o tipo de clientSector</param>
            /// <param name="category">String com a categoria para a regra</param>
            public void AddRuleTrade(ref List<RuleTrade> rulesTrade, Guid id, string name, double valueInit, double valueEnd, string clientSector, string category)
            {
                try
                {
                    rulesTrade.Add(new RuleTrade
                    {
                        Id = id,
                        Name = name,
                        ValueInit = valueInit,
                        ValueEnd = valueEnd,
                        ClientSector = clientSector,
                        Category = category
                    });
                }
                catch (Exception ex)
                {
                    throw new System.ArgumentException(string.Format("Erro ao tentar adicionar uma RuleTrade. Mensagem de erro: {0}", ex.Message));
                }

            }

            /// <summary>
            /// Método para deletar uma ruleTrade da lista de RulesTrade
            /// </summary>
            /// <param name="rulesTrade">List de RuleTrade</param>
            /// <param name="idRuleTrade">Guid do id da regra que será deletada</param>
            public void DeleteRuleTrade(ref List<RuleTrade> rulesTrade, Guid idRuleTrade)
            {
                try
                {
                    RuleTrade ruleTradeRemove = rulesTrade.Where(x => x.Id == idRuleTrade).FirstOrDefault();
                    if (ruleTradeRemove != null)
                    {
                        rulesTrade.Remove(ruleTradeRemove);
                    }
                }
                catch (Exception ex)
                {
                    throw new System.ArgumentException(string.Format("Erro ao tentar deletar uma RuleTrade. Mensagem de erro: {0}", ex.Message));
                }
            }

            /// <summary>
            /// Método para recuperar todas RulesTrade
            /// </summary>
            /// <returns>List de RuleTrade</returns>
            public List<RuleTrade> GetRulesTrade()
            {
                List<RuleTrade> rulesTrade = new List<RuleTrade>();

                try
                {
                    //Add Rules
                    AddRuleTrade(ref rulesTrade, new Guid("a72a6c78-00f8-4cef-b5d1-60017623c1e0"), "Rule LOWRISK", 0, 1000000, "Public", "LOWRISK");
                    AddRuleTrade(ref rulesTrade, new Guid("dae3c80e-277d-4ff0-ae0b-f217ae242cfb"), "Rule MEDIUMRISK", 1000000, 1000000000, "Public", "MEDIUMRISK");
                    AddRuleTrade(ref rulesTrade, new Guid("1f84850e-843c-4ab3-b8bc-4a99aadaeec7"), "Rule HIGHRISK", 1000000, 1000000000, "Private", "HIGHRISK");

                    //Remove Rules
                    AddRuleTrade(ref rulesTrade, new Guid("89d3e0e6-440a-47ff-9a33-8788aaba024e"), "Rule DELETE", 1000000, 1000000000, "Private", "DELETE TEST");
                    DeleteRuleTrade(ref rulesTrade, new Guid("89d3e0e6-440a-47ff-9a33-8788aaba024e"));
                }
                catch (Exception ex)
                {
                    throw new System.ArgumentException(string.Format("Erro ao tentar recuperar as RulesTrade. Mensagem de erro: {0}", ex.Message));
                }

                return rulesTrade;
            }
        }

        #endregion

    }

}
