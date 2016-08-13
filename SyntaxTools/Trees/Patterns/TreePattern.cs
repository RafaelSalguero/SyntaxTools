using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Trees.Patterns
{
    /// <summary>
    /// Base class for tree collection patterns
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SequencePattern
    {
        /// <summary>
        /// Match a sequence of trees
        /// </summary>
        public abstract IReadOnlyList<MatchResult<string, ExpressionTree>> Match(IReadOnlyList<ExpressionTree> Sequence);
    }

    /// <summary>
    /// Base class for tree patterns
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TreePattern
    {
        /// <summary>
        /// Match this pattern to a tree
        /// </summary>
        /// <param name="Tree"></param>
        public abstract IReadOnlyList<MatchResult<string, ExpressionTree>> Match(ExpressionTree Tree);

        /// <summary>
        /// Execute a condition that have access to the global match results after a successful match which determines if this pattern still matches 
        /// </summary>
        /// <param name="Context">Variable bindings</param>
        /// <returns></returns>
        public Func<MatchResult<string, ExpressionTree>, bool> Condition;
    }
}
