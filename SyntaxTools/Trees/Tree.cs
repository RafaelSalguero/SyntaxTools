using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Operators;

namespace SyntaxTools.Trees
{

    public interface ITree<T>
    {
        T Value { get; }
        IReadOnlyList<ITree<T>> Childs { get; }
    }
  

    /// <summary>
    /// Generic n-child tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExpressionTree : IEquatable<ExpressionTree>, ITree<OperatorToken>
    {
        /// <summary>
        /// Create a new tree
        /// </summary>
        public ExpressionTree(OperatorToken Value, IEnumerable<ExpressionTree> Childs)
        {
            this.Value = Value;
            this.Childs = Childs.ToList();
        }

        /// <summary>
        /// Create a new tree
        /// </summary>
        public ExpressionTree(OperatorToken Value, params ExpressionTree[] Childs) : this(Value, (IEnumerable<ExpressionTree>)Childs)
        {
        }

        /// <summary>
        /// Expression value. 
        /// </summary>
        public OperatorToken Value { get; private set; }

        /// <summary>
        /// Expression childrens
        /// </summary>
        public IReadOnlyList<ExpressionTree> Childs { get; private set; }

        IReadOnlyList<ITree<OperatorToken>> ITree<OperatorToken>.Childs
        {
            get
            {
                return Childs;
            }
        }

        public bool Equals(ExpressionTree other)
        {
            if (!Value.Equals(other.Value))
                return false;

            if (Childs.Count != other.Childs.Count)
                return false;

            for (var i = 0; i < Childs.Count; i++)
            {
                if (!Childs[i].Equals(other.Childs[i]))
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            return Value.ToString() + (Childs.Count > 0 ? " (" + Childs.Select(x => x.ToString()).Aggregate("", (a, b) => a == "" ? b : a + ", " + b) + ")" : "");
        }
    }
}
