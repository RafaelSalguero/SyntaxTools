using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Precedence;

namespace SyntaxTools.Operators
{
    public enum OperatorArgumentPosition
    {
        Binary,
        PrefixUnary,
        PostfixUnary
    }

    public enum OperatorAssociativity
    {
        Left,
        Right
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
        public Operator(int Precedence, OperatorArgumentPosition OperatorType, OperatorAssociativity Associativity, int ArgumentCount, Guid Symbol, Guid Id)
        {
            this.Precedence = Precedence;
            this.OperatorType = OperatorType;
            this.Symbol = Symbol;
            this.Id = Id;
            this.Associativity = Associativity;
            this.ArgumentCount = ArgumentCount;
        }

        /// <summary>
        /// Create a new operator definition with the operator Id equal to the operator symbol
        /// <param name="Name">Operator friendly name</param>
        /// </summary>
        public Operator(int Precedence, OperatorArgumentPosition OperatorType, OperatorAssociativity Associativity, int ArgumentCount, Guid Symbol) : this(Precedence, OperatorType, Associativity, ArgumentCount, Symbol, Symbol)
        {
        }

        /// <summary>
        /// Operator associativity
        /// </summary>
        public OperatorAssociativity Associativity { get; private set; }

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

        /// <summary>
        /// Operator argument count
        /// </summary>
        public int ArgumentCount { get; set; }




        public override string ToString()
        {
            return Global.GuidNames.GetName(Id);
        }
    }
}
