using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTools.BackusNaur.Expressions;
using SyntaxTools.DataStructures;
using SyntaxTools.Global;
using SyntaxTools.Text;

namespace SyntaxTools.Test
{
    [TestClass]
    public class BackusNaurTest
    {
        [TestMethod]
        public void TerminalTest()
        {
            var T = new Terminal(SpecialTokens.Number, false);
            var Tokenizer = new Tokenizer();
            var Tokens = Tokenizer.Tokenize(new Substring("1 + 2"));

            var SymbolQueue = new StateDequeue<TokenSubstring>(Tokens);
            var Result = T.Parse(SymbolQueue);

            Assert.IsInstanceOfType(Result, typeof(Terminal.Result));
            Assert.AreEqual(1, SymbolQueue.AbsoluteReadPointer);
        }


        [TestMethod]
        public void SequenceTest()
        {
            var Tokenizer = new Tokenizer();
            var Tokens = Tokenizer.Tokenize(new Substring("1 + 2"));
            var SymbolQueue = new StateDequeue<TokenSubstring>(Tokens);

            var T =
                new Sequence(
                    new Terminal(SpecialTokens.Number, false),
                    new Terminal(CommonTokens.Aritmetic.Add, true),
                    new Terminal(SpecialTokens.Number, false)
                    );

            var Result = (Sequence.Result)T.Parse(SymbolQueue);

            Assert.AreEqual(2, Result.Items.Count);
            Assert.AreEqual(Tokens[0], ((Terminal.Result)Result.Items[0]).Token);
            Assert.AreEqual(Tokens[2], ((Terminal.Result)Result.Items[1]).Token);
        }

        [TestMethod]
        public void SequenceExceptionTest()
        {
            var Tokenizer = new Tokenizer();
            var Tokens = Tokenizer.Tokenize(new Substring("1 + 2"));
            var SymbolQueue = new StateDequeue<TokenSubstring>(Tokens);

            var T =
             new Sequence(
                 new Terminal(SpecialTokens.Number, false),
                 new Terminal(CommonTokens.Aritmetic.Minus, true),
                 new Terminal(SpecialTokens.Number, false)
                 );

            bool correct = false;
            try
            {
                var Result = (Sequence.Result)T.Parse(SymbolQueue);
            }
            catch (BackusNaur.SequenceException ex)
            {
                Assert.IsInstanceOfType(ex.InnerException, typeof(BackusNaur.UnexpectedSymbolException));
                correct = true;
            }

            if (!correct)
                Assert.Fail();
        }

        [TestMethod]
        public void SimpleCasesTest()
        {
            var Tokenizer = new Tokenizer();
            var Tokens = Tokenizer.Tokenize(new Substring("+"));
            var SymbolQueue = new StateDequeue<TokenSubstring>(Tokens);

            var T = new Cases(
                new Terminal(CommonTokens.Aritmetic.Minus, false), 
                new Terminal(CommonTokens.Aritmetic.Add, false)
                );

            var Result = T.Parse(SymbolQueue);
        }
    }
}
