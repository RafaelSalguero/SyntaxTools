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
    public class OperatorToken : TokenSubstring
    {
        public OperatorToken(TokenSubstring Other, Operator Operator) : base(Other.Symbol, Other.Substring)
        {
            this.Operator = Operator;
        }
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
                return Substring.ToString() + " (" + Operator.ToString() + ")";
            else
                return Substring.ToString();
        }
    }
}
