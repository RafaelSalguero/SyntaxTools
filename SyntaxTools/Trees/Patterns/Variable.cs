using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Operators;

namespace SyntaxTools.Trees.Patterns
{
    /// <summary>
    /// Match any tree without variable binding
    /// </summary>
    public class Any : TreePattern
    {
        public override IReadOnlyList<MatchResult<string, ExpressionTree>> Match(ExpressionTree Tree)
        {
            return new[] { new MatchResult<string, ExpressionTree>() };
        }

        public override string ToString()
        {
            return "?";
        }
    }

    /// <summary>
    /// Match any tree binding its value to a variable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Variable : TreePattern
    {
        public readonly string Key;
        public Variable(string Key)
        {
            this.Key = Key;
        }
        public override IReadOnlyList<MatchResult<string, ExpressionTree>> Match(ExpressionTree Tree)
        {
            return new[] { new MatchResult<string, ExpressionTree>(Key, Tree) };
        }

        public override string ToString()
        {
            return Key.ToString() + "?";
        }
    }
}
