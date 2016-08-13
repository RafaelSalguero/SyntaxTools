using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;
using SyntaxTools.Text;

namespace SyntaxTools.BackusNaur
{

    public class BackusNaurException : CompilerException
    {
        public BackusNaurException(string message, Substring Position) : base(message, Position) { }
        public BackusNaurException(string message, Substring Position, BackusNaurException InnerException) : base(message, Position, InnerException) { }
    }

    public class UnexpectedEndingException : BackusNaurException
    {
        public UnexpectedEndingException(string Expected, StateDequeue<TokenSubstring> Symbols)
            : base($"The symbol '{Expected}' was expected instead on the end of file", new Substring())
        { }
    }

    public class UnexpectedSymbolException : BackusNaurException
    {
        public UnexpectedSymbolException(string Expected, Substring Position)
            : base($"The symbol '{Expected}' was expected instead of the symbol '{Position}'", Position)
        {
        }
    }

    public class SequenceException : BackusNaurException
    {
        public SequenceException(string Name, string Before, Substring Position, BackusNaurException InnerException)
            : base(
            string.IsNullOrEmpty(Before) ?
            ("At the beginning of the sequence '" + Name + "' " + InnerException.Message) :
            ("In the sequence '" + Name + "' after the tokens '" + Before + "' " + InnerException.Message), Position, InnerException)
        {
        }
    }

    public class CasesException : BackusNaurException
    {
        public CasesException(string Cases, Substring Position, IEnumerable<BackusNaurException> InnerExceptions)
            : base("One of the given expressions was expected: " + Cases + ": " + InnerExceptions.Select((y) => y.Message).ToStringEnum(" or "), Position, new AggregateException(InnerExceptions))
        {

        }
    }

}