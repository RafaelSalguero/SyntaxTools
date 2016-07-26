using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;

namespace SyntaxTools.Text
{
    /// <summary>
    /// A single token, output from a tokenizer
    /// </summary>
    public class TokenSubstring  : IEquatable<TokenSubstring>
    {
        public TokenSubstring(Guid Symbol, Substring Substring)
        {
            this.Symbol = Symbol;
            this.Substring = Substring;
        }


        /// <summary>
        /// The identified lexical symbol
        /// </summary>
        public Guid Symbol { get; private set; }

        /// <summary>
        /// The string that originated this token
        /// </summary>
        public Substring Substring { get; private set; }

        public bool Equals(TokenSubstring other)
        {
            return other.Symbol == this.Symbol && other.Substring.Equals(this.Substring);
        }
    }
}
