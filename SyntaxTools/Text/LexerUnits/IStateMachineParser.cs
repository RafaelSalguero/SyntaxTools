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
    public interface IStateMachineParser
    {
        /// <summary>
        /// Append this character to the parser
        /// </summary>
        /// <param name="C"></param>
        void Append(char Current);

        /// <summary>
        /// Reset the parser to its original state
        /// </summary>
        void Reset();

        ParserState CurrentValidity { get; }
    }

    /// <summary>
    /// Determines the state of a lexer unit acording to past character inputs
    /// </summary>
    public enum ParserState
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
        ValidPossible,

        /// <summary>
        /// This word as is doesn't comply with the validation but the result of this word and concatenation of one or more characters could return in a valid word
        /// </summary>
        Possible
    }

    public static class LexerUnitExtensiosns
    {
        /// <summary>
        /// Returns true if the current validity is Posible or ValidPosible
        /// </summary>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static bool IsPossible(this IStateMachineParser Unit)
        {
            return Unit.CurrentValidity == ParserState.Possible || Unit.CurrentValidity == ParserState.ValidPossible;
        }

        /// <summary>
        /// Returns true if the current validity is Valid or ValidPosible
        /// </summary>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static bool IsValid(this IStateMachineParser Unit)
        {
            return Unit.CurrentValidity == ParserState.Valid || Unit.CurrentValidity == ParserState.ValidPossible;
        }


        /// <summary>
        /// Returns true if the current validity is Invalid
        /// </summary>
        /// <param name="Unit"></param>
        /// <returns></returns>
        public static bool IsInvalid(this IStateMachineParser Unit)
        {
            return Unit.CurrentValidity == ParserState.Invalid;
        }
    }
}
