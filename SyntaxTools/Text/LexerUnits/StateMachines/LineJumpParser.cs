﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Text.LexerUnits.StateMachines
{
    /// <summary>
    /// Identifies all combinations of lines jumps (CR_LF, CR, LF). All line jumps share the same Id
    /// </summary>
    public class LineJumpParser : IStateMachineParser
    {
        public LineJumpParser()
        {
            Reset();
        }
        public enum LineJumpState
        {
            /// <summary>
            /// Expecting a CR or LF
            /// </summary>
            CR_LF,

            /// <summary>
            /// Expecting an LF
            /// </summary>
            LF,

            Invalid

        }

        private LineJumpState State = LineJumpState.CR_LF;

        public void Append(char Current)
        {
            switch (State)
            {
                case LineJumpState.CR_LF:
                    {
                        if (Current == '\r')
                        {
                            State = LineJumpState.LF;
                            CurrentValidity = ParserState.ValidPossible;
                        }
                        else if (Current == '\n')
                        {
                            State = LineJumpState.Invalid;
                            CurrentValidity = ParserState.Valid;
                        }
                        else
                        {
                            State = LineJumpState.Invalid;
                            CurrentValidity = ParserState.Invalid;
                        }
                        break;
                    }
                case LineJumpState.LF:
                    {
                        if (Current == '\n')
                        {
                            State = LineJumpState.Invalid;
                            CurrentValidity = ParserState.Valid;
                        }
                        else
                        {
                            State = LineJumpState.Invalid;
                            CurrentValidity = ParserState.Invalid;
                        }
                        break;
                    }
                default:
                    throw new NotSupportedException("State '" + State.ToString() + "' not supported by the LineJumpLexer unit");
            }
        }

        public void Reset()
        {
            State = LineJumpState.CR_LF;
            CurrentValidity = ParserState.Possible;
        }


        public ParserState CurrentValidity
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return "line jump";
        }
    }
}
