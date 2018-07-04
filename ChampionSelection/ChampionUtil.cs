using System.Collections.Generic;

namespace ChampionSelection
{
    public class ChampionUtil
    {
        private const string RoleQuestion = "RoleQuestion";
        private const string LaneQuestion = "LaneQuestion";
        private const string DamageQuestion = "DamageQuestion";
        private const string NewnessQuestion = "NewnessQuestion";

        //Wrapper method to determine how to filter champions given generic input
        public static List<Champion> FilterChampions(string input, string questionSymbol, List<Champion> champions)
        {
            if (input == "NoPref")
            {
                return champions;
            }
            if (questionSymbol == LaneQuestion)
            {
                Lane.TryParse(input, out Lane lane);
                return FilterChampionsByCriteria(lane, champions);
            }
            if (questionSymbol == RoleQuestion)
            {
                AttackRange.TryParse(input, out AttackRange role);
                return FilterChampionsByCriteria(role, champions);
            }
            if (questionSymbol == DamageQuestion)
            {
                DamageType.TryParse(input, out DamageType dmg);
                return FilterChampionsByCriteria(dmg, champions);
            }
            if (questionSymbol == NewnessQuestion)
            {
                IsNew.TryParse(input, out IsNew n);
                return FilterChampionsByCriteria(n, champions);
            }
            return champions;
        }

        //Overloaded methods to filter champions by input
        //All return new list of potential champions based on given input
        private static List<Champion> FilterChampionsByCriteria(Lane lane, List<Champion> champions)
        {
            List<Champion> options = new List<Champion>();
            foreach (Champion champ in champions)
            {
                if (champ.MainLane.Contains(lane))
                {
                    options.Add(champ);
                }
            }
            return options;
        }
        
        private static List<Champion> FilterChampionsByCriteria(AttackRange range, List<Champion> champions)
        {
            List<Champion> options = new List<Champion>();
            foreach (Champion champ in champions)
            {
                if (champ.Range == range)
                {
                    options.Add(champ);
                }
            }
            return options;
        }
        
        private static List<Champion> FilterChampionsByCriteria(IsNew newness, List<Champion> champions)
        {
            List<Champion> options = new List<Champion>();
            foreach (Champion champ in champions)
            {
                if (newness == champ.TryNew)
                {
                    options.Add(champ);
                }
            }
            return options;
        }
        
        private static List<Champion> FilterChampionsByCriteria(DamageType dmgType, List<Champion> champions)
        {
            List<Champion> options = new List<Champion>();
            foreach (Champion champ in champions)
            {
                if (dmgType == champ.DmgType)
                {
                    options.Add(champ);
                }
            }
            return options;
        }
    }
}