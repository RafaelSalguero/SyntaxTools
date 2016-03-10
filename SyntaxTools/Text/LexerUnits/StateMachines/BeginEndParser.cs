using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxTools.Text.LexerUnits.StateMachines
{
    /// <summary>
    /// Parse lexer units that are enclosed between two other lexer units, such as comments
    /// </summary>
    public class BeginEndParser : IStateMachineParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Begin">Block begin parser. Remember not to share parser instances because each instance is an state machine</param>
        /// <param name="End">Block end parser. Remember not to share parser instances because each instance is an state machine</param>
        /// <param name="AllowOpenEnds">If true when the begin is found, this would be considered a valid unit, even if the end is never found</param>
        public BeginEndParser(IStateMachineParser Begin, IStateMachineParser End, bool AllowOpenEnds, string friendlyName = "")
        {
            this.AllowOpenEnds = AllowOpenEnds;
            this.Begin = Begin;
            this.End = End;

            Reset();
        }


        public readonly bool AllowOpenEnds;
        private IStateMachineParser Begin;
        private IStateMachineParser End;

        public enum BeginEndState
        {
            BeginExpected,
            EndOrMiddleExpected,
            EndExpected,
            Invalid
        }

        public BeginEndState State;
        public void Append(char Current)
        {
            switch (State)
            {
                case BeginEndState.BeginExpected:
                    {
                        Begin.Append(Current);
                        if (Begin.CurrentValidity == ParserState.Invalid)
                        {
                            Begin.Reset();
                            Begin.Append(Current);
                        }

                        if (Begin.CurrentValidity == ParserState.Valid || Begin.CurrentValidity == ParserState.ValidPossible)
                        {
                            CurrentValidity = AllowOpenEnds ? ParserState.ValidPossible : ParserState.Possible;
                            State = BeginEndState.EndOrMiddleExpected;
                        }
                        else
                            CurrentValidity = Begin.CurrentValidity;
                        break;
                    }
                case BeginEndState.EndOrMiddleExpected:
                    {
                        End.Append(Current);
                        if (End.CurrentValidity == ParserState.Invalid)
                        {
                            End.Reset();
                            End.Append(Current);
                        }

                        if (End.CurrentValidity == ParserState.Valid)
                        {
                            State = BeginEndState.Invalid;
                            CurrentValidity = ParserState.Valid;
                        }
                        else if (End.CurrentValidity == ParserState.ValidPossible)
                        {
                            CurrentValidity = ParserState.ValidPossible;
                            State = BeginEndState.EndExpected;
                        }
                        else if (End.CurrentValidity == ParserState.Invalid)
                        {
                            End.Reset();
                        }
                        break;
                    }
                case BeginEndState.EndExpected:
                    {
                        End.Append(Current);
                        if (End.CurrentValidity == ParserState.Valid)
                        {
                            State = BeginEndState.Invalid;
                            CurrentValidity = ParserState.Valid;
                        }
                        else if (End.CurrentValidity == ParserState.ValidPossible)
                        {
                            //Do nothing
                        }
                        else
                        {
                            Reset();
                            CurrentValidity = ParserState.Invalid;
                            State = BeginEndState.Invalid;
                        }
                        break;
                    }
                default:
                    throw new NotImplementedException("State '" + State.ToString() + "' not implemented");

            }
        }

        public void Reset()
        {
            this.Begin.Reset();
            this.End.Reset();
            State = BeginEndState.BeginExpected;
            CurrentValidity = ParserState.Possible;
        }

        public ParserState CurrentValidity
        {
            get;
            private set;
        }

        private string friendlyName;
        public override string ToString()
        {
            return friendlyName;
        }
    }
}
