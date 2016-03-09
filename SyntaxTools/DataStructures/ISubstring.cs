using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.DataStructures
{
    /// <summary>
    /// Efficienly represents a substring with a reference to the original string plus starting index and count. Implementers should override ToString to return the substring
    /// </summary>
    public interface ISubstring : IEquatable<ISubstring>
    {
        string CompleteString { get; }
        /// <summary>
        /// Start index of the substring
        /// </summary>
        int Index { get; }
        /// <summary>
        /// Lenght of the substring
        /// </summary>
        int Length { get; }
    }

    /// <summary>
    /// A class that has been cathegorized with a given token type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IToken<T>
    {
        T Token { get; }
    }

    /// <summary>
    /// A substring that contains a value that identifies the string as a symbol
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITokenSubstring<T> : ISubstring, IToken<T>, IEquatable<ITokenSubstring<T>>
    {
    }

    public static class Substring
    {
        private class SubstringImplementation : ISubstring
        {
            public SubstringImplementation(ISubstring Other) : this(Other.CompleteString, Other.Index, Other.Length) { }
            public SubstringImplementation(string CompleteString, int CharIndex, int CharLen)
            {
                this.completeString = CompleteString;
                this.charIndex = CharIndex;
                this.charLen = CharLen;
            }

            private readonly string completeString;
            public string CompleteString
            {
                get { return completeString; }
            }

            private readonly int charIndex;
            public int Index
            {
                get { return charIndex; }
            }
            private readonly int charLen;
            public int Length
            {
                get { return charLen; }
            }

            public bool Equals(ISubstring other)
            {
                return this.EqualsSubstring(other);
            }

            public override string ToString()
            {
                return this.AsString();
            }
        }
        private class TokenSubstring<T> : SubstringImplementation, ITokenSubstring<T>
        {
            public TokenSubstring(ISubstring other, T Token)
                : base(other)
            {
                this.token = Token;
            }

            private readonly T token;
            public T Token
            {
                get { return token; }
            }

            public override string ToString()
            {
                return "[" + Token.ToString() + "] " + base.ToString();
            }

            public bool Equals(ITokenSubstring<T> other)
            {
                return other.Token.Equals(this.Token) && this.EqualsSubstring(other);
            }
        }

        /// <summary>
        /// Concatenate 2 adjacent substrings
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ISubstring Concat(this ISubstring a, ISubstring b)
        {
            if (!Object.ReferenceEquals(a.CompleteString, b.CompleteString))
                throw new ArgumentException("ParserPositions are from diferent codelines");
            if (b.Index != a.Index + a.Length)
                throw new ArgumentException("ParserPositions arent contigous");
            return new SubstringImplementation(a.CompleteString, a.Index, a.Length + b.Length);
        }

        /// <summary>
        /// Create a ITokenSubstring from a given substring
        /// </summary>
        /// <returns></returns>
        public static ITokenSubstring<T> AsToken<T>(this ISubstring S, T Token)
        {
            return new TokenSubstring<T>(S, Token);
        }

        /// <summary>
        /// Return a substring with all the content of the original string until the begining of this substring
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static ISubstring BeforeSelf(this ISubstring S)
        {
            return S.CompleteString.AsSubstring(0, S.Index + 1 - S.Length);
        }

        /// <summary>
        /// Return a substring with all the content of the original substring after this substring
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static ISubstring AfterSelf(this ISubstring S)
        {
            return S.CompleteString.AsSubstring(S.Index + S.Length, S.CompleteString.Length - S.Index - S.Length);
        }

        /// <summary>
        /// Create a substring 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="Index"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static ISubstring AsSubstring(this string s, int Index, int Length)
        {
            return new SubstringImplementation(s, Index, Length);
        }
        public static ISubstring AsSubstring(this string s)
        {
            return s.AsSubstring(0, s.Length);
        }

        /// <summary>
        /// Returns true if the string representation of two substrings are equal, even when they represent different parts of the same original string
        /// </summary>
        /// <param name="S"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool EqualsSubstring(this ISubstring S, ISubstring other)
        {
            if (object.ReferenceEquals(S.CompleteString, other.CompleteString) &&
                S.Index == other.Index &&
                S.Length == other.Length)
                return true;
            else
                return S.AsString() == other.AsString();
        }

        /// <summary>
        /// Convert the substring to the represented string
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static string AsString(this ISubstring S)
        {
            return S.CompleteString.Substring(S.Index, S.Length);
        }
    }

}
