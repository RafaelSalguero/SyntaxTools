using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Trees.Patterns
{
    /// <summary>
    /// Match a tree with a given value and a collection of 
    /// </summary>
    public class Leaf<T> : TreePattern<T>
    {
        public Leaf(T Value, SequencePattern<T> Sequence)
        {
            this.Value = Value;
            this.Sequence = Sequence;
        }

        /// <summary>
        /// The leaf value to match
        /// </summary>
        public readonly T Value;

        /// <summary>
        /// The pattern used to match the tree children
        /// </summary>
        public readonly SequencePattern<T> Sequence;

        public override IReadOnlyList<MatchResult<T, Tree<T>>> Match(Tree<T> Tree)
        {
            //The value must be equal
            if (!Tree.Value.Equals(Value))
                return new MatchResult<T, Tree<T>>[0];

            //Devuelve el resultado del sequence
            return Sequence.Match(Tree.Childs);
        }
    }
}
