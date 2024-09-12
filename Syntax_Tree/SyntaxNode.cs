using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorPrecedenceParser.Syntax_Tree
{
    internal class SyntaxNode
    {
        public string Value { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<SyntaxNode> Children { get; set; } = new List<SyntaxNode>();

        public string? TypeAnnotation { get; set; }
        public object? SemanticValue { get; set; }

        public override string ToString()
        {
            return $"{Type}: {Value}";
        }

        public class NumberNode : SyntaxNode
        {
            public double Value { get; }

            public NumberNode(double value)
            {

                Value = value;
                Type = "Number";
                TypeAnnotation = value % 1 == 0 ? "Integer" : "Float";
            }

            public override string ToString()
            {
                return $"{Value} ({TypeAnnotation})";
            }
        }

        public class FunctionCallNode : SyntaxNode
        {
            public string FunctionName { get; }
            public ArgumentListNode Arguments { get; }

            public FunctionCallNode(string functionName, ArgumentListNode argumentList)
            {
                FunctionName = functionName;
                Arguments = argumentList;


                Type = "FunctionCall";
                TypeAnnotation = $"Function : {FunctionName}";
                SemanticValue = ComputeFunctionSemanticValue();
                Children.Add(argumentList);

            }


            public override string ToString()
            {
                return $"{FunctionName}({Arguments})";
            }

            public bool ValidateConstraints()
            {
                if (FunctionName == "log" || FunctionName == "exp")
                {
                    var arguments = Arguments._arguments;
                    if (arguments.Count == 2 && arguments[0] is NumberNode baseNode)
                    {
                        return baseNode.Value > 0;
                    }
                    return false;
                }

                return true;
            }

            private object ComputeFunctionSemanticValue()
            {

                var arguments = Arguments._arguments;
                switch (FunctionName)
                {
                    case "sin":
                        double sinVal = Math.Sin(DegreesToRadians(GetArgumentValue(arguments[0])));
                        return Math.Round(sinVal, 2);
                    case "cos":
                        double cosVal = Math.Cos(DegreesToRadians(GetArgumentValue(arguments[0])));
                        return Math.Round(cosVal, 2);
                    case "tan":
                        double tanVal = Math.Tan(DegreesToRadians(GetArgumentValue(arguments[0])));
                        return Math.Round(tanVal, 2);
                    case "log":

                        return Math.Log(GetArgumentValue(arguments[1]), GetArgumentValue(arguments[0]));
                    case "exp":
                        return Math.Pow(GetArgumentValue(arguments[0]), GetArgumentValue(arguments[1]));
                    default:
                        throw new InvalidOperationException($"Unknown function: {FunctionName}");
                }

            }

            private double DegreesToRadians(double value)
            {
                return (double)(Math.PI / 180) * value;
            }
            private double GetArgumentValue(SyntaxNode node)
            {
                if (node is NumberNode numberNode)
                {
                    return numberNode.Value;
                }

                throw new InvalidOperationException($"Invalid argument node type: {node.GetType()}");
            }

        }
        public class BinaryOperationNode : SyntaxNode
        {
            public SyntaxNode Left { get; }
            public SyntaxNode Right { get; }
            public string Operator { get; }
            public BinaryOperationNode(SyntaxNode left, SyntaxNode right, string op)
            {
                Left = left;

                Right = right;

                Operator = op;

                Type = "BinaryOperation";
                TypeAnnotation = GetOperationType(op);
                SemanticValue = ComputeSemanticValue();
                Children.Add(left);
                Children.Add(right);
            }

            private object ComputeSemanticValue()
            {
                double leftValue = GetNodeValue(Left);
                double rightValue = GetNodeValue(Right);

                return Operator switch
                {
                    "+" => leftValue + rightValue,
                    "-" => leftValue - rightValue,
                    "*" => leftValue * rightValue,
                    "/" => rightValue != 0 ? leftValue / rightValue : throw new DivideByZeroException("Division by zero"),
                    _ => throw new InvalidOperationException($"Unsupported operator {Operator}")
                };
            }

            private double GetNodeValue(SyntaxNode node)
            {
                if (node is NumberNode numberNode)
                {
                    return numberNode.Value;
                }

                if (node is UnaryOperationNode uNode)
                {
                    return (double)uNode.SemanticValue;
                }

                if (node is FunctionCallNode functionCallNode)
                {
                    return (double)(functionCallNode.SemanticValue);
                }

                if (node is BinaryOperationNode binaryOperationNode)
                {
                    return (double)binaryOperationNode.SemanticValue;
                }

                throw new InvalidOperationException("Invalid node type for value extraction");
            }


            private string GetOperationType(string op)
            {
                return op switch
                {
                    "+" => "Addition",
                    "-" => "Subtraction",
                    "*" => "Multiplication",
                    "/" => "Division",
                };
            }



            public override string ToString()
            {
                return $"({Left}{Operator}{Right})";
            }
        }

        public class UnaryOperationNode : SyntaxNode
        {
            public string _op { get; }
            public SyntaxNode _factor { get; }

            public UnaryOperationNode(SyntaxNode factor, string op)
            {
                _factor = factor;
                _op = op;
                Type = "UnaryOperation";
                TypeAnnotation = $"UnaryOperator: {op}";
                SemanticValue = ComputeSematicValue();
                Children.Add(factor);

            }
            public override string ToString()
            {
                return $"{_op}{_factor}";
            }

            private object? ComputeSematicValue()
            {
                var operandValue = GetNodeValue(_factor);
                return _op == "-" ? -operandValue : operandValue;
            }

            private double GetNodeValue(SyntaxNode node)
            {
                if (node is NumberNode numberNode)
                {
                    return numberNode.Value;
                }

                if (node is UnaryOperationNode unaryNode)
                {
                    return (double)unaryNode.SemanticValue;
                }

                throw new InvalidOperationException($"Invalid node type for value extraction {node}");
            }


        }

        public class ArgumentListNode : SyntaxNode
        {
            public List<SyntaxNode> _arguments { get; }

            public ArgumentListNode(List<SyntaxNode> arguments)
            {
                _arguments = arguments;
            }

            public override string ToString()
            {
                return string.Join(", ", _arguments.Select(arg => arg.ToString()));
            }

        }

    }
}
