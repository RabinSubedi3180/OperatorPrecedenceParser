using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OperatorPrecedenceParser.Syntax_Tree
{
    internal class AbstractSyntaxTree
    {
        public SyntaxNode Root { get; set; }

        public AbstractSyntaxTree(SyntaxNode root)
        {
            Root = root;
        }

        public void PrintTree()
        {
            PrintNode(Root, 0);
        }

        private void PrintNode(SyntaxNode node, int indentLevel)
        {
            var indent = new string(' ', indentLevel * 4);
            Console.WriteLine($"{indent}{node.Type}:{node.Value}");
            foreach (var child in node.Children)
            {
                PrintNode(child, indentLevel + 1);
            }
        }
    }
}
