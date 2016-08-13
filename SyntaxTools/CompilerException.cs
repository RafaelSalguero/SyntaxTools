using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;

namespace SyntaxTools
{

    /// <summary>
    /// A compiler error that was caused by the user code and not by the compiler malfunction
    /// </summary>
    public class CompilerException : Exception
    {
        public CompilerException(string message, Substring Position) : base(message)
        {
            this.Position = Position;
        }
        public CompilerException(string message, Substring Position, Exception InnerException) : base(message, InnerException)
        {
            this.Position = Position;
        }
        public readonly Substring Position;

    }
}
