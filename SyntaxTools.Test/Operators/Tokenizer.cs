using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxTools.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Text;
using SyntaxTools.Test;
using SyntaxTools.DataStructures;
using SyntaxTools.Global;

namespace SyntaxTools.Operators.Tests
{
    [TestClass()]
    public class TokenizerTest
    {
        [TestMethod()]
        public void CountArgumentsTest()
        {
            var Expr = "Func(1, 2, (1 + 2), 4 , 5)";
            var Tok = new Tokenizer();
            var Tokens = Tok.Tokenize(new Substring(Expr));

            Assert.IsTrue(Tokens.Select(x => x.Symbol).SequenceEqual(new Guid[]
            {
                Guid.Empty, //Func
                SpecialTokens.LeftParenthesis,
                SpecialTokens.Number, //1
                SpecialTokens.Comma,
                SpecialTokens.Number,  //2
                SpecialTokens.Comma,
                SpecialTokens.LeftParenthesis,
                SpecialTokens.Number, //1
                CommonTokens.Aritmetic.Add,
                SpecialTokens.Number, //2
                SpecialTokens.RightParenthesis,
                SpecialTokens.Comma,
                SpecialTokens.Number , //4
                SpecialTokens.Comma,
                SpecialTokens.Number, //5
                SpecialTokens.RightParenthesis
            }));

            Assert.AreEqual(5, CallSolver.CountArguments(Tokens, 1));

            Tokens = Tok.Tokenize(new Substring("Func(1)"));
            Assert.AreEqual(1, CallSolver.CountArguments(Tokens, 1));

            Tokens = Tok.Tokenize(new Substring("Func()"));
            Assert.AreEqual(0, CallSolver.CountArguments(Tokens, 1));
        }

        [TestMethod]
        public void InsertCallOperatorTest()
        {
            var Expr = "Func(1, 2)";
            var Tok = new Tokenizer();
            var Tokens = Tok.Tokenize(new Substring(Expr));
            var Solved = OperatorSolver.Solve(Tokens, new Operator[0]);

            var CallSolved = CallSolver.InsertCallOperator(Solved);

            Assert.IsTrue(CallSolved.Select(x => x.Symbol).SequenceEqual(new Guid[]
            {
                Guid.Empty, //Func
                SpecialTokens.Call,
                SpecialTokens.LeftParenthesis,
                SpecialTokens.Number,
                SpecialTokens.Comma,
                SpecialTokens.Number,
                SpecialTokens.RightParenthesis
            }));

            //Function name + function arguments = 3
            Assert.AreEqual(3, CallSolved[1].Operator.ArgumentCount);
        }

        [TestMethod]
        public void RPNTest()
        {
            var Expr = "Func(1, (1+2*3))";
            var Tok = new Tokenizer();
            var Tokens = Tok.Tokenize(new Substring(Expr));

            var Operators = typeof(CommonOperators).GetFields( ).Select(x => x.GetValue(null)).Cast<Operator>().ToList();

            var Solved = OperatorSolver.Solve(Tokens, Operators);

            var CallSolved = CallSolver.InsertCallOperator(Solved);

            var RPN = Precedence.RPN.InfixToRPN(CallSolved);
            Assert.IsTrue(RPN.Select(x => x.Symbol).SequenceEqual(new Guid[]
            {
               Guid.Empty, //Func,
               SpecialTokens.Number, //1
               SpecialTokens.Number, //1
               SpecialTokens.Number, //2
               SpecialTokens.Number, //3
               CommonOperators.Mul.Id, //*
               CommonOperators.Add.Id, //*
               SpecialTokens.Call, //*
            }));
        }
    }
}