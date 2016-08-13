using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;
using SyntaxTools.Text;

namespace SyntaxTools.BackusNaur.Expressions
{
    /// <summary>
    /// Match one of a set of given expressions
    /// </summary>
    public class Cases : BackusExpression
    {
        public Cases(IEnumerable<BackusExpression> Expressions)
        {
            this.Expressions = Expressions;
        }
        public Cases(params BackusExpression[] Expressions)
            : this((IEnumerable<BackusExpression>)Expressions)
        { }

        public IEnumerable<BackusExpression> Expressions
        {
            get;
            private set;
        }


        public override BackusNaurResult Parse(StateDequeue<TokenSubstring> Symbols)
        {
            //Test each expression, until one expression is succesfully parsed
            var exceptions = new List<BackusNaurException>();
            foreach (var Ex in Expressions)
            {
                try
                {
                    var Ret = Ex.Parse(Symbols);
                    return new Result(Ret, ExpressionId, false);
                }
                catch (BackusNaurException ex)
                {
                    //By contract all parsers should let the symbol queue untouched when failing
                }
            }

            //All parses failed
            throw new CasesException(Expressions.Select(x => x.ToString()).Aggregate("", (a, b) => a == "" ? b : a + ", " + b), Symbols.Peek().Substring, exceptions);
        }

        protected override string InternalToString()
        {
            return $"({Expressions.Select(x => x.ToString()).Aggregate("", (a, b) => a == "" ? b : a + " | " + b)})";
        }

        /// <summary>
        /// A result of a selection of varios posible backus naur expressions
        /// </summary>
        public class Result : BackusNaurResult
        {
            public Result(BackusNaurResult Value, Guid ExpressionId, bool Ignore) : base(ExpressionId, Ignore)
            {
                this.Value = Value;

            }

            public BackusNaurResult Value { get; private set; }
            public override string ToString()
            {
                if (Value == null)
                    return "<[empty]>";
                else
                    return $"<{Value}>";
            }
        }
    }
}
