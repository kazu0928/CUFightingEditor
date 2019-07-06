using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxJudgement
{
    private FighterCore core;
    private Transform transform;//動かすTransform
    public bool isGround { get;private set; }

    private List<FighterSkill.CustomHitBox> customs = new List<FighterSkill.CustomHitBox>();
    private List<int> startFrames = new List<int>();
    private List<int> nowPlayCustomNumber = new List<int>();
    private List<ComponentObjectPool<BoxCollider>.Objs> nowPlayCollider = new List<ComponentObjectPool<BoxCollider>.Objs>();

    #region 初期化
    public HitBoxJudgement(FighterCore fighter)
	{
		transform = fighter.transform;
		core = fighter;
		Init();
	}
 
    #region 当たり判定
	private BoxCollider headCollider = null;
	private BoxCollider bodyCollider = null;
	private BoxCollider footCollider = null;
	private BoxCollider grabCollider = null;
	private ComponentObjectPool<BoxCollider> hitBoxCollider;
    #endregion

    //初期化
    private void Init()
    {
        //どのプレイヤーか
        string playerNumber = core.PlayerNumber.ToString();
        //デフォルトのコライダーの作成
        SetCollider(ref headCollider, core.Status.headHitBox.localPosition, core.Status.headHitBox.size, CommonConstants.Names.head, LayerMask.NameToLayer(playerNumber), CommonConstants.Tags.HurtBox);
        SetCollider(ref bodyCollider, core.Status.bodyHitBox.localPosition, core.Status.bodyHitBox.size, CommonConstants.Names.body, LayerMask.NameToLayer(playerNumber), CommonConstants.Tags.HurtBox);
        SetCollider(ref footCollider, core.Status.footHitBox.localPosition, core.Status.footHitBox.size, CommonConstants.Names.foot, LayerMask.NameToLayer(playerNumber), CommonConstants.Tags.HurtBox);
        SetCollider(ref grabCollider, core.Status.grabHitBox.localPosition, core.Status.grabHitBox.size, CommonConstants.Names.grab, LayerMask.NameToLayer(playerNumber), CommonConstants.Tags.Grab);
        //hitboxのプール
        hitBoxCollider = new ComponentObjectPool<BoxCollider>(10,"hitMan",core.gameObject);
    }
    #endregion
    //コライダーのゲームオブジェクトを作成する
    private void SetCollider(ref BoxCollider col, Vector3 pos, Vector3 size, string name, int layer, string tag)
	{
		GameObject obj = new GameObject();//生成
		obj.transform.parent = transform;//親をファイターに
		obj.name = name;//名前付け
		col = obj.gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;//BoxCollider取得
        //大きさと場所の設定
		col.center = pos;
		col.size = new Vector3(size.x, size.y, 1);
        //isTriggerはon
		col.isTrigger = true;
		col.transform.localPosition = Vector3.zero;
        //タグとレイヤー
		col.gameObject.tag = tag;
		col.gameObject.layer = layer;
	}
    public void UpdateGame()
    {
        ChangeSkillInit();
        CustomHitBoxes();
        GroundCheck();
    }
    //地面判定
	private void GroundCheck()
	{
		Vector3 vector3 = new Vector3(2, 8, 2);
		Collider[] col = Physics.OverlapBox(new Vector3(transform.position.x, transform.position.y + (vector3.y / 2), transform.position.z), vector3 / 2.0f, Quaternion.identity, 1 << LayerMask.NameToLayer(CommonConstants.Layers.Ground));
		if (col.Length <= 0)
		{
            isGround = false;
            return;
		}
        isGround = true;
        //transform.positionをコライダーの上に
        transform.position = new Vector3(transform.position.x, col[0].transform.position.y + ((BoxCollider)col[0]).center.y + (((BoxCollider)col[0]).size.y / 2), transform.position.z);
    }
	public void SetGround(bool _f)
	{
		isGround = _f;
	}
    	#region 技入れ替えチェック
	//入れ替わり処理
	private void ChangeSkillInit()
	{
		//入れ替わったかどうか
		if (core.changeSkill == false) return;
        if (core.NowPlaySkill != null)
        {
            foreach(ComponentObjectPool<BoxCollider>.Objs g in nowPlayCollider)
            {
                if (g.gameObject != null)
                {
                    g.gameObject.SetActive(false);
                }
            }
            customs = new List<FighterSkill.CustomHitBox>(core.NowPlaySkill.customHitBox);
            nowPlayCustomNumber = new List<int>();
            nowPlayCollider = new List<ComponentObjectPool<BoxCollider>.Objs>();
            foreach (FighterSkill.CustomHitBox c in customs)
            {
                c.frameHitBoxes = new List<FighterSkill.FrameHitBox>(c.frameHitBoxes);
                nowPlayCollider.Add(new ComponentObjectPool<BoxCollider>.Objs());
            }
            //移動配列のソート、フレームが近い順に並べる
            for (int i = 0; i < customs.Count; i++)
            {
                if (customs[i].frameHitBoxes.Count > 1)
                {
                    customs[i].frameHitBoxes.Sort((a, b) => a.startFrame - b.startFrame);
                }
                nowPlayCustomNumber.Add(-1);
            }
        }
        //なければなし
        else
        {
            customs = null;
        }
    }
    private void CustomHitBoxes()
    {
        if ((customs == null) || (customs.Count == 0)) return;
        for (int i = 0; i < customs.Count; i++)
        {
            if((customs[i].frameHitBoxes == null)||(customs.Count == 0)) continue;
            if (customs[i].frameHitBoxes.Count > nowPlayCustomNumber[i] + 1)
            {
                //現在再生中の次フレームを越えれば
                if (customs[i].frameHitBoxes[nowPlayCustomNumber[i] + 1].startFrame <= core.AnimationPlayerCompornent.NowFrame)
                {
                    //処理
                    if (nowPlayCollider[i].gameObject != null)
                    {
                        nowPlayCollider[i].gameObject.SetActive(false);
                    }
                    nowPlayCustomNumber[i]++;
                    nowPlayCollider[i] = hitBoxCollider.GetObjects();
                    nowPlayCollider[i].gameObject.tag = CommonConstants.Tags.GetTags(customs[i].mode);
                    nowPlayCollider[i].gameObject.layer = LayerMask.NameToLayer(CommonConstants.Layers.GetPlayerNumberLayer(core.PlayerNumber));
                    nowPlayCollider[i].component.size = customs[i].frameHitBoxes[nowPlayCustomNumber[i]].hitBox.size;
                    nowPlayCollider[i].component.center = customs[i].frameHitBoxes[nowPlayCustomNumber[i]].hitBox.localPosition;
                }
            }
            //ループ時
            else
            {
                if (customs[i].frameHitBoxes[0].startFrame <= core.AnimationPlayerCompornent.NowFrame && customs[i].frameHitBoxes[nowPlayCustomNumber[i]].startFrame > core.AnimationPlayerCompornent.NowFrame)
                {
                    //処理
                    if (nowPlayCollider[i].gameObject != null)
                    {
                        nowPlayCollider[i].gameObject.SetActive(false);
                    }
                    nowPlayCustomNumber[i] = 0;
                    //処理
                    nowPlayCollider[i] = hitBoxCollider.GetObjects();
                    nowPlayCollider[i].gameObject.tag = CommonConstants.Tags.GetTags(customs[i].mode);
                    nowPlayCollider[i].gameObject.layer = LayerMask.NameToLayer(CommonConstants.Layers.GetPlayerNumberLayer(core.PlayerNumber));
                    nowPlayCollider[i].component.size = customs[i].frameHitBoxes[nowPlayCustomNumber[i]].hitBox.size;
                    nowPlayCollider[i].component.center = customs[i].frameHitBoxes[nowPlayCustomNumber[i]].hitBox.localPosition;
                }
            }
            if (nowPlayCustomNumber[i] < 0) continue;
            if(customs[i].frameHitBoxes[nowPlayCustomNumber[i]].endFrame < core.AnimationPlayerCompornent.NowFrame)
            {
                nowPlayCollider[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion
}
