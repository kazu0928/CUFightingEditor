using System.Collections;
using System.Collections.Generic;

public static class CommonConstants
{
    public static class Names
    {
        public static readonly string head = "head";
        public static readonly string body = "body";
        public static readonly string foot = "foot";
        public static readonly string grab = "grab";
    }
    public static class Tags
    {
        public static readonly string HitBox = "HitBox";
        public static readonly string HurtBox = "HurtBox";
        public static readonly string Grab = "Grab";
        public static readonly string Ground = "Ground";
        public static string GetTags(HitBoxMode _mode)
        {
            switch (_mode)
            {
                case HitBoxMode.HitBox:
                    return HitBox;
                case HitBoxMode.HurtBox:
                    return HurtBox;
                case HitBoxMode.GrabAndSqueeze:
                    return Grab;
            }
            return "";
        }
    }
    public static class Layers
    {
        public static readonly string Ground = "Ground";
        public static readonly string Player_One = "Player1";
        public static readonly string Player_Two = "Player2";
        public static string GetPlayerNumberLayer(PlayerNumber _n)
        {
            switch(_n)
            {
                case PlayerNumber.Player1:
                    return Player_One;
                case PlayerNumber.Player2:
                    return Player_Two;
            }
            return "";
        }
    }
}
