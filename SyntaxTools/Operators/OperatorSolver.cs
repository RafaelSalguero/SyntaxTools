using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;
using SyntaxTools.Precedence;
using SyntaxTools.Text;

namespace SyntaxTools.Operators
{
    public static class OperatorSolver
    {

        static bool IsRightArgument(int i,
                   Solving[] Tokens)
        {
            if (i >= Tokens.Length)
                return false;

            switch (Tokens[i])
            {
                case Solving.Value:
                case Solving.OpenParenthesis:
                case Solving.Prefix:
                    return true;
                case Solving.Binary:
                case Solving.Postfix:
                case Solving.PostfixOrBinary:
                case Solving.ClosedParenthesis:
                case Solving.Comma:
                    return false;

                case Solving.PrefixOrBinary:
                    return IsRightArgument(i + 1, Tokens);
                default:
                    throw new ApplicationException();
            }
        }

        static bool IsLeftArgument(int i,
                   Solving[] Tokens)
        {
            if (i < 0)
                return false;

            switch (Tokens[i])
            {
                case Solving.Value:
                case Solving.ClosedParenthesis:
                case Solving.Postfix:
                    return true;
                case Solving.Binary:
                case Solving.Prefix:
                case Solving.PrefixOrBinary:
                case Solving.OpenParenthesis:
                case Solving.Comma:
                    return false;

                case Solving.PostfixOrBinary:
                    return IsLeftArgument(i - 1, Tokens);
                default:
                    throw new ApplicationException();
            }
        }

        enum Solving
        {
            Value,
            Comma,
            OpenParenthesis,
            ClosedParenthesis,
            Prefix,
            Postfix,
            Binary,
            PrefixOrBinary,
            PostfixOrBinary
        }


        /// <summary>
        /// For each token, identifies each related operator (all the operators with the same symbol) and solve conflicts if more than one operator is related to the same symbol acording to operator.
        /// This function uses the SpecialTokens for identifying special operators
        /// argument position rules
        /// </summary>
        /// <param name="Tokens">Token list to solve</param>
        /// <param name="Operators">The collection of operator definitions</param>
        /// <param name="TokenOperatorSelector"></param>
        /// <returns></returns>
        public static IReadOnlyList<OperatorToken> Solve(
             IReadOnlyList<TokenSubstring> Tokens,
            IEnumerable<Operator> Operators
            )
        {
            //Initialize the operator dictionary:
            var OperatorDic = Operators.GroupBy(x => x.Symbol).ToDictionary(x => x.Key, x => x.ToList());

            //Validate the operator dictionary:
            foreach (var kv in OperatorDic)
            {
                if (kv.Value.Count == 3)
                    throw new ArgumentException("Can't handle triple operator discrimination on '" + Global.GuidNames.GetName(kv.Key) + "'");
                if (kv.Value.Count == 2 && !kv.Value.Any((x) => x.OperatorType == OperatorArgumentPosition.Binary))
                    throw new ArgumentException("Can't handle prefix/postfix operator discrimination on '" + Global.GuidNames.GetName(kv.Key) + "'");
            }

            //****************************************************************************
            //Presolve all operators onto an array of matches;
            Solving[] Solving = new Solving[Tokens.Count];
            var MatchArray = new List<Operator>[Tokens.Count];
            for (int i = 0; i < Tokens.Count; i++)
            {
                List<Operator> Match;
                if (OperatorDic.TryGetValue(Tokens[i].Symbol, out Match))
                {
                    MatchArray[i] = Match;
                    if (Match.Count == 1)
                    {
                        switch (Match[0].OperatorType)
                        {
                            case OperatorArgumentPosition.Binary: Solving[i] = OperatorSolver.Solving.Binary; break;
                            case OperatorArgumentPosition.PrefixUnary: Solving[i] = OperatorSolver.Solving.Prefix; break;
                            case OperatorArgumentPosition.PostfixUnary: Solving[i] = OperatorSolver.Solving.Postfix; break;
                        }
                    }
                    else
                    {
                        if (Match.Any((x) => x.OperatorType == OperatorArgumentPosition.PrefixUnary))
                            Solving[i] = OperatorSolver.Solving.PrefixOrBinary;
                        else
                            Solving[i] = OperatorSolver.Solving.PostfixOrBinary;
                    }
                }
                else
                {
                    //Identify parenthesis and commas:
                    if (Tokens[i].Symbol == SpecialTokens.LeftParenthesis)
                        Solving[i] = OperatorSolver.Solving.OpenParenthesis;
                    else if (Tokens[i].Symbol == SpecialTokens.RightParenthesis)
                        Solving[i] = OperatorSolver.Solving.ClosedParenthesis;
                    else if (Tokens[i].Symbol == SpecialTokens.Comma)
                        Solving[i] = OperatorSolver.Solving.Comma;
                    else
                        Solving[i] = OperatorSolver.Solving.Value;

                }
            }

            var ret = new OperatorToken[Tokens.Count];
            //****************************************************************************
            for (int i = 0; i < Tokens.Count; i++)
            {
                if (Solving[i] == OperatorSolver.Solving.Value ||
                    Solving[i] == OperatorSolver.Solving.OpenParenthesis ||
                    Solving[i] == OperatorSolver.Solving.ClosedParenthesis ||
                    Solving[i] == OperatorSolver.Solving.Comma
                    )

                    ret[i] = new OperatorToken(Tokens[i], null);
                else if (MatchArray[i].Count == 1)
                    ret[i] = new OperatorToken(Tokens[i], MatchArray[i][0]);
                else
                {
                    bool LeftArg = IsLeftArgument(i - 1, Solving);
                    bool RightArg = IsRightArgument(i + 1, Solving);

                    var BinaryOp = MatchArray[i].First((x) => x.OperatorType == OperatorArgumentPosition.Binary);
                    var OtherOp = MatchArray[i].First((x) => x.OperatorType != OperatorArgumentPosition.Binary);

                    if (LeftArg && RightArg)
                    {
                        Solving[i] = OperatorSolver.Solving.Binary;
                        ret[i] = new OperatorToken(Tokens[i], BinaryOp);

                    }
                    else
                    {
                        if (OtherOp.OperatorType == OperatorArgumentPosition.PostfixUnary)
                            Solving[i] = OperatorSolver.Solving.Postfix;
                        else
                            Solving[i] = OperatorSolver.Solving.Prefix;
                        ret[i] = new OperatorToken(Tokens[i], OtherOp);
                    }
                }

            }

            return ret;
        }


    }
}
