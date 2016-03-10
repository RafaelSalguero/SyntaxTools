using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Text.LexerUnits.StateMachines
{
   /// <summary>
    /// Identify a sequence of spaces and tabs
    /// </summary>
    public class WhitespaceParser : IStateMachineParser
    {
        public enum SpaceLexerState
        {
            /// <summary>
            /// Waiting for a space or tab
            /// </summary>
            SpaceOrTab,

            Invalid
        }

        public WhitespaceParser()
        {
            Reset();
        }

        public SpaceLexerState State;

        public void Append(char Current)
        {
            switch (State)
            {
                case SpaceLexerState.SpaceOrTab:
                    if (Current == ' ')
                    {
                        State = SpaceLexerState.SpaceOrTab;
                        CurrentValidity = ParserState.ValidPossible;
                    } 
                    else if (Current == '\t')
                    {
                        State = SpaceLexerState.SpaceOrTab;
                        CurrentValidity = ParserState.ValidPossible;
                    }
                    else
                    {
                        State = SpaceLexerState.Invalid;
                        CurrentValidity = ParserState.Invalid;
                    }
                    break;
                default:
                    throw new NotSupportedException("State '" + State.ToString() + "' not supported by the SpaceLexer unit");
            }
        }

        public void Reset()
        {
            CurrentValidity = ParserState.Possible;
            State = SpaceLexerState.SpaceOrTab;
        }

        public ParserState CurrentValidity
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return "space";
        }
    }
}
