using System.Collections.Generic;

namespace ChampionSelector
{
    public class ChampionUtil
    {
        public static List<Champion> FilterCrewByCriteria(Answer answer, List<Champion> champions)
        {
            List<Champion> options = new List<Champion>();
            foreach (Champion champ in champions)
            {
                bool hasLane = champ.MainLane.Contains(answer.lane);
                bool hasDamageType = answer.dmgType == champ.DmgType;
                bool hasNewness = answer.isNew == champ.TryNew;
                bool hasRole = champ.Roles.Contains(answer.role);
                if (hasLane && hasDamageType && hasNewness && hasRole)
                {
                    options.Add(champ);
                }
            }
            return options;
        }
    }
}