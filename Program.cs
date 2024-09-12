using OperatorPrecedenceParser.Grammar;
using OperatorPrecedenceParser.Lexer;
using OperatorPrecedenceParser.Parser;
class Program
{

    static void Main()
    {

        var grammar = new Grammar("E");
        grammar.AddRule("E", new List<string> { "E", "+", "T" });
        grammar.AddRule("E", new List<string> { "E", "-", "T" });
        grammar.AddRule("E", new List<string> { "T" });
        grammar.AddRule("T", new List<string> { "T", "*", "F" });
        grammar.AddRule("T", new List<string> { "T", "/", "F" });
        grammar.AddRule("T", new List<string> { "F" });
        grammar.AddRule("F", new List<string> { "(", "E", ")" });
        grammar.AddRule("F", new List<string> { "number" });
        grammar.AddRule("F", new List<string> { "X", "(", "A", ")" });
        grammar.AddRule("A", new List<string> { "number", ",", "number" });
        grammar.AddRule("A", new List<string> { "number" });
        grammar.AddRule("X", new List<string> { "sin" });
        grammar.AddRule("X", new List<string> { "cos" });
        grammar.AddRule("X", new List<string> { "tan" });
        grammar.AddRule("X", new List<string> { "exp" });
        grammar.AddRule("X", new List<string> { "log" });


        Console.WriteLine("Enter an expression");
        string input = Console.ReadLine();

        Lexer lexer = new Lexer(input);
        Queue<Token> tokens = lexer.Tokenize();

        Console.WriteLine("Tokens:");
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }

        OpParser parser = new OpParser(tokens);
        var syntaxTree = parser.ParseExpression();
        Console.WriteLine("Annotated Parse Tree:\n");
        parser.PrintAnnotatedParseTree(syntaxTree);
        Console.WriteLine($"Value: {syntaxTree.SemanticValue}");

    }


}
