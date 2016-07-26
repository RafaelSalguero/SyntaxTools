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
    /// <summary>
    /// Provides methods for splitting strings with state machine parsers
    /// </summary>
    public static class LexerSplitter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static LexerWordLenght FindLargestValidSequence(string Text, int index, int count, IEnumerable<ITokenizedParser> Parsers)
        {
            int len = 0;
            int maxValidLen = 0;
            List<ITokenizedParser> MaxValidUnits = new List<ITokenizedParser>();

            bool anyPossible = true;
            //loop when there are any parsers with Possible or ValidPossible states
            while (anyPossible && len < count)
            {
                len++;
                anyPossible = false;
                foreach (var U in Parsers)
                {
                    if (U.IsPossible() || U.IsValid())
                    {
                        //Append characters to all Valid/Possible/ValidPossible parsers
                        U.Append(Text[index]);

                        if (U.IsPossible())
                            anyPossible = true;
                    }
                    if (U.IsValid())
                    {
                        //If this is the largest valid parser, remove all others
                        if (len > maxValidLen)
                        {
                            MaxValidUnits.Clear();
                            maxValidLen = len;
                        }

                        //Add this parser as the valid one
                        MaxValidUnits.Add(U);
                    }
                }

                index++;
            }
            if (MaxValidUnits.Count > 1)
                //There are more than one valid parsers
                throw new ApplicationException("Can't diferentiate between two valid sequences");
            else if (MaxValidUnits.Count == 1)
                //There was only one largest valid parser
                return new LexerWordLenght(maxValidLen, MaxValidUnits[0].Token);
            else
                //There wasn't any valid parser, return lenght of 1 to continue to the next character
                return new LexerWordLenght(1, Guid.Empty);
        }


        /// <summary>
        /// Split a substring by finding the largest symbol identified by the given parsers
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static List<TokenSubstring> Split(Substring Text, IEnumerable<ITokenizedParser> Parsers)
        {
            var str = Text.CompleteString;
            var result = new List<TokenSubstring>();
            //Text substring indexes:
            int index = Text.Index;
            int lastIndex = Text.Index + Text.Length;

            //Length of the current unidentified word
            int UnidentifiedWordLen = 0;
            int i;
            for (i = index; i < lastIndex;)
            {
                //Reset all units
                foreach (var U in Parsers) U.Reset();

                //Find the next largest leaf:
                var AdvanceWord = FindLargestValidSequence(Text.CompleteString, i, lastIndex - i, Parsers);

                if (AdvanceWord.Token == Guid.Empty)
                    UnidentifiedWordLen++;
                else
                {
                    //Add the last unidentified word if any
                    if (UnidentifiedWordLen > 0)
                    {
                        result.Add(new TokenSubstring(Guid.Empty, new Substring(str, i - UnidentifiedWordLen, UnidentifiedWordLen)));
                        UnidentifiedWordLen = 0;
                    }
                    //Add the parsed symbol
                    result.Add(new TokenSubstring(AdvanceWord.Token, new Substring(str, i, AdvanceWord.Lenght)));
                }
                //Advance the char index by the length returned by the FindLargestValidSequence
                i += AdvanceWord.Lenght;
            }

            //Add the last unidentified word if any
            if (UnidentifiedWordLen > 0)
                result.Add(new TokenSubstring(Guid.Empty, new Substring(str, i - UnidentifiedWordLen, UnidentifiedWordLen)));
            return result;
        }


    }
}
