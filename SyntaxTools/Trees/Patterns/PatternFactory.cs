using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxTools.Operators;
using SyntaxTools.Trees;

namespace SyntaxTools.Trees.Patterns
{
    /// <summary>
    /// Converts expressions to patterns
    /// </summary>
    public static class PatternFactory
    {
        /// <summary>
        /// Convert an expression tree to a pattern with the following rules:
        /// </summary>
        /// <param name="Expression">The expression to convert</param>
        /// <param name="Variables">Pattern varibles that can match any tree</param>
        /// <param name="WildcardVariable">The name of the wildcard variable. This variable can be bound to any value, usually ?</param>
        /// <returns></returns>
        public static TreePattern  PatternFromTree(ExpressionTree  Expression, IEnumerable<string> Variables, string WildcardVariable = "?")
        {
            
            if (Expression.Childs.Count == 0)
            {
                //Check if the expression is a variable
                if (Expression.Value.Symbol == Guid.Empty)
                {
                    if (Variables.Contains(Expression.Value.Substring.ToString()))
                        return new Variable (Expression.Value.Substring.ToString());
                    else if (Expression.Value.Substring.ToString() == WildcardVariable)
                    {
                        return new Any ();
                    }
                }
                return new Leaf (Expression.Value);
            }
            else
            {
                 var ChildPatterns =  Expression.Childs.Select(x => PatternFromTree(x, Variables, WildcardVariable)).ToList();
                return new Leaf (Expression.Value, new Sequence.Exact(ChildPatterns));
            }
        }
    }
}
