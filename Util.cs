using System;

namespace ChampionSelector
{
    public class Util
    {
        public Type GetEnumType(string type)
        {
            switch (type)
            {
                case "role":
                    return typeof(Role);
                case "lane":
                    return typeof(Lane);
                case "dmg":
                    return typeof(DamageType);
                case "newness":
                    return typeof(IsNew);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}