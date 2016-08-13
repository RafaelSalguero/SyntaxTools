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
    /// Match an ordered sequence of expressions
    /// </summary>
    public class Sequence : BackusExpression
    {
        public Sequence(IEnumerable<BackusExpression> Expressions)
        {
            this.Expressions = Expressions;
        }
        public Sequence(params BackusExpression[] Expressions) : this((IEnumerable<BackusExpression>)Expressions)
        {
        }

        public IEnumerable<BackusExpression> Expressions { get; private set; }

        public override BackusNaurResult Parse(StateDequeue<TokenSubstring> Symbols)
        {
            //Save the symbol queue state
            Symbols.PushState();

            var result = new List<BackusNaurResult>();
            string Before = "";
            try
            {
                //Parse each expression sequentially
                foreach (var Ex in Expressions)
                {
                    result.Add(Ex.Parse(Symbols));
                    Before += Ex.ToString();
                }
            }
            catch (BackusNaurException ex)
            {
                //If any expression fails
                //Return the symbol queue to its original state
                Symbols.PopState();
                throw new SequenceException(ToString(), Before, Symbols.Peek().Substring, ex);
            }

            //No parser failed, all the sequence was succesfully parsed
            //Preserve state
            Symbols.DropState();
            return new Result(result, this.ExpressionId);
        }

        protected override string InternalToString()
        {
            return Expressions.Select(x => x.ToString()).Aggregate("", (a, b) => a == "" ? b : a + " " + b);
        }

        /// <summary>
        /// Result of a sequence or repeated expression
        /// </summary>
        public class Result : BackusNaurResult
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="Items">Collection result items. Ignore items will be filtered out</param>
            public Result(IReadOnlyList<BackusNaurResult> Items, Guid ExpressionId)
                : base(ExpressionId, false)
            {
                this.Items = Items.Where((x) => !x.Ignore).ToList();
            }

            public IReadOnlyList<BackusNaurResult> Items
            {
                get;
                private set;
            }

            public override string ToString()
            {
                return "{" + Items.Select(x => x.ToString()).Aggregate("", (a, b) => a == "" ? b : a + " " + b) + "}";
            }
        }
    }
}
