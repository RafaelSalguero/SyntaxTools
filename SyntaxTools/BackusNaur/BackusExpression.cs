using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.DataStructures;
using SyntaxTools.Text;

namespace SyntaxTools.BackusNaur
{
    /// <summary>
    /// Matches a sequence of tokens with a given pattern
    /// </summary>
    public abstract class BackusExpression
    {
        public Guid ExpressionId
        {
            get; private set;
        } = Guid.NewGuid();

        /// <summary>
        /// Parse the given symbol queue. A failed parsing attempt must leave the symbol queue untouched, as it was at the beggining of the parsing
        /// </summary>
        /// <param name="Symbols">The symbol queue to parse</param>
        public abstract BackusNaurResult Parse(StateDequeue<TokenSubstring> Symbols);

        public sealed override string ToString()
        {
            if (Global.GuidNames.HasName(ExpressionId))
                return Global.GuidNames.GetName(ExpressionId);
            else
                return InternalToString();
        }

        protected abstract string InternalToString();
    }
}
