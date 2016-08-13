using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;
using SyntaxTools.Text;

namespace SyntaxTools.Operators
{
    /// <summary>
    /// Output from the operator solver
    /// </summary>
    public class OperatorToken : TokenSubstring, IEquatable<OperatorToken>
    {
        public OperatorToken(TokenSubstring Other, Operator Operator) : base(Other.Symbol, Other.Substring)
        {
            this.Operator = Operator;
        }

        /// <summary>
        /// Create a new operator token
        /// </summary>
        /// <param name="Symbol">The operator token symbol</param>
        /// <param name="Substring">The substring originating this token</param>
        /// <param name="Operator">The token operator or null if this isn't an operator</param>
        public OperatorToken(Guid Symbol, Substring Substring, Operator Operator) : base(Symbol, Substring)
        {
            this.Operator = Operator;
        }


        public StackTokenType GetStackType()
        {
            var Token = Symbol;
            if (Token == SpecialTokens.LeftParenthesis)
                return StackTokenType.LeftParenthesis;
            else if (Token == SpecialTokens.RightParenthesis)
                return StackTokenType.RightParenthesis;
            else if (Token == SpecialTokens.Comma)
                return StackTokenType.Comma;
            else if (Operator != null)
                return StackTokenType.Operand;
            else
                return StackTokenType.Value;
        }

        /// <summary>
        /// Contains the solved operator if this token was identified as an operator, else contains null
        /// </summary>
        public Operator Operator { get; private set; }

        public override string ToString()
        {
            if (Operator != null)
                return Operator.ToString();
            else
                return Substring.ToString();
        }

        public bool Equals(OperatorToken other)
        {
            return object.Equals(other.Operator?.Id, this.Operator?.Id) && other.Substring.Equals(this.Substring) && object.Equals(other.Symbol, this.Symbol);
        }
    }
}
