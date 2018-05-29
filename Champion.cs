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
        Jungle
    }

    public enum Role
    {
        Fighter = 0,
        Ranged = 1,
        Tank = 2,
        Marksman = 3,
        Melee = 4
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

    public class Champions
    {
        private List<Champion> champions { get; }

        public Champions(ChampionDto champData)
        {
            champions = new List<Champion>();
            foreach (var c in champData.Data)
            {
                champions.Add(new Champion(c.Value.Name, c.Value.Tags, c.Value.Info));
            }
        }

        public List<Champion> FilterCrewByCriteria(Answer answer)
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

    public class Champion
    {
        private string _name;
        private List<Lane> _mainLane;
        private List<Role> _roles;
        private DamageType _damageType;
        private IsNew _isNew;

        public DamageType DmgType
        {
            get { return _damageType; }
        }

        public IsNew TryNew
        {
            get { return _isNew; }
        }

        public List<Lane> MainLane
        {
            get { return _mainLane; }
        }

        public List<Role> Roles
        {
            get { return _roles; }
        }

        public Champion(string name, List<string> tags, Dictionary<string, int> damageTypes)
        {
            this._name = name;
            this._damageType = GetDamageType(damageTypes);
            this._mainLane = GetLanes(tags);
        }

        private List<Lane> GetLanes(List<string> tags)
        {
            List<Lane> lanes = new List<Lane>();
            bool isFighter = tags.Contains("fighter");
            bool isAssassin = tags.Contains("assassin");
            bool isMage = tags.Contains("mage");
            bool isTank = tags.Contains("tank");
            bool isSupport = tags.Contains("support");
            bool isMarksman = tags.Contains("marksman");

            //Super edge cases
            if (_name == "kindred")
            {
                lanes.Add(Lane.Jungle);
                return lanes;
            }
            if (_name == "graves")
            {
                lanes.Add(Lane.Jungle);
                lanes.Add(Lane.Bottom);
                return lanes;
            }
            if (isSupport)
            {
                lanes.Add(Lane.Support);
            }
            if (isFighter && !isMage && (_damageType == DamageType.AD || _damageType == DamageType.Hybrid))
            {
                lanes.Add(Lane.Top);
                lanes.Add(Lane.Jungle);
            }
            else if (isFighter && _damageType == DamageType.AP)
            {
                lanes.Add(Lane.Mid);
                lanes.Add(Lane.Top);
            }
            else if (isAssassin && _damageType == DamageType.AP)
            {
                lanes.Add(Lane.Mid);
            }
            else if (isAssassin && (_damageType == DamageType.AD || _damageType == DamageType.Hybrid))
            {
                lanes.Add(Lane.Mid);
                lanes.Add(Lane.Top);
            }
            else if (isFighter && isMage)
            {
                lanes.Add(Lane.Mid);
                lanes.Add(Lane.Jungle);
            }
            else if (isTank && isFighter && _damageType == DamageType.AD)
            {
                lanes.Add(Lane.Top);
            }
            else if (isTank && isFighter && _damageType == DamageType.AP)
            {
                lanes.Add(Lane.Support);
            }
            else if (isTank && isFighter && _damageType == DamageType.Hybrid)
            {
                lanes.Add(Lane.Jungle);
            }
            else if (isMarksman && (_damageType == DamageType.AD || _damageType == DamageType.Hybrid))
            {
                lanes.Add(Lane.Bottom);
            }
            else if (isMarksman && _damageType == DamageType.AP)
            {
                lanes.Add(Lane.Top);
            }
            else if (isTank && (_damageType == DamageType.AD || _damageType == DamageType.AP))
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
    }
}