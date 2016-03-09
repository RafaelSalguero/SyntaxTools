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
    public class WordParser : INoLookupLexerUnitParser
    {
        public WordParser(string Word)
        {
            this.Value = Word;
            Reset();
        }

        private readonly string Value;
        /// <summary>
        /// The last valid lenght
        /// </summary>
        private int ValidLen = 0;

        public void Reset()
        {
            ValidLen = 0;
            if (Value.Length == 0)
                Validity = LexerUnitValidity.Valid;
            else
                Validity = LexerUnitValidity.Posible;
        }

        public void Append(char Current)
        {
            if (ValidLen < Value.Length && Value[ValidLen] == Current)
            {
                ValidLen++;
                Validity = ValidLen < Value.Length ? LexerUnitValidity.Posible : LexerUnitValidity.Valid;
            }
            else
                Validity = LexerUnitValidity.Invalid;
        }

        private LexerUnitValidity Validity;
        public LexerUnitValidity CurrentValidity
        {
            get { return Validity; }
        }

        public override string ToString()
        {
            return Value;
        }

      
    }
}
