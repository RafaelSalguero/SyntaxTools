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
    public interface ITokenizedParser : INoLookupLexerUnitParser
    {
        Guid Token { get; }
    }

    public static class NoLookupLexerUnitExtensions
    {
        /// <summary>
        /// Return the given parser paired with a guid representing a symbol
        /// </summary>
        /// <param name="Symbol">The symbol to pair</param>
        public static ITokenizedParser SetSymbol(this INoLookupLexerUnitParser Parser, Guid Symbol)
        {
            return new SingleTokenParser(Symbol, Parser);
        }

        /// <summary>
        /// Return the given parser paired with a random guid representing a symbol
        /// </summary>
        /// <param name="Symbol">The symbol to pair</param>
        public static ITokenizedParser SetSymbol(this INoLookupLexerUnitParser Parser)
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
            public SingleTokenParser(Guid Token, INoLookupLexerUnitParser Parser)
            {
                GuidNames.AddToken(Token, Parser.ToString());
                this.Token = Token;
                this.Parser = Parser;
            }

            private INoLookupLexerUnitParser Parser;

            /// <summary>
            /// Lexer unit token
            /// </summary>
            public Guid Token
            {
                get;
                private set;
            }

            void INoLookupLexerUnitParser.Append(char Current)
            {
                Parser.Append(Current);
            }

            void INoLookupLexerUnitParser.Reset()
            {
                Parser.Reset();
            }

            LexerUnitValidity INoLookupLexerUnitParser.CurrentValidity
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
