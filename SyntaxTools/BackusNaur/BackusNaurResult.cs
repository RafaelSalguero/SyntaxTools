using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Global;

namespace SyntaxTools.BackusNaur
{
    /// <summary>
    /// The result of parsing with a backus naur expression
    /// </summary>
    public class BackusNaurResult
    {
        /// <summary>
        /// Create a new backus naur result
        /// </summary>
        /// <param name="ExpressionId">The id of the backus naur expression originating this result</param>
        public BackusNaurResult(Guid ExpressionId, bool Ignore)
        {
            this.ExpressionId = ExpressionId;
            this.Ignore = Ignore;
        }
        public Guid ExpressionId { get; private set; }

        /// <summary>
        /// Gets if this result doesn't contain any information
        /// </summary>
        public bool Ignore { get; private set; }

        public string ExpressionFriendlyName
        {
            get
            {
                return GuidNames.GetName(ExpressionId);
            }
        }
    }
}
