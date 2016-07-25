using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Tree.Patterns
{
    /// <summary>
    /// Base class for tree collection patterns
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SequencePattern<T>
    {
        /// <summary>
        /// Match a sequence of trees
        /// </summary>
        public abstract IReadOnlyList<MatchResult<T, Tree<T>>> Match(IReadOnlyList<Tree<T>> Sequence);
    }

    /// <summary>
    /// Base clas for tree patterns
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public  abstract class TreePattern<T>
    {
        /// <summary>
        /// Match this pattern to a tree
        /// </summary>
        /// <param name="Tree"></param>
        public abstract IReadOnlyList<MatchResult<T, Tree<T>>> Match(Tree<T> Tree);
        
        /// <summary>
        /// Execute a condition that have access to the global match results after a successful match which determines if this pattern still matches 
        /// </summary>
        /// <param name="Context">Variable bindings</param>
        /// <returns></returns>
        public Func<MatchResult<T,Tree<T>>,bool> Condition;
    }
}
