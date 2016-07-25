using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Operators
{
    public enum OperatorArgumentPosition
    {
        Binary, 
        PrefixUnary,
        PostfixUnary
    }

    /// <summary>
    /// An operator definition. This class is immutable
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// Create a new operator definition
        /// <param name="Name">Operator friendly name</param>
        /// </summary>
        public Operator( int Precedence, OperatorArgumentPosition OperatorType, Guid Symbol, Guid Id)
        {
            this.Precedence = Precedence;
            this.OperatorType = OperatorType;
            this.Symbol = Symbol;
            this.Id = Id;
        }

        /// <summary>
        /// Create a new operator definition with the operator Id equal to the operator symbol
        /// <param name="Name">Operator friendly name</param>
        /// </summary>
        public Operator(int Precedence, OperatorArgumentPosition OperatorType, Guid Symbol) : this(Precedence, OperatorType, Symbol, Symbol)
        {
        }

        /// <summary>
        /// Operator precedence
        /// </summary>
        public int Precedence { get; private set; }
        /// <summary>
        /// Operator argument count and position
        /// </summary>
        public OperatorArgumentPosition OperatorType { get; private set; }

        /// <summary>
        /// The symbol that lexically identifies this operator
        /// </summary>
        public Guid Symbol { get; private set; }

        /// <summary>
        /// Unique identifier for this operator
        /// </summary>
        public Guid Id { get; private set; }

    
        
        public override string ToString()
        {
            return Global.GuidNames.GetName(Id);
        }
    }
}
