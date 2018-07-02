using System.Collections.Generic;

namespace ChampionSelector
{
    public enum Lane
    {
        Bottom,
        Mid,
        Top,
        Support,
        Jungle,
        NoPref
    }

    public enum AttackRange
    {
        Ranged,
        Melee,
        NoPref
    }

    public enum DamageType
    {
        AD,
        AP,
        Hybrid,
        NoPref
    }

    public enum IsNew
    {
        Yes,
        No,
        NoPref
    }

    public class Champion
    {
        private readonly int _id;

        public DamageType DmgType { get; }
        public IsNew TryNew { get; }
        public List<Lane> MainLane { get; }
        public AttackRange Range { get; }
        public string Name { get; }
        const int maxDiffRange = 2;

        public Champion(string name, int id, List<string> tags, Dictionary<string, int> damageTypes, Dictionary<string, float> stats, List<Mastery> masteries)
        {
            Name = name;
            _id = id;
            DmgType = GetDamageType(damageTypes);
            MainLane = GetLanes(tags);
            Range = GetAttackRange(stats);
            TryNew = GetNewness(masteries);
        }

        // Get the lane the champion plays by a combination of damage type and designated roles
        // Theoretically this could change if the meta does, but we'll go with what's generally accepted for now
        private List<Lane> GetLanes(List<string> tags)
        {
            List<Lane> lanes = new List<Lane>();
            bool isFighter = tags.Contains("Fighter");
            bool isAssassin = tags.Contains("Assassin");
            bool isMage = tags.Contains("Mage");
            bool isTank = tags.Contains("Tank");
            bool isSupport = tags.Contains("Support");
            bool isMarksman = tags.Contains("Marksman");

            // Super edge cases, these champions are known junglers, despite being marksman and AD. 
            if (Name == "kindred")
            {
                lanes.Add(Lane.Jungle);
                return lanes;
            }
            if (Name == "graves")
            {
                lanes.Add(Lane.Jungle);
                lanes.Add(Lane.Bottom);
                return lanes;
            }
            //
            if (isSupport)
            {
                lanes.Add(Lane.Support);
            }
            if (isFighter && !isMage && (DmgType == DamageType.AD || DmgType == DamageType.Hybrid))
            {
                lanes.Add(Lane.Top);
                lanes.Add(Lane.Jungle);
            }
            else if (isFighter && !isTank && DmgType == DamageType.AP)
            {
                lanes.Add(Lane.Mid);
                lanes.Add(Lane.Top);
            }
            else if (isAssassin && DmgType == DamageType.AP)
            {
                lanes.Add(Lane.Mid);
            }
            else if (isAssassin && (DmgType == DamageType.AD || DmgType == DamageType.Hybrid))
            {
                lanes.Add(Lane.Mid);
                lanes.Add(Lane.Top);
            }
            else if (isFighter && isMage)
            {
                lanes.Add(Lane.Mid);
                lanes.Add(Lane.Jungle);
            }
            else if (isTank && isFighter && DmgType == DamageType.AD)
            {
                lanes.Add(Lane.Top);
            }
            else if (isTank && isFighter && DmgType == DamageType.AP)
            {
                lanes.Add(Lane.Support);
            }
            else if (isTank && isFighter && DmgType == DamageType.Hybrid)
            {
                lanes.Add(Lane.Jungle);
            }
            else if (isMarksman && (DmgType == DamageType.AD || DmgType == DamageType.Hybrid))
            {
                lanes.Add(Lane.Bottom);
            }
            else if (isMarksman && DmgType == DamageType.AP)
            {
                lanes.Add(Lane.Top);
            }
            else if (isTank && (DmgType == DamageType.AD || DmgType == DamageType.AP))
            {
                lanes.Add(Lane.Top);
                lanes.Add(Lane.Jungle);
            }
            return lanes;
        }

        private DamageType GetDamageType(Dictionary<string, int> damageTypes)
        {
            int diff = damageTypes["attack"] - damageTypes["magic"];
            if (diff > maxDiffRange)
            {
                return DamageType.AD;
            }
            if (diff < -maxDiffRange)
            {
                return DamageType.AP;
            }
            return DamageType.Hybrid;
        }

        private AttackRange GetAttackRange(Dictionary<string, float> stats)
        {
            if (stats["attackrange"] >= 450)
            {
                return AttackRange.Ranged;
            }
            return AttackRange.Melee;
        }

        private IsNew GetNewness(List<Mastery> masteries)
        {
            foreach (Mastery mastery in masteries)
            {
                if (mastery.ChampionId == _id)
                {
                    if (mastery.ChampionLevel >= 4)
                    {
                        return IsNew.No;
                    }
                }
            }
            return IsNew.Yes;
        }
    }
}