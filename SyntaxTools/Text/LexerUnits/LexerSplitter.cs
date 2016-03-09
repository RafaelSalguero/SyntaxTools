using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;
using SyntaxTools.Global;

namespace SyntaxTools.Text.LexerUnits
{
    /// <summary>
    /// A tokenized word lenght
    /// </summary>
    public struct LexerWordLenght
    {
        /// <summary>
        /// Create a new valid token lexer word
        /// </summary>
        /// <param name="Lenght"></param>
        /// <param name="Token"></param>
        public LexerWordLenght(int Lenght, Guid Token)
        {
            this.Lenght = Lenght;
            this.Token = Token;
        }

        /// <summary>
        /// Create a new non-token word
        /// </summary>
        /// <param name="Lenght"></param>
        public LexerWordLenght(int Lenght)
            : this(Lenght, Guid.Empty)
        {

        }

        /// <summary>
        /// Gets the lenght of the lexer word
        /// </summary>
        public readonly int Lenght;

        /// <summary>
        /// Gets the ID of the lexer unit that generated this lexer word
        /// </summary>
        public readonly Guid Token;

        /// <summary>
        /// Returns weather this lexer word is a valid token
        /// </summary>
        public bool IsToken
        {
            get
            {
                return Token != Guid.Empty;
            }
        }

        public override string ToString()
        {
            if (IsToken)
                return Lenght.ToString() + " [" + GuidNames.GetName(Token) + "]";
            else
                return Lenght.ToString();
        }
    }
    public class SubstringLexerSeparator
    {
        public SubstringLexerSeparator(IEnumerable<ITokenizedParser> Units)
        {
            this.Units = new List<ITokenizedParser>(Units);
        }
        public List<ITokenizedParser> Units;

        public LexerWordLenght FindValidLeaf(string Text, int index, int count)
        {
            int len = 0;
            int maxValidLen = 0;
            List<ITokenizedParser> MaxValidUnits = new List<ITokenizedParser>();

            bool anyPosible = true;
            while (anyPosible && len < count)
            {
                len++;
                anyPosible = false;
                foreach (var U in Units)
                {
                    if (U.IsPosible() || U.IsValid())
                    {
                        U.Append(Text[index]);

                        if (U.IsPosible())
                            anyPosible = true;
                    }
                    if (U.IsValid())
                    {
                        if (len > maxValidLen)
                        {
                            MaxValidUnits.Clear();
                            maxValidLen = len;
                        }
                        MaxValidUnits.Add(U);
                    }
                }

                index++;
            }
            if (MaxValidUnits.Count > 1)
                throw new ApplicationException("Can't diferentiate between valid units");
            else if (MaxValidUnits.Count == 1)
                return new LexerWordLenght(maxValidLen, MaxValidUnits[0].Token);
            else
                return new LexerWordLenght(1, Guid.Empty);
        }

        public List<ITokenSubstring<Guid>> Split(string Text)
        {
            return Split(Text.AsSubstring());
        }
        public List<ITokenSubstring<Guid>> Split(ISubstring Text)
        {
            var str = Text.CompleteString;
            var Ret = new List<ITokenSubstring<Guid>>();

            int index = Text.Index;
            int lastIndex = Text.Index + Text.Length;

            int UnidentifiedWordLen = 0;
            int i;
            for (i = index; i < lastIndex;)
            {
                //Reset all units
                foreach (var U in Units) U.Reset();

                //Find the next largest leaf:
                var AdvanceWord = FindValidLeaf(Text.CompleteString, i, lastIndex - i);

                if (AdvanceWord.Token == Guid.Empty)
                    UnidentifiedWordLen++;
                else
                {
                    if (UnidentifiedWordLen > 0)
                    {
                        Ret.Add(str.AsSubstring(i - UnidentifiedWordLen, UnidentifiedWordLen).AsToken(Guid.Empty));
                        UnidentifiedWordLen = 0;
                    }

                    Ret.Add(str.AsSubstring(i, AdvanceWord.Lenght).AsToken(AdvanceWord.Token));
                }

                i += AdvanceWord.Lenght;
            }

            if (UnidentifiedWordLen > 0)
            {
                Ret.Add(str.AsSubstring(i - UnidentifiedWordLen, UnidentifiedWordLen).AsToken(Guid.Empty));
            }
            return Ret;
        }


    }
}
