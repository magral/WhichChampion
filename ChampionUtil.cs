using System.Collections.Generic;

namespace ChampionSelector
{
    public class ChampionUtil
    {
        private const string RoleQuestion = "RoleQuestion";
        private const string LaneQuestion = "LaneQuestion";
        private const string DamageQuestion = "DamageQuestion";
        private const string NewnessQuestion = "NewnessQuestion";

        public static List<Champion> FilterChampions(string input, string questionSymbol, List<Champion> champions)
        {
            if (questionSymbol == LaneQuestion)
            {
                Lane.TryParse(input, out Lane lane);
                return FilterChampionsByCriteria(lane, champions);
            }
            else if (questionSymbol == RoleQuestion)
            {
                Role.TryParse(input, out Role role);
                return FilterChampionsByCriteria(role, champions);
            }
            else if (questionSymbol == DamageQuestion)
            {
                DamageType.TryParse(input, out DamageType dmg);
                return FilterChampionsByCriteria(dmg, champions);
            }
            else if (questionSymbol == NewnessQuestion)
            {
                IsNew.TryParse(input, out IsNew n);
                return FilterChampionsByCriteria(n, champions);
            }
            return champions;
        }

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
        
        private static List<Champion> FilterChampionsByCriteria(Role role, List<Champion> champions)
        {
            List<Champion> options = new List<Champion>();
            foreach (Champion champ in champions)
            {
                if (champ.Roles.Contains(role))
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
                if (dmgType == champ.DmgType;)
                {
                    options.Add(champ);
                }
            }
            return options;
        }
    }
}