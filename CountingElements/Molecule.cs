using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CountingElements
{
    public sealed class Molecule
    {
        const string MoleculePattern = @"^(?(\()\(([A-Z]+[a-z]?[0-9]*)+\)[0-9]+|([A-Z]+[a-z]?[0-9]*)+)*$";
        const string GroupPattern = @"(?(\()\(([A-Z]+[a-z]?[0-9]*)+\)[0-9]+|([A-Z]+[a-z]?[0-9]*)+)";

        public IList<MoleculeGroup> Groups { get; }
            = new List<MoleculeGroup>();

        public IDictionary<string, ushort> CountElements()
        {
            var counts = new Dictionary<string, ushort>();

            foreach (var group in Groups) {
                CountInGroup(counts, group);
            }

            return counts;
        }

        public static Molecule Parse(string formula)
        {
            ArgCheck.IsNullOrEmpty(formula, nameof(formula));
            var molecule = new Molecule();

            if (TryParseFormula(formula, out string[] groupMatches)) {
                Array.ForEach(groupMatches, g => molecule.Groups.Add(MoleculeGroup.Parse(g)));
                return molecule;
            }
            else {
                throw new FormatException("The formula is not in the expected format; expected: E1E2...En(E1E2...En)n...");
            }
        }

        public override string ToString()
            => Groups.Aggregate(string.Empty, (current, next) => current + next.ToString());

        static void CountInGroup(IDictionary<string, ushort> counts, MoleculeGroup group)
        {
            foreach (var part in group.Parts) {
                CountPart(counts, part, group.Count);
            }
        }

        static void CountPart(IDictionary<string, ushort> counts, MoleculePart part, ushort multiplier)
        {
            var addCount = (ushort)(part.Count * multiplier);

            if (counts.ContainsKey(part.Element)) {
                counts[part.Element] += addCount;
            }
            else {
                counts.Add(part.Element, addCount);
            }
        }

        static bool TryParseFormula(string formula, out string[] matches)
        {
            if (Regex.IsMatch(formula, MoleculePattern)) {
                var matchedGroups = new List<string>();

                foreach (Match match in Regex.Matches(formula, GroupPattern)) {
                    matchedGroups.Add(match.Value);
                }

                matches = matchedGroups.ToArray();
                return true;
            }

            matches = null;
            return false;
        }
    }
}

/* NOTES:
 * GroupPattern use an alternation construct to match element groups. For instance,
 * PbCl(NH3)2(COOH)2 matches: PbCl (NH3)2 (COOH)2
 * These groups are then individually parsed.
 */