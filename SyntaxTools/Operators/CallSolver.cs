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
    /// Insert call operator on to a list of tokens
    /// </summary>
    public static class CallSolver
    {
        /// <summary>
        /// Count the number of arguments inside a parenthesis group.
        /// </summary>
        /// <param name="Tokens">The token symbols that contains the parenthesis group. The SpecialToken LeftParenthesis, RightParenthesis and Comma symbols are used to define the parenthesis group</param>
        /// <param name="LeftParenthesisIndex">The index of the left parenthesis that starts the group</param>
        /// <returns></returns>
        public static int CountArguments(IReadOnlyList<TokenSubstring> TokenGuids, int LeftParenthesisIndex)
        {
            if (TokenGuids[LeftParenthesisIndex].Symbol != SpecialTokens.LeftParenthesis)
                throw new ArgumentException("The token at the left parenthesis index is not a left parenthesis");

            var argCount = 0;
            var level = 0;


            for (var i = LeftParenthesisIndex; i < TokenGuids.Count; i++)
            {
                var current = TokenGuids[i].Symbol;

                if (current == SpecialTokens.LeftParenthesis)
                    level++;
                else if (current == SpecialTokens.RightParenthesis)
                    level--;

                //El primer argumento es contado
                //Despues del primer argumento, se cuentan las comas
                var firstArgument = i == LeftParenthesisIndex + 1 && current != SpecialTokens.RightParenthesis;
                if ((current == SpecialTokens.Comma && level == 1) || firstArgument)
                {
                    argCount++;
                }

                if (level == 0)
                    return argCount;
            }

            throw new CompilerException("Argument counting parenthesis mismatch", TokenGuids[LeftParenthesisIndex].Substring);
        }


        /// <summary>
        /// Insert the call between detected function names and function arguments enclosed in parenthesis
        /// </summary>
        /// <param name="Tokens">The tokens to process</param>
        /// <param name="IsOperator">Returns true if a token symbol is an operator</param>
        /// <returns></returns>
        public static IReadOnlyList<OperatorToken> InsertCallOperator(IReadOnlyList<OperatorToken> Tokens)
        {
            var ret = new List<OperatorToken>(Tokens.Count);
            for (var i = 0; i < Tokens.Count; i++)
            {
                var current = Tokens[i];
                ret.Add(current);
                var IsOperator =
                    current.Operator != null ||
                    current.Symbol == SpecialTokens.LeftParenthesis ||
                    current.Symbol == SpecialTokens.Comma;
                if (i < Tokens.Count - 1 && !IsOperator && Tokens[i + 1].Symbol == SpecialTokens.LeftParenthesis)
                {
                    var ArgCount = CountArguments(Tokens, i + 1);

                    //Creates the call operator token:
                    var SS = Tokens[i].Substring;
                    var Op = new Operator(int.MaxValue, OperatorArgumentPosition.Binary, OperatorAssociativity.Left, ArgCount + 1, SpecialTokens.Call);
                    var newToken = new OperatorToken(SpecialTokens.Call, new Substring(SS.CompleteString, SS.Index + 1, 0), Op);
                    ret.Add(newToken);
                }
            }
            return ret;
        }
    }
}
