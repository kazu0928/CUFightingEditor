using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Fighting/スキル")]
public class FighterSkill : ScriptableObject
{
    #region 構造体、クラス
    //当たり判定の基礎構造体
	[System.Serializable]
	public struct HitBox_
	{
		public Vector3 size;
		public Vector3 localPosition;
	}
    //当たり判定
	[System.Serializable]
	public class FrameHitBox
	{
		public HitBox_ hitBox;
		public int startFrame;
		public int endFrame;        
    }
    [System.Serializable]
    public class HitEffects
    {
        public GameObject effect;
		public GameObject guardEffect;
        public Vector3 position;
    }
    //当たり判定群
    [System.Serializable]
    public class CustomHitBox
    {
        public HitBoxMode mode;
        public List<FrameHitBox> frameHitBoxes = new List<FrameHitBox>();
        public int hitStop;             //ヒットストップ
        public HitPoint hitPoint;       //上段中段下段
        public HitStrength hitStrength; //弱中強
        public int damage;              //ダメージ
        public int stanDamage;          //スタン値
        public float knockBack;           //ノックバック値
        public float airKnockBack;
        public float guardKnockBack;
        public List<HitEffects> hitEffects;//TODO::ヒットエフェクト
        public bool isDown = false;     //ダウンするかどうか
        //ダウン時の移動
        public bool isContinue = false;
        public List<Move> movements = new List<Move>();
        public List<GravityMove> gravityMoves = new List<GravityMove>();

        public bool isFaceDown = false; //うつ伏せかどうか
        public bool isPassiveNotPossible = false;//受け身不可
        public int hitRigor;            //ヒット硬直
        public int guardHitRigor;       //ガード硬直
        public int plusGauge;           //ゲージ増加量
    }
    //移動量
    [System.Serializable]
	public class Move
	{
		public Vector3 movement;
		public int startFrame;
	}
    //重力用
	[System.Serializable]
	public class GravityMove
	{
		public Vector3 movement;
		public Vector3 limitMove;
		public int startFrame;
	}
    #endregion
    public AnimationClip animationClip = null;  //再生するアニメーション
    public float animationSpeed = 1;            //アニメーションの速度
    public SkillStatus status = SkillStatus.Normal;                  //Normal,Special等
                                                //地上か空中か
    public HitMode hitMode = HitMode.Normal;    //投げかどうか
    public AnimationClip throwMotion = null;    //投げだった場合のモーション
    public AnimationClip enemyThrowMotion = null;//相手の投げられモーション
                                                //TODO::エフェクトリスト
    public SkillStatus cancelFrag = (SkillStatus)(1<<0);//キャンセルできるもの(ビット)
                                                        //TODO::飛び道具
    public bool barrageCancelFrag = false;      //連打キャンセル
    public int cancelLayer = 0;                 //キャンセルできるレイヤー
    //ブレンドするかしないか
    public bool inBlend = false;
    public bool outBlend = false;

    //当たり判定
	public List<FrameHitBox> plusHeadHitBox = new List<FrameHitBox>();//頭
	public List<FrameHitBox> plusBodyHitBox = new List<FrameHitBox>();//体
	public List<FrameHitBox> plusFootHitBox = new List<FrameHitBox>();//足
    public List<FrameHitBox> plusGrabHitBox = new List<FrameHitBox>();//掴み
	public List<FrameHitBox> plusPushingHitBox = new List<FrameHitBox>();//押し合い

    public List<CustomHitBox> customHitBox = new List<CustomHitBox>();//カスタム

	//重力継続判定
	public bool isContinue = false;
	//移動
	public List<Move> movements = new List<Move>();
    public List<GravityMove> gravityMoves = new List<GravityMove>();

    //Default当たり判定があるかないか
    public bool headFlag = true;
    public bool bodyFlag = true;
    public bool footFlag = true;
    public bool grabFlag = true;
	public bool pushingFlag = true;

    #region EDITOR_
#if UNITY_EDITOR
    [CustomEditor(typeof(FighterSkill))]
    public class FighterSkillInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("技設定画面を開く"))
            {
                PlayerSkillEditor.Open((FighterSkill)target);
            }
        }
    }
#endif
    #endregion
}
