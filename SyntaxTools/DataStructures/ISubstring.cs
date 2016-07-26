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
    public struct Substring : IEquatable<Substring>
    {
        /// <summary>
        /// Create a new subtring
        /// </summary>
        public Substring(string CompleteString, int CharIndex, int CharLen)
        {
            this.completeString = CompleteString;
            this.charIndex = CharIndex;
            this.charLen = CharLen;
        }

        /// <summary>
        /// Create a new subtring
        /// </summary>
        public Substring(string CompleteString) : this(CompleteString, 0, CompleteString.Length)
        {
        }


        private readonly string completeString;
        /// <summary>
        /// Original string
        /// </summary>
        public string CompleteString
        {
            get { return completeString; }
        }

        private readonly int charIndex;
        /// <summary>
        /// Substring index
        /// </summary>
        public int Index
        {
            get { return charIndex; }
        }
        private readonly int charLen;
        /// <summary>
        /// Substring length
        /// </summary>
        public int Length
        {
            get { return charLen; }
        }

        /// <summary>
        /// Substring equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Substring other)
        {
            if (object.ReferenceEquals(this.CompleteString, other.CompleteString))
            {
                return
             this.Index == other.Index &&
             this.Length == other.Length;
            }
            else
                throw new ArgumentException("Can't compare substring from diferent original strings");
        }


        public override string ToString()
        {
            return this.completeString.Substring(this.Index, this.Length);
        }

        /// <summary>
        /// Concatenate 2 adjacent substrings
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Substring Concat(Substring a, Substring b)
        {
            if (!Object.ReferenceEquals(a.CompleteString, b.CompleteString))
                throw new ArgumentException("ParserPositions are from diferent codelines");
            if (b.Index != a.Index + a.Length)
                throw new ArgumentException("ParserPositions arent contigous");
            return new Substring(a.CompleteString, a.Index, a.Length + b.Length);
        }

        /// <summary>
        /// Return a substring with all the content of the original string until the begining of this substring
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public Substring BeforeSelf(Substring S)
        {
            return new Substring(S.CompleteString, 0, S.Index + 1 - S.Length);
        }

        /// <summary>
        /// Return a substring with all the content of the original substring after this substring
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public Substring AfterSelf(Substring S)
        {
            return new Substring(S.CompleteString, S.Index + S.Length, S.CompleteString.Length - S.Index - S.Length);
        }
    }


}
