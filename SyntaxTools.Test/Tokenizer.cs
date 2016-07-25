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
            }
        }

        Keywords words = new Tokenizer.Keywords();

        public IReadOnlyList<ITokenSubstring<Guid>> Tokenize(ISubstring Text, bool RemoveWhitespaces = true)
        {
            var R = LexerSplitter.Split(Text, words);

            //Tokenizer extra logic
            if (RemoveWhitespaces)
                return R.Where(x => x.Token != SpecialTokens.WhiteSpace).ToList();
            else
                return R;
        }
    }

    public class ExpressionParser
    {
        private List<Operator> Operators;
        private Tokenizer Tokenizer = new Tokenizer();
        public ExpressionParser()
        {
            //Get operators from common operators:
            this.Operators = new List<Operator>(typeof(CommonOperators).GetProperties(System.Reflection.BindingFlags.Static).Select(x => x.GetValue(null)).Cast<Operator>());
        }
        
        /// <summary>
        /// Parse an expression and return the Operator solved and RPN representation of the input
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public IReadOnlyList<ITokenSubstring<Guid>> Parse(ISubstring Text)
        {
            var Tokens = Tokenizer.Tokenize(Text);
            return RPN.InfixToRPN()
        }
    }

}
