using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Text.LexerUnits.StateMachines
{
    /// <summary>
    /// A lexer unit that parses a whole word
    /// </summary>
    public class WordParser : IStateMachineParser
    {
        public WordParser(string Word)
        {
            this.Value = Word;
            Reset();
        }

        private readonly string Value;
        /// <summary>
        /// The number of valid characters apended
        /// </summary>
        private int ValidLen = 0;
        public void Append(char Current)
        {
            //If the current character is the next character on the word
            if (ValidLen < Value.Length && Value[ValidLen] == Current)
            {
                ValidLen++;
                //If the whole word was added, set Valid, else possible
                Validity = ValidLen == Value.Length ? ParserState.Valid : ParserState.Possible;
            }
            else
                Validity = ParserState.Invalid;
        }

        public void Reset()
        {
            ValidLen = 0;
            if (Value.Length == 0)
                Validity = ParserState.Valid;
            else
                Validity = ParserState.Possible;
        }



        private ParserState Validity;
        public ParserState CurrentValidity
        {
            get { return Validity; }
        }

        public override string ToString()
        {
            return Value;
        }


    }
}
