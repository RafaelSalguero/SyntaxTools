using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTools.DataStructures;
using SyntaxTools.Global;
using SyntaxTools.Text;
using SyntaxTools.Text.LexerUnits;
using SyntaxTools.Trees;
using SyntaxTools.Trees.Patterns;
using SyntaxTools.Trees.Patterns.Sequence;
namespace SyntaxTools.Test
{
    [TestClass]
    public class Patterns
    {
        [TestMethod]
        public void Splitter()
        {
            var text = "(1 +2)* 8.2";
            var Language = new Tokenizer();
            var R = Language.Tokenize(text.AsSubstring());

            Assert.AreEqual(R.Count, 7);

            var Expected = new[]
            {
                SpecialTokens.LeftParenthesis,
                SpecialTokens.Number,
                CommonTokens.Aritmetic.Add,
                SpecialTokens.Number,
                SpecialTokens.RightParenthesis,
                CommonTokens.Aritmetic.Mul,
                SpecialTokens.Number
            };

            Assert.IsTrue(R.Select(x => x.Symbol).SequenceEqual(Expected));
        }


    }
}
