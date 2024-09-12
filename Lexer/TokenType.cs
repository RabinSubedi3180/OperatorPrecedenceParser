using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorPrecedenceParser.Lexer
{
    internal enum TokenType
    {
        Number,
        Operator,
        LeftParenthesis,
        RightParenthesis,
        Function,
        Comma,
        End
    }
}
