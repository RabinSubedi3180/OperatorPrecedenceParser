using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorPrecedenceParser.Lexer
{
    internal class Lexer
    {
        private readonly string _input;
        private int _index;
        public Lexer(string input)
        {
            _input = input;
            _index = 0;
        }

        public Queue<Token> Tokenize()
        {
            var tokens = new Queue<Token>();
            while (_index < _input.Length)
            {
                char currentChar = _input[_index];

                if (char.IsDigit(currentChar))
                {
                    string number = ReadWhile(char.IsDigit);
                    tokens.Enqueue(new Token(TokenType.Number, number));
                }
                else if (currentChar == '-')
                {
                    tokens.Enqueue(new Token(TokenType.Operator, currentChar.ToString()));
                    _index++;
                }
                else if (currentChar == '+')
                {
                    tokens.Enqueue(new Token(TokenType.Operator, currentChar.ToString()));
                    _index++;
                }
                else if (currentChar == '*')
                {
                    tokens.Enqueue(new Token(TokenType.Operator, currentChar.ToString()));
                    _index++;
                }
                else if (currentChar == '/')
                {
                    tokens.Enqueue(new Token(TokenType.Operator, currentChar.ToString()));
                    _index++;
                }
                else if (currentChar == '(')
                {
                    tokens.Enqueue(new Token(TokenType.LeftParenthesis, currentChar.ToString()));
                    _index++;
                }
                else if (currentChar == ')')
                {
                    tokens.Enqueue(new Token(TokenType.RightParenthesis, currentChar.ToString()));
                    _index++;
                }
                else if (currentChar == ',')
                {
                    tokens.Enqueue(new Token(TokenType.Comma, currentChar.ToString()));
                    _index++;
                }
                else if (char.IsWhiteSpace(currentChar))
                {
                    _index++;
                }
                else if (char.IsLetter(currentChar))
                {
                    string buffer = ReadWhile(char.IsLetter);
                    if (buffer == "sin" || buffer == "cos" || buffer == "tan" || buffer == "log" || buffer == "exp")
                    {
                        tokens.Enqueue(new Token(TokenType.Function, buffer));
                    }

                    else
                    {
                        throw new Exception($"Invalid function: {buffer}");

                    }

                }
                else
                {
                    throw new Exception($"Invalid Character: {currentChar}");
                }

            }
            tokens.Enqueue(new Token(TokenType.End, "$"));
            return tokens;

        }




        private string ReadWhile(Func<char, bool> condition)
        {
            int startPosition = _index;
            while (_index < _input.Length && condition(_input[_index]))
            {
                _index++;
            }

            return _input.Substring(startPosition, _index - startPosition);
        }
    }
}
