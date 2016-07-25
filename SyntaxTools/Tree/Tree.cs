using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Tree
{
    /// <summary>
    /// Generic n-child tree
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Tree<T> : IEquatable<Tree<T>>
    {
        /// <summary>
        /// Create a new tree
        /// </summary>
        public Tree(T Value, IEnumerable<Tree<T>> Childs)
        {
            this.Value = Value;
            this.Childs = Childs.ToList();
        }

        /// <summary>
        /// Create a new tree
        /// </summary>
        public Tree(T Value, params Tree<T>[] Childs) : this(Value, (IEnumerable<Tree<T>>)Childs)
        {
        }

        /// <summary>
        /// Expression value. 
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Expression childrens
        /// </summary>
        public IReadOnlyList<Tree<T>> Childs { get; private set; }

        public bool Equals(Tree<T> other)
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
