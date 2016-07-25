using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Global;

namespace SyntaxTools.Text.LexerUnits
{
    /// <summary>
    /// Pairs a lexer unit with a guid
    /// </summary>
    public interface ITokenizedParser : IStateMachineParser
    {
        Guid Token { get; }
    }

    public static class NoLookupLexerUnitExtensions
    {
        /// <summary>
        /// Return the given parser paired with a guid representing a symbol
        /// </summary>
        /// <param name="Symbol">The symbol to pair</param>
        public static ITokenizedParser SetSymbol(this IStateMachineParser Parser, Guid Symbol)
        {
            return new SingleTokenParser(Symbol, Parser);
        }

        /// <summary>
        /// Return the given parser paired with a random guid representing a symbol
        /// </summary>
        /// <param name="Symbol">The symbol to pair</param>
        public static ITokenizedParser SetSymbol(this IStateMachineParser Parser)
        {
            return Parser.SetSymbol(Guid.NewGuid());
        }

        /// <summary>
        /// Pairs a lexer unit with a guid
        /// </summary>
        private class SingleTokenParser : ITokenizedParser
        {
            /// <summary>
            /// Create an instance from a given guid and a parser
            /// </summary>
            /// <param name="Token">The guid that will identify the symbol</param>
            /// <param name="Parser">State machine that will parse the symbol</param>
            public SingleTokenParser(Guid Token, IStateMachineParser Parser)
            {
                GuidNames.SetName(Token, Parser.ToString());
                this.Token = Token;
                this.Parser = Parser;
            }

            private IStateMachineParser Parser;

            /// <summary>
            /// Lexer unit token
            /// </summary>
            public Guid Token
            {
                get;
                private set;
            }

            void IStateMachineParser.Append(char Current)
            {
                Parser.Append(Current);
            }

            void IStateMachineParser.Reset()
            {
                Parser.Reset();
            }

            ParserState IStateMachineParser.CurrentValidity
            {
                get
                {
                    return Parser.CurrentValidity;
                }
            }

            public override string ToString()
            {
                return Parser.ToString() + " " + Parser.CurrentValidity.ToString();
            }
        }
    }

}
