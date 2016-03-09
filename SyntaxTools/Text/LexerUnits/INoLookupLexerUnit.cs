using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Text.LexerUnits
{

    /// <summary>
    /// A parser unit with no look up
    /// </summary>
    public interface INoLookupLexerUnitParser
    {
        /// <summary>
        /// Append this character to the parser unit
        /// </summary>
        /// <param name="C"></param>
        void Append(char Current);

        /// <summary>
        /// Reset the lexer unit to its original state
        /// </summary>
        void Reset();

        LexerUnitValidity CurrentValidity { get; }
    }

    /// <summary>
    /// Determines the state of a lexer unit acording to past character inputs
    /// </summary>
    public enum LexerUnitValidity
    {
        /// <summary>
        /// This word or any word that results in the concatenation of this word and other word doesn't comply with this parser unit validation
        /// </summary>
        Invalid,

        /// <summary>
        /// This word as is comply with this parser unit validation, if a character is added, the word would become invalid
        /// </summary>
        Valid,

        /// <summary>
        /// This word is valid as is, if a character is added, the word could be still be a valid unit
        /// </summary>
        ValidPosible,

        /// <summary>
        /// This word as is doesn't comply with the validation but the result of this word and concatenation of one or more characters could return in a valid word
        /// </summary>
        Posible
    }

    public static class LexerUnitExtensiosns
    {
        /// <summary>
        /// Returns true if the current validity is Posible or ValidPosible
        /// </summary>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static bool IsPosible(this INoLookupLexerUnitParser Unit)
        {
            return Unit.CurrentValidity == LexerUnitValidity.Posible || Unit.CurrentValidity == LexerUnitValidity.ValidPosible;
        }

        /// <summary>
        /// Returns true if the current validity is Valid or ValidPosible
        /// </summary>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static bool IsValid(this INoLookupLexerUnitParser Unit)
        {
            return Unit.CurrentValidity == LexerUnitValidity.Valid || Unit.CurrentValidity == LexerUnitValidity.ValidPosible;
        }


        /// <summary>
        /// Returns true if the current validity is Invalid
        /// </summary>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static bool IsInvalid(this INoLookupLexerUnitParser Unit)
        {
            return Unit.CurrentValidity == LexerUnitValidity.Invalid;
        }
    }
}
