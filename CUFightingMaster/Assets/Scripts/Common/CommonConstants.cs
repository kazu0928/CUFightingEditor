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
		public static readonly string pushing = "pushing";
    }
    public static class Tags
    {
        public static readonly string HitBox = "HitBox";
        public static readonly string HurtBox = "HurtBox";
        public static readonly string Grab = "Grab";
        public static readonly string Ground = "Ground";
		public static readonly string Pushing = "Pushing";
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
				case HitBoxMode.Pushing:
					return Pushing;
            }
            return "";
        }
    }
    public static class Layers
    {
        public static readonly string Ground = "Ground";
        public static readonly string Wall = "Wall";
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
    public static class Skills
    {
        //一つ増やすたびに増やして
        public static readonly int SkillCount = 68;

        //地上の動き
        public static readonly int Idle = 0;            //待機
        public static readonly int Front_Walk = 1;      //前歩き
        public static readonly int Back_Walk = 2;       //後ろ歩き
        public static readonly int Crouching = 3;       //しゃがみ
        public static readonly int Jump = 4;            //ジャンプ
        public static readonly int Front_Jump = 5;      //前ジャンプ
        public static readonly int Back_Jump = 6;       //後ジャンプ
        public static readonly int Stand_Guard = 7;     //立ちガード
        public static readonly int Crouching_Guard = 8; //しゃがみガード
                                                    //立ちやられ上
        public static readonly int Stand_Light_Top_HitMotion = 9;
        public static readonly int Stand_Middle_Top_HitMotion = 10;
        public static readonly int Stand_Strong_Top_HitMotion = 11;
        //中
        public static readonly int Stand_Light_Middle_HitMotion = 12;
        public static readonly int Stand_Middle_Middle_HitMotion = 13;
        public static readonly int Stand_Strong_Middle_HitMotion = 14;
        //下
        public static readonly int Stand_Light_Bottom_HitMotion = 15;
        public static readonly int Stand_Middle_Bottom_HitMotion = 16;
        public static readonly int Stand_Strong_Bottom_HitMotion = 17;

        //しゃがみやられ上
        public static readonly int Crouching_Light_Top_HitMotion = 18;
        public static readonly int Crouching_Middle_Top_HitMotion = 19;
        public static readonly int Crouching_Strong_Top_HitMotion = 20;
        //中
        public static readonly int Crouching_Light_Middle_HitMotion = 21;
        public static readonly int Crouching_Middle_Middle_HitMotion = 22;
        public static readonly int Crouching_Strong_Middle_HitMotion = 23;
        //下
        public static readonly int Crouching_Light_Bottom_HitMotion = 24;
        public static readonly int Crouching_Middle_Bottom_HitMotion = 25;
        public static readonly int Crouching_Strong_Bottom_HitMotion = 26;

        //空中やられ上
        public static readonly int Air_Light_Top_HitMotion = 27;
        public static readonly int Air_Middle_Top_HitMotion = 28;
        public static readonly int Air_Strong_Top_HitMotion = 29;
        //中
        public static readonly int Air_Light_Middle_HitMotion = 30;
        public static readonly int Air_Middle_Middle_HitMotion = 31;
        public static readonly int Air_Strong_Middle_HitMotion = 32;
        //下
        public static readonly int Air_Light_Bottom_HitMotion = 33;
        public static readonly int Air_Middle_Bottom_HitMotion = 34;
        public static readonly int Air_Strong_Bottom_HitMotion = 35;
        //空中の動き
        //地上攻撃
        //立ちP
        public static readonly int Stand_Light_Jab = 36;
        public static readonly int Stand_Middle_Jab = 37;
        public static readonly int Stand_Strong_Jab = 38;

        //しゃがみP
        public static readonly int Crouching_Light_Jab = 39;
        public static readonly int Crouching_Middle_Jab = 40;
        public static readonly int Crouching_Strong_Jab = 41;

        //垂直ジャンプp
        public static readonly int Air_Light_Jab = 42;
        public static readonly int Air_Middle_Jab = 43;
        public static readonly int Air_Strong_Jab = 44;

        //前ジャンプp
        public static readonly int Air_Front_Light_Jab = 45;
        public static readonly int Air_Front_Middle_Jab = 46;
        public static readonly int Air_Front_Strong_Jab = 47;

        //後ジャンプp
        public static readonly int Air_Back_Light_Jab = 48;
        public static readonly int Air_Back_Middle_Jab = 49;
        public static readonly int Air_Back_Strong_Jab = 50;

        //立ちK
        public static readonly int Stand_Light_Kick = 51;
        public static readonly int Stand_Middle_Kick = 52;
        public static readonly int Stand_Strong_Kick = 53;

        //しゃがみK
        public static readonly int Crouching_Light_Kick = 54;
        public static readonly int Crouching_Middle_Kick = 55;
        public static readonly int Crouching_Strong_Kick = 56;

        //垂直ジャンプK
        public static readonly int Air_Light_Kick = 57;
        public static readonly int Air_Middle_Kick = 58;
        public static readonly int Air_Strong_Kick = 59;

        //前ジャンプK
        public static readonly int Air_Front_Light_Kick = 60;
        public static readonly int Air_Front_Middle_Kick = 61;
        public static readonly int Air_Front_Strong_Kick = 62;

        //後ジャンプK
        public static readonly int Air_Back_Light_Kick = 63;
        public static readonly int Air_Back_Middle_Kick = 64;
        public static readonly int Air_Back_Strong_Kick = 65;

        public static readonly int Down = 66;//ダウン
        public static readonly int Wake_Up = 67;//起き上がり
    }
}
public enum SkillConstants
{
        //地上の動き
        Idle = 0,            //待機
        Front_Walk = 1,      //前歩き
        Back_Walk = 2,       //後ろ歩き
        Crouching = 3,       //しゃがみ
        Jump = 4,            //ジャンプ
        Front_Jump = 5,      //前ジャンプ
        Back_Jump = 6,       //後ジャンプ
        Stand_Guard = 7,     //立ちガード
        Crouching_Guard = 8, //しゃがみガード
                                                    //立ちやられ上
        Stand_Light_Top_HitMotion = 9,
        Stand_Middle_Top_HitMotion = 10,
        Stand_Strong_Top_HitMotion = 11,
        //中
        Stand_Light_Middle_HitMotion = 12,
        Stand_Middle_Middle_HitMotion = 13,
        Stand_Strong_Middle_HitMotion = 14,
        //下
        Stand_Light_Bottom_HitMotion = 15,
        Stand_Middle_Bottom_HitMotion = 16,
        Stand_Strong_Bottom_HitMotion = 17,

        //しゃがみやられ上
        Crouching_Light_Top_HitMotion = 18,
        Crouching_Middle_Top_HitMotion = 19,
        Crouching_Strong_Top_HitMotion = 20,
        //中
        Crouching_Light_Middle_HitMotion = 21,
        Crouching_Middle_Middle_HitMotion = 22,
        Crouching_Strong_Middle_HitMotion = 23,
        //下
        Crouching_Light_Bottom_HitMotion = 24,
        Crouching_Middle_Bottom_HitMotion = 25,
        Crouching_Strong_Bottom_HitMotion = 26,

        //空中やられ上
        Air_Light_Top_HitMotion = 27,
        Air_Middle_Top_HitMotion = 28,
        Air_Strong_Top_HitMotion = 29,
        //中
        Air_Light_Middle_HitMotion = 30,
        Air_Middle_Middle_HitMotion = 31,
        Air_Strong_Middle_HitMotion = 32,
        //下
        Air_Light_Bottom_HitMotion = 33,
        Air_Middle_Bottom_HitMotion = 34,
        Air_Strong_Bottom_HitMotion = 35,
        //空中の動き
        //地上攻撃
        //立ちP
        Stand_Light_Jab = 36,
        Stand_Middle_Jab = 37,
        Stand_Strong_Jab = 38,

        //しゃがみP
        Crouching_Light_Jab = 39,
        Crouching_Middle_Jab = 40,
        Crouching_Strong_Jab = 41,

        //垂直ジャンプp
        Air_Light_Jab = 42,
        Air_Middle_Jab = 43,
        Air_Strong_Jab = 44,

        //前ジャンプp
        Air_Front_Light_Jab = 45,
        Air_Front_Middle_Jab = 46,
        Air_Front_Strong_Jab = 47,

        //後ジャンプp
        Air_Back_Light_Jab = 48,
        Air_Back_Middle_Jab = 49,
        Air_Back_Strong_Jab = 50,

        //立ちK
        Stand_Light_Kick = 51,
        Stand_Middle_Kick = 52,
        Stand_Strong_Kick = 53,

        //しゃがみK
        Crouching_Light_Kick = 54,
        Crouching_Middle_Kick = 55,
        Crouching_Strong_Kick = 56,

        //垂直ジャンプK
        Air_Light_Kick = 57,
        Air_Middle_Kick = 58,
        Air_Strong_Kick = 59,

        //前ジャンプK
        Air_Front_Light_Kick = 60,
        Air_Front_Middle_Kick = 61,
        Air_Front_Strong_Kick = 62,

        //後ジャンプK
        Air_Back_Light_Kick = 63,
        Air_Back_Middle_Kick = 64,
        Air_Back_Strong_Kick = 65,

        Down = 66,//ダウン
        Wake_Up = 67,//起き上がり
}