using OperatorPrecedenceParser.Lexer;
using OperatorPrecedenceParser.Syntax_Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OperatorPrecedenceParser.Syntax_Tree.SyntaxNode;

namespace OperatorPrecedenceParser.Parser
{
    internal class OpParser
    {
        private Queue<Token> _tokens;
        private readonly OperatorPrecedence _precedence = new OperatorPrecedence();
        public OpParser(Queue<Token> tokens)
        {
            _tokens = tokens;
        }

        private Token Peek() => _tokens.Peek();
        private Token Consume() => _tokens.Dequeue();
        public SyntaxNode ParseExpression()
        {

            var left = ParseTerm();

            while (_tokens.Count > 0)
            {
                var opToken = Peek();
                if (opToken.Type == TokenType.Operator &&
                    _precedence.TryGetPrecedence(opToken.Value, out var opDetails))
                {
                    Consume();
                    var right = ParseTerm();

                    left = new BinaryOperationNode(left, right, opToken.Value);
                }
                else
                {
                    break;
                }
            }
            return left;
        }

        private SyntaxNode ParseTerm()
        {
            var left = ParseFactor();
            while (_tokens.Count > 0)
            {
                var op = Peek();
                if (op.Type == TokenType.Operator &&
                    _precedence.TryGetPrecedence(op.Value, out var opDetails) &&
                    (opDetails.Precedence == 2))
                {
                    Consume();
                    var right = ParseFactor();
                    left = new BinaryOperationNode(left, right, op.Value);
                }
                else
                {
                    break;
                }

            }
            return left;
        }

        private SyntaxNode ParseFactor()
        {

            if (_tokens.Count == 0)
            {
                throw new InvalidOperationException("Unexpected end of input");
            }

            if (Peek().Type == TokenType.Operator && Peek().Value == "-")
            {
                Consume();
                var right = ParseFactor();
                return new UnaryOperationNode(right, "-");
            }

            if (Peek().Type == TokenType.Number)
            {
                var numberToken = Consume();
                return new NumberNode(double.Parse(numberToken.Value));

            }

            else if (Peek().Type == TokenType.LeftParenthesis)
            {
                Consume();
                var node = ParseExpression();
                if (_tokens.Count > 0)
                {
                    bool r = Peek().Type == TokenType.RightParenthesis;
                    if (!r)
                    {
                        throw new Exception("Mismatched Parentheses");
                    }
                    Consume();

                }
                return node;
            }

            else if (Peek().Type == TokenType.Function)
            {
                var functionToken = Consume().Value;
                if (Peek().Type == TokenType.LeftParenthesis)
                {
                    Consume();
                    var argument = ParseArguments();
                    Consume();

                    var node = new FunctionCallNode(functionToken, argument);

                    return node;
                }
            }
            else if (Peek().Type == TokenType.End)
            {
                Consume();
            }

            throw new InvalidOperationException($"Unexpected Token: {Peek().Value}");
        }

        private ArgumentListNode ParseArguments()
        {
            var arguments = new List<SyntaxNode>();

            while (Peek().Type != TokenType.RightParenthesis)
            {
                var argument = ParseFactor();
                arguments.Add(argument);
                if (Peek().Type == TokenType.Comma)
                {
                    Consume();
                }
                else if (Peek().Type != TokenType.RightParenthesis)
                {
                    throw new InvalidOperationException("Expected comma or closing parenthesis");
                }
            }

            return new ArgumentListNode(arguments);
        }

        public void PrintSyntaxTree(SyntaxNode node, int level)
        {
            Console.WriteLine($"{new string(' ', level * 2)}{node}");
            foreach (var child in node.Children)
            {
                PrintSyntaxTree(child, level + 1);
            }
        }

        public void PrintAnnotatedParseTree(SyntaxNode node, string indent = "", bool last = true)
        {
            if (node == null) return;


            Console.WriteLine(indent + (last ? "└── " : "├── ") + node);
            indent += last ? "    " : "│   ";

            for (int i = 0; i < node.Children.Count; i++)
            {
                PrintAnnotatedParseTree(node.Children[i], indent, i == node.Children.Count - 1);
            }

        }
    }
}
