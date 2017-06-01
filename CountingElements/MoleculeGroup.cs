using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CountingElements
{
    public sealed class MoleculeGroup
    {
        const string GroupPattern = @"\(([A-Z]+[a-z]?[0-9]*)+\)[0-9]+";
        const string PartPattern = @"([A-Z]{1}[a-z]?[0-9]*)";

        public IList<MoleculePart> Parts { get; }
            = new List<MoleculePart>();

        public ushort Count { get; set; } = 1;

        public static MoleculeGroup Parse(string formula)
        {
            ArgCheck.IsNullOrEmpty(formula, nameof(formula));

            formula = EnsureGroupWrapped(formula);
            var group = new MoleculeGroup();

            if (TryParseFormula(formula, out string[] partMatches)) {
                Array.ForEach(partMatches, g => group.Parts.Add(MoleculePart.Parse(g)));
                group.Count = ParseMultiplier(formula);

                return group;
            }
            else {
                throw new FormatException("The formula is in an unexpected format; examples: NaHCO3, (NH3)2");
            }
        }

        public override string ToString()
        {
            string formula = Parts.Aggregate(string.Empty, (current, next) => current + next.ToString());

            if (Count > 1) {
                formula = "(" + formula + ")" + Count.ToString();
            }

            return formula;
        }

        static string EnsureGroupWrapped(string formula)
            => (formula.StartsWith("(") ? formula : "(" + formula + ")1");

        static bool TryParseFormula(string formula, out string[] matches)
        {
            if (Regex.IsMatch(formula, GroupPattern)) {
                var matchedParts = new List<string>();

                foreach (Match match in Regex.Matches(formula, PartPattern)) {
                    matchedParts.Add(match.Value);
                }

                matches = matchedParts.ToArray();
                return true;
            }

            matches = null;
            return false;
        }

        static ushort ParseMultiplier(string formula)
        {
            int locGroupEnd = formula.IndexOf(')');
            string multiplier = formula.Substring(locGroupEnd + 1);

            return ushort.Parse(multiplier);
        }
    }
}
