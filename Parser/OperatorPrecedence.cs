using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorPrecedenceParser.Parser
{
    internal class OperatorPrecedence
    {
        public enum Associativity
        {
            Left,
            Right
        }

        private readonly Dictionary<string, (int Precedence, Associativity Associativity)> _operators = new Dictionary<string, (int, Associativity)>
    {
        { "+", (1, Associativity.Left) },
        { "-", (1, Associativity.Left) },
        { "*", (2, Associativity.Left) },
        { "/", (2, Associativity.Left) },
        { "sin", (3, Associativity.Left) },
        { "cos", (3, Associativity.Left) },
        { "tan", (3, Associativity.Left) },
        { "log", (3, Associativity.Left) },
        { "exp", (3, Associativity.Left) }
    };

        public bool TryGetPrecedence(string op, out (int Precedence, Associativity Associativity) result)
        {
            return _operators.TryGetValue(op, out result);
        }
    }
}
