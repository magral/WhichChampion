using System;
using System.Collections;
using System.Collections.Generic;
using Gtk;

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

    public enum Playstyle
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
        public Playstyle Style { get; }
        public string Name { get; }

        public Champion(string name, int id, List<string> tags, Dictionary<string, int> damageTypes, Dictionary<string, float> stats, MasteryDto masteries)
        {
            Name = name;
            _id = id;
            DmgType = GetDamageType(damageTypes);
            MainLane = GetLanes(tags);
            Style = GetPlaystyle(stats);
            TryNew = GetNewness(masteries.Masteries);
        }

        // Get the lane the champion plays by a combination of damage type and designated roles
        // Theoretically this could change if the meta does, but we'll go with what's generally accepted for now
        private List<Lane> GetLanes(List<string> tags)
        {
            List<Lane> lanes = new List<Lane>();
            bool isFighter = tags.Contains("fighter");
            bool isAssassin = tags.Contains("assassin");
            bool isMage = tags.Contains("mage");
            bool isTank = tags.Contains("tank");
            bool isSupport = tags.Contains("support");
            bool isMarksman = tags.Contains("marksman");

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
            else if (isFighter && DmgType == DamageType.AP)
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
            if (damageTypes["attack"] > damageTypes["magic"])
            {
                return DamageType.AD;
            }
            if (damageTypes["attack"] < damageTypes["magic"])
            {
                return DamageType.AP;
            }
            return DamageType.Hybrid;
        }

        private Playstyle GetPlaystyle(Dictionary<string, float> stats)
        {
            if (stats["attackrange"] >= 450)
            {
                return Playstyle.Ranged;
            }
            return Playstyle.Melee;
        }

        private IsNew GetNewness(List<Mastery> masteries)
        {
            foreach (Mastery mastery in masteries)
            {
                if (mastery.ChampionId == _id)
                {
                    if (mastery.ChampionLevel >= 4)
                    {
                        return IsNew.Yes;
                    }
                }
            }
            return IsNew.No;
        }
    }
}