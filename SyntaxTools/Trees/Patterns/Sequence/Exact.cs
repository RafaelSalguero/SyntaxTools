using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericCompiler.PatternMatching.Permutations;

namespace SyntaxTools.Trees.Patterns.Sequence
{
    /// <summary>
    /// Matches a sequence with the same order and item count
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Exact : SequencePattern
    {
        public Exact(IReadOnlyList<TreePattern> Sequence)
        {
            this.Sequence = Sequence;
        }
        public Exact(params TreePattern[] Sequence) : this((IReadOnlyList<TreePattern>)Sequence)
        {
        }

        /// <summary>
        /// The sequence to match
        /// </summary>
        public readonly IReadOnlyList<TreePattern> Sequence;

        public override IReadOnlyList<MatchResult<string, ExpressionTree>> Match(IReadOnlyList<ExpressionTree> Sequence)
        {
            if (this.Sequence.Count != Sequence.Count)
                return new MatchResult<string, ExpressionTree>[0];

            var Digits = new IEnumerable<MatchResult<string, ExpressionTree>>[Sequence.Count];
            for (var i = 0; i < Sequence.Count; i++)
            {
                Digits[i] = this.Sequence[i].Match(Sequence[i]);
            }

            var Result = new List<MatchResult<string, ExpressionTree>>();
            var Power = PermutationGenerator.PowerCombine(Digits);
            foreach (var String in Power)
            {
                var Join = MatchResultFactory.JoinMatch(String);
                if (Join != null)
                    Result.Add(Join);
            }

            return Result.AsReadOnly();
        }

        public override string ToString()
        {
            return Sequence.Select(x => x.ToString()).Aggregate("", (a, b) => a == "" ? b : a + ", " + b);
        }
    }
}
