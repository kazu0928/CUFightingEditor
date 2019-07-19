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

	//頭
	private List<FighterSkill.FrameHitBox> heads = new List<FighterSkill.FrameHitBox>();
	private List<int> headStartFrames = new List<int>();
	private int nowPlayHeadsNumber = 0;

    private int RightLeft = 1;
    private bool attackHit = false;//技が当たってるかどうか
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
	private BoxCollider pushingCollider = null;
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
		SetCollider(ref pushingCollider, core.Status.pushingHitBox.localPosition, core.Status.pushingHitBox.size, CommonConstants.Names.pushing, LayerMask.NameToLayer(playerNumber), CommonConstants.Tags.Pushing);


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



        if (core.Direction == PlayerDirection.Right)
        {
            RightLeft = 1;
        }
        if (core.Direction == PlayerDirection.Left)
        {
            RightLeft = -1;
        }
        //最初に地面のチェックを一度やらないと移動値がそのまま入っているのでチェック
        GroundCheck();

		//スキルごとの当たり判定に移動、拡張
        CustomHitBoxes();
        HurtBoxMoving();
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
            headCollider.center = core.Status.headHitBox.localPosition;
            headCollider.size = core.Status.headHitBox.size;
			//ヒットボックス格納
            customs = new List<FighterSkill.CustomHitBox>(core.NowPlaySkill.customHitBox);
			heads = new List<FighterSkill.FrameHitBox>(core.NowPlaySkill.plusHeadHitBox);
            nowPlayCustomNumber = new List<int>();
            nowPlayCollider = new List<ComponentObjectPool<BoxCollider>.Objs>();
            attackHit = false;
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
			if (heads.Count > 1)
			{
				heads.Sort((a, b) => a.startFrame - b.startFrame);
			}
            nowPlayHeadsNumber = -1;
        }
        //なければなし
        else
        {
            customs = null;
        }
    }
    #endregion
    private void CustomHitBoxes()
    {
        if ((customs == null) || (customs.Count == 0)) return;
        for (int i = 0; i < customs.Count; i++)
        {
            if ((customs[i].frameHitBoxes == null) || (customs.Count == 0)) continue;
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
                    Vector3 tmp = customs[i].frameHitBoxes[nowPlayCustomNumber[i]].hitBox.localPosition;
                    tmp.x *= RightLeft;
                    nowPlayCollider[i].component.center = tmp;
                    attackHit = false;
                }
            }
            //ループ時
            else
            {
                //一番最初のスタートフレーム以上かつ現在のフレームが現在の再生フレーム数より小さい場合ループありの
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
                    Vector3 tmp = customs[i].frameHitBoxes[nowPlayCustomNumber[i]].hitBox.localPosition;
                    tmp.x *= RightLeft;
                    nowPlayCollider[i].component.center = tmp;
                    attackHit = false;
                }
            }
            if (nowPlayCustomNumber[i] < 0) continue;
            if (customs[i].frameHitBoxes[nowPlayCustomNumber[i]].endFrame < core.AnimationPlayerCompornent.NowFrame)
            {
                nowPlayCollider[i].gameObject.SetActive(false);
                return;
            }
            if (attackHit == true)return;
			//ヒットボックスの当たり判定
            if (nowPlayCollider[i].gameObject.tag == CommonConstants.Tags.GetTags(HitBoxMode.HitBox))
            {
                //ダメージ判定処理
                CheckHitBox(nowPlayCollider[i].component, customs[i]);
            }
        }
    }
	//当たり判定の大きさ、場所移動
	private void HurtBoxMoving()
	{
		if ((heads == null) || (heads.Count == 0)) return;
		if (heads.Count > nowPlayHeadsNumber + 1)
		{
			//現在再生中の次フレームを越えれば
			if (heads[nowPlayHeadsNumber + 1].startFrame <= core.AnimationPlayerCompornent.NowFrame)
			{
                Debug.Log("aass");
                //処理
                nowPlayHeadsNumber++;
				Vector3 tmp = heads[nowPlayHeadsNumber].hitBox.localPosition + core.Status.headHitBox.localPosition;
				tmp.x *= RightLeft;
				headCollider.center = tmp;
                headCollider.size = core.Status.headHitBox.size + heads[nowPlayHeadsNumber].hitBox.size;
            }
		}
		//ループ時
		else
		{
			if (heads[0].startFrame <= core.AnimationPlayerCompornent.NowFrame && heads[nowPlayHeadsNumber].startFrame > core.AnimationPlayerCompornent.NowFrame)
			{
                //処理
                nowPlayHeadsNumber = 0;
                Vector3 tmp = heads[nowPlayHeadsNumber].hitBox.localPosition + core.Status.headHitBox.localPosition;
				tmp.x *= RightLeft;
				headCollider.center = tmp;
                headCollider.size = core.Status.headHitBox.size + heads[nowPlayHeadsNumber].hitBox.size;
            }
		}
		if (nowPlayHeadsNumber < 0) return;
        if (heads[nowPlayHeadsNumber].endFrame < core.AnimationPlayerCompornent.NowFrame)
        {
            Debug.Log( "aaaa");
            Vector3 tmp = core.Status.headHitBox.localPosition;
            tmp.x *= RightLeft;
            headCollider.center = tmp;
            headCollider.size = core.Status.headHitBox.size;
            return;
        }
    }
    private void CheckHitBox(BoxCollider _bCol,FighterSkill.CustomHitBox _cHit)
    {
        Transform t = _bCol.gameObject.transform;
        Collider[] col = Physics.OverlapBox(new Vector3(t.position.x + _bCol.center.x, t.position.y + _bCol.center.y, t.position.z + _bCol.center.z), _bCol.size/2, Quaternion.identity, -1 - (1 << LayerMask.NameToLayer(CommonConstants.Layers.GetPlayerNumberLayer(core.PlayerNumber))));
        foreach(Collider c in col)
        {
            if(c.gameObject.tag == CommonConstants.Tags.GetTags(HitBoxMode.HurtBox))
            {
				FighterCore cr = GameManager.Instance.GetPlayFighterCore(c.gameObject.layer);
				cr.SetDamage(_cHit);
				cr.SetEnemyNumber(core.PlayerNumber);
                attackHit = true;
                return;
            }
        }
    }
	//デフォルト(+技ごとのデフォルト拡張)の押し合い判定
	//TODO 壁判定
	public void CheckDefaultPushingBox(BoxCollider _col,FighterSkill.FrameHitBox _hitBox)
	{
		Transform t = _col.gameObject.transform;
		Vector3 pos = new Vector3(t.position.x + _col.center.x + _hitBox.hitBox.localPosition.x, t.position.y + _col.center.y + _hitBox.hitBox.localPosition.y, t.position.z + _col.center.z + _hitBox.hitBox.localPosition.z);
		Vector3 siz = (_col.size + _hitBox.hitBox.size) / 2;
		Collider[] col = Physics.OverlapBox(pos, siz, Quaternion.identity, -1 - (1 << LayerMask.NameToLayer(CommonConstants.Layers.GetPlayerNumberLayer(core.PlayerNumber))));
		foreach(Collider c in col)
		{
			//if(c.gameObject.tag == CommonConstants.Tags.GetTags(HitBoxMode.Pushing))
			//{
			//	pos.x
			//}
		}
	}
}
