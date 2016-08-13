using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Operators;

namespace SyntaxTools.Trees.Patterns
{
    /// <summary>
    /// Match a tree with a given value and a collection of 
    /// </summary>
    public class Leaf : TreePattern
    {
        public Leaf(OperatorToken Value, SequencePattern Sequence)
        {
            this.Value = Value;
            this.Sequence = Sequence;
        }

        /// <summary>
        /// Create a leaf that matches only trees without any child
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Sequence"></param>
        public Leaf(OperatorToken Value) : this(Value, new Sequence.Exact())
        {
        }

        /// <summary>
        /// The leaf value to match
        /// </summary>
        public readonly OperatorToken Value;

        /// <summary>
        /// The pattern used to match the tree children
        /// </summary>
        public readonly SequencePattern  Sequence;

        public override IReadOnlyList<MatchResult<string, ExpressionTree>> Match(ExpressionTree Tree)
        {
            //The value must be equal
            if (!Tree.Value.Equals(Value))
                return new MatchResult<string, ExpressionTree>[0];

            //Devuelve el resultado del sequence
            return Sequence.Match(Tree.Childs);
        }
        public override string ToString()
        {
            return Value.ToString() + " (" + Sequence.ToString() + ")";
        }
    }
}
