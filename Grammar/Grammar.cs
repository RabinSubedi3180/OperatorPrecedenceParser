using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorPrecedenceParser.Grammar
{
    internal class Grammar
    {
        public Dictionary<string, List<Production>> Rules { get; }
        public string StartSymbol { get; }
        public List<Production> Productions { get; }

        public Grammar(string startSymbol)
        {
            StartSymbol = startSymbol;
            Rules = new Dictionary<string, List<Production>>();
            Productions = new List<Production>();

        }
        public void AddRule(string nonTerminal, List<string> production)
        {

            if (!Rules.ContainsKey(nonTerminal))
            {
                Rules[nonTerminal] = new List<Production>();
            }
            var prod = new Production(nonTerminal, production);
            Productions.Add(prod);

            Rules[nonTerminal].Add(prod);


        }

        public Production GetProductionByIndex(int index)
        {
            var result = new List<Production>();
            if (index >= 0 && index < Productions.Count)
            {
                return Productions[index];
            }
            throw new ArgumentOutOfRangeException("index");
        }

        public List<Production> GetProductions(string nonTerminal)
        {
            return Rules.ContainsKey(nonTerminal) ? Rules[nonTerminal] : new List<Production>();
        }
        public HashSet<string> GetNonTerminals()
        {
            return new HashSet<string>(Rules.Keys);
        }
        public HashSet<string> GetTerminals()
        {
            var terminals = new HashSet<string>();
            terminals.Add("$");
            foreach (var productions in Rules.Values)
            {
                foreach (var production in productions)
                {
                    foreach (var symbol in production.RightSide)
                        if (!Rules.ContainsKey(symbol))
                        {
                            terminals.Add(symbol);
                        }
                }
            }
            return terminals;
        }



        public List<Production> GetProductionFor(string nonTerminal)
        {
            if (Rules.ContainsKey(nonTerminal))
            {
                return Rules[nonTerminal];
            }
            return new List<Production>();

        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Start Symbol: {StartSymbol}");
            foreach (var rules in Rules)
            {
                foreach (var production in rules.Value)
                {
                    sb.AppendLine(production.ToString());

                }

            }

            return sb.ToString();
        }
    
    }
}
