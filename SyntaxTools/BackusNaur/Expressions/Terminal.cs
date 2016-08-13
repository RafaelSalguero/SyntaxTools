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
    /// Matches a given token
    /// </summary>
    public class Terminal : BackusExpression
    {
        /// <summary>
        /// Matches a given token. The result is marked as ignore
        /// </summary>
        /// <param name="token">The token to match</param>
        public Terminal(TokenSubstring token) : this(x => x.Equals(token), token.ToString(), true)
        { }

        /// <summary>
        /// Matches a token with the given symbol
        /// </summary>
        /// <param name="Symbol">The token symbol to match</param>
        /// <param name="Ignore">True to ignore the result on sequence expressions</param>
        public Terminal(Guid Symbol, bool Ignore) : this(x => x.Symbol == Symbol, Global.GuidNames.GetName(Symbol), Ignore)
        { }

        /// <summary>
        /// Match a token that passes the given predicate
        /// </summary>
        /// <param name="Predicate">The token predicate</param>
        /// <param name="description">The predicate friendly description</param>
        /// <param name="Ignore">True to ignore the result on sequence expression</param>
        public Terminal(Func<TokenSubstring, bool> Predicate, string description, bool Ignore)
        {
            this.tokenPredicate = Predicate;
            this.description = description;
            this.Ignore = Ignore;
        }

        readonly string description;
        readonly Func<TokenSubstring, bool> tokenPredicate;
        readonly bool Ignore;
        protected override string InternalToString()
        {
            return description;
        }

        public override BackusNaurResult Parse(StateDequeue<TokenSubstring> Symbols)
        {
            if (Symbols.IsEmpty)
                throw new UnexpectedEndingException(ToString(), Symbols);
            else
            {
                var Peek = Symbols.Peek();
                if (tokenPredicate(Peek))
                {
                    //Consume the symbol and return the result
                    Symbols.Dequeue();
                    return new Result(this.ExpressionId, Ignore, Peek);
                }
                else
                {
                    throw new UnexpectedSymbolException(ToString(), Peek.Substring);
                }
            }
        }

        /// <summary>
        /// The result of parsing a terminal expression. Contains the matched token
        /// </summary>
        public class Result : BackusNaurResult
        {
            public Result(Guid ExpressionId, bool Ignore, TokenSubstring Token) : base(ExpressionId, Ignore)
            {
                this.Token = Token;
            }

            public TokenSubstring Token { get; private set; }

            public override string ToString()
            {
                return Token.ToString();
            }
        }
    }
}
