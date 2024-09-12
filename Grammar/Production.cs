using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorPrecedenceParser.Grammar
{
    internal class Production
    {
        public string NonTerminal { get; }
        public List<string> RightSide { get; }
        public Production(string nonTerminal, List<string> rightSide)
        {
            NonTerminal = nonTerminal;
            RightSide = rightSide;

        }

        public override string ToString()
        {
            return $"{NonTerminal} -> {string.Join(" ", RightSide)}";
        }
    }
}
