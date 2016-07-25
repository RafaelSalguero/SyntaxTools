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
using SyntaxTools.Tree;
using SyntaxTools.Tree.Patterns;
using SyntaxTools.Tree.Patterns.Sequence;
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

            Assert.IsTrue(R.Select(x => x.Token).SequenceEqual(Expected));
        }

        [TestMethod]
        public void PatternTest()
        {
            //pattern for x + x
            var Pattern = new Tree.Patterns.Leaf<string>("+", new Exact<string>(new Variable<string>("x"), new Variable<string>("x")));

            //expression 5 + 5
            var Expression = new Tree<string>("+", new Tree<string>("5"), new Tree<string>("5"));

            var Match = Pattern.Match(Expression);

            Assert.AreEqual(1, Match.Count);
            Assert.AreEqual(1, Match[0].Values.Count);
            Assert.AreEqual("5", Match[0].Values["x"].Value);
        }
    }
}
