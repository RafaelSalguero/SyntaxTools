﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Text.LexerUnits.StateMachines
{
    /// <summary>
    /// Parse a string literal. All parsed strings share the same id
    /// </summary>
    public class StringParser : IStateMachineParser
    {
        public StringParser()
        {
            Reset();
        }

        public enum StringState
        {
            /// <summary>
            /// Expecting the begin quote or the @ for ignoring all escape sequences
            /// </summary>
            BeginQuoteOrAt,

            /// <summary>
            /// Expecting the begin double quote with disabled escape sequences
            /// </summary>
            BeginQuoteNoEscape,

            /// <summary>
            /// Expecting escape character
            /// </summary>
            Escape,

            /// <summary>
            /// Expecting the ending quote or an escape sequence
            /// </summary>
            EndQuoteOrEscape,

            /// <summary>
            /// Expecting the ending quote
            /// </summary>
            EndQuote,

            Invalid
        }

        public StringState State;


        public void Append(char Current)
        {
            switch (State)
            {
                case StringState.BeginQuoteOrAt:
                    {
                        if (Current == '@')
                        {
                            CurrentValidity = ParserState.Possible;
                            State = StringState.BeginQuoteNoEscape;
                        }
                        else if (Current == '\"')
                        {
                            State = StringState.EndQuoteOrEscape;
                            CurrentValidity = ParserState.ValidPossible;
                        }
                        else
                        {
                            State = StringState.Invalid;
                            CurrentValidity = ParserState.Invalid;
                        }
                        break;
                    }
                case StringState.BeginQuoteNoEscape:
                    {
                        if (Current == '\"')
                        {
                            State = StringState.EndQuote;
                            CurrentValidity = ParserState.ValidPossible;
                        }
                        else
                        {
                            State = StringState.Invalid;
                            CurrentValidity = ParserState.Invalid;
                        }
                        break;
                    }
                case StringState.EndQuoteOrEscape:
                    {
                        if (Current == '\"' || (Current == '\r' || Current == '\n'))
                        {
                            State = StringState.Invalid;
                            CurrentValidity = ParserState.Valid;
                        }
                        else if (Current == '\\')
                        {
                            State = StringState.Escape;
                        }
                        break;
                    }
                case StringState.EndQuote:
                    {
                        if (Current == '\"')
                        {
                            State = StringState.Invalid;
                            CurrentValidity = ParserState.Valid;
                        }
                        break;
                    }
                case StringState.Escape:
                    {
                        State = StringState.EndQuoteOrEscape;
                        break;
                    }
                default:
                    throw new NotImplementedException("State '" + State.ToString() + "' not implemented");
            }
        }

        public void Reset()
        {
            State = StringState.BeginQuoteOrAt;
            CurrentValidity = ParserState.Possible;
        }

        public ParserState CurrentValidity
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return "string";
        }
    }
}
