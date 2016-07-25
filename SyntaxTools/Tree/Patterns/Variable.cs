using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Tree.Patterns
{
    /// <summary>
    /// Match any tree without variable binding
    /// </summary>
    public class Any<T> : TreePattern<T>
    {
        public override IReadOnlyList<MatchResult<T, Tree<T>>> Match(Tree<T> Tree)
        {
             return new[] { new MatchResult<T, Tree<T>>() };
        }
    }

    /// <summary>
    /// Match any tree binding its value to a variable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Variable<T> : TreePattern<T>
    {
        public readonly T Key;
        public Variable(T Key)
        {
            this.Key = Key;
        }
        public override IReadOnlyList<MatchResult<T, Tree<T>>> Match(Tree<T> Tree)
        {
          return  new[] { new MatchResult<T, Tree<T>>(Key, Tree) };
        }
    }
}
