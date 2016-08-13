using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;
using SyntaxTools.Global;
using SyntaxTools.Operators;
using SyntaxTools.Precedence;
using SyntaxTools.Text;
using SyntaxTools.Text.LexerUnits;
using SyntaxTools.Trees;

namespace SyntaxTools.Test
{
    public class Tokenizer
    {
        /// <summary>
        /// Example of a tokenizer class 
        /// </summary>
        class Keywords : LexerDictionary
        {
            public Keywords()
            {
                AddWhiteSpace();
                AddNumber();
                AddTokens<CommonTokens.Aritmetic>();
                Add("(", SpecialTokens.LeftParenthesis);
                Add(")", SpecialTokens.RightParenthesis);
                Add(",", SpecialTokens.Comma);
            }
        }

        Keywords words = new Tokenizer.Keywords();

        public IReadOnlyList<TokenSubstring> Tokenize(Substring Text, bool RemoveWhitespaces = true)
        {
            var R = LexerSplitter.Split(Text, words);

            //Tokenizer extra logic
            if (RemoveWhitespaces)
                return R.Where(x => x.Symbol != SpecialTokens.WhiteSpace).ToList();
            else
                return R;
        }
    }

    public class ExpressionParser
    {
        private  IReadOnlyList< Operator > Operators;
        private Tokenizer Tokenizer = new Tokenizer();
        public ExpressionParser()
        {
            //Get operators from common operators:
            this.Operators = typeof(CommonOperators).GetFields().Select(x => x.GetValue(null)).Cast<Operator>().ToList();
        }
       


        /// <summary>
        /// Parse an expression and return the Operator solved and RPN representation of the input
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public ExpressionTree Parse(Substring Text)
        {
            var Tokens = Tokenizer.Tokenize(Text);
            var OpSolved = OperatorSolver.Solve(Tokens, Operators);
            var CallSolved = CallSolver.InsertCallOperator(OpSolved);
            var RPN = Precedence.RPN.InfixToRPN(CallSolved);

            return Precedence.RPN.RPNToTree(RPN);
        }
    }

}
