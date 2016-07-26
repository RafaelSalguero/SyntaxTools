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
    public class Exact<T> : SequencePattern<T>
    {
        public Exact(IEnumerable<TreePattern<T>> Sequence)
        {
            this.Sequence = Sequence.ToList();
        }
        public Exact(params TreePattern<T>[] Sequence) : this((IEnumerable<TreePattern<T>>)Sequence)
        {
        }

        /// <summary>
        /// The sequence to match
        /// </summary>
        public readonly IReadOnlyList<TreePattern<T>> Sequence;

        public override IReadOnlyList<MatchResult<T, Tree<T>>> Match(IReadOnlyList<Tree<T>> Sequence)
        {
            if (this.Sequence.Count != Sequence.Count)
                return new MatchResult<T, Tree<T>>[0];

            var Digits = new IEnumerable<MatchResult<T, Tree<T>>>[Sequence.Count];
            for (var i = 0; i < Sequence.Count; i++)
            {
                Digits[i] = this.Sequence[i].Match(Sequence[i]);
            }

            var Result = new List<MatchResult<T, Tree<T>>>();
            var Power = PermutationGenerator.PowerCombine(Digits);
            foreach (var String in Power)
            {
                var Join = MatchResultFactory.JoinMatch(String);
                if (Join != null)
                    Result.Add(Join);
            }

            return Result.AsReadOnly();
        }
    }
}
