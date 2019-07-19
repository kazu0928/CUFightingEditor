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

	//体
	private List<FighterSkill.FrameHitBox> bodies = new List<FighterSkill.FrameHitBox>();
	private List<int> bodyStartFrames = new List<int>();
	private int nowPlayBodyNumber = 0;

	//足
	private List<FighterSkill.FrameHitBox> foots = new List<FighterSkill.FrameHitBox>();
	private List<int> footStartFrames = new List<int>();
	private int nowPlayFootNumber = 0;

	//掴まれ
	private List<FighterSkill.FrameHitBox> grabs = new List<FighterSkill.FrameHitBox>();
	private List<int> grabStartFrames = new List<int>();
	private int nowPlayGrabNumber = 0;

	//押し合い
	private List<FighterSkill.FrameHitBox> pushes = new List<FighterSkill.FrameHitBox>();
	private List<int> pushStartFrames = new List<int>();
	private int nowPlayPushNumber = 0;


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
		//HurtBoxMoving();
        DefaultBoxMoving(ref heads,ref nowPlayHeadsNumber,ref headCollider,core.Status.headHitBox);
		DefaultBoxMoving(ref bodies, ref nowPlayBodyNumber, ref bodyCollider, core.Status.bodyHitBox);
		DefaultBoxMoving(ref pushes, ref nowPlayPushNumber, ref pushingCollider, core.Status.pushingHitBox);
		if (core.NowPlaySkill != null)
		{
			if (core.NowPlaySkill.pushingFlag)
			{
				CheckDefaultPushingBox(pushingCollider);
			}
		}
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
			foreach (ComponentObjectPool<BoxCollider>.Objs g in nowPlayCollider)
			{
				if (g.gameObject != null)
				{
					g.gameObject.SetActive(false);
				}
			}
			//ヒットボックス格納
			customs = new List<FighterSkill.CustomHitBox>(core.NowPlaySkill.customHitBox);
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
			//当たり判定の場所移動
			DefaultColInit(ref heads, ref headCollider, core.Status.headHitBox, core.NowPlaySkill.plusHeadHitBox, ref nowPlayHeadsNumber, core.NowPlaySkill.headFlag);
			DefaultColInit(ref bodies, ref bodyCollider, core.Status.bodyHitBox, core.NowPlaySkill.plusBodyHitBox, ref nowPlayBodyNumber, core.NowPlaySkill.bodyFlag);
			DefaultColInit(ref foots, ref footCollider, core.Status.footHitBox, core.NowPlaySkill.plusFootHitBox, ref nowPlayFootNumber, core.NowPlaySkill.footFlag);
			DefaultColInit(ref grabs, ref grabCollider, core.Status.grabHitBox, core.NowPlaySkill.plusGrabHitBox, ref nowPlayGrabNumber, core.NowPlaySkill.grabFlag);
			DefaultColInit(ref pushes, ref pushingCollider, core.Status.pushingHitBox, core.NowPlaySkill.plusPushingHitBox, ref nowPlayPushNumber, core.NowPlaySkill.pushingFlag);
		}
		//なければなし
		else
		{
			customs = null;
		}
    }
	private void DefaultColInit(ref List<FighterSkill.FrameHitBox> _hit, ref BoxCollider _col, FighterStatus.HitBox_ _hitbox, List<FighterSkill.FrameHitBox> _plusBox, ref int _number, bool _flag)
	{
		if (_flag)
		{
			_col.gameObject.SetActive(true);
			//デフォルトの大きさに
			Vector3 cent = new Vector3(_hitbox.localPosition.x * RightLeft, _hitbox.localPosition.y, _hitbox.localPosition.z);
			_col.center = cent;
			_col.size = _hitbox.size;
			_hit = new List<FighterSkill.FrameHitBox>(_plusBox);
			if (_hit.Count > 1)
			{
				_hit.Sort((a, b) => a.startFrame - b.startFrame);
			}
			_number = -1;
		}
		else
		{
			_col.gameObject.SetActive(false);
		}
		//if (core.NowPlaySkill.headFlag)
		//{
		//	headCollider.gameObject.SetActive(true);
		//	//デフォルトの大きさに
		//	headCollider.center = core.Status.headHitBox.localPosition;
		//	headCollider.size = core.Status.headHitBox.size;
		//	heads = new List<FighterSkill.FrameHitBox>(core.NowPlaySkill.plusHeadHitBox);
		//	if (heads.Count > 1)
		//	{
		//		heads.Sort((a, b) => a.startFrame - b.startFrame);
		//	}
		//	nowPlayHeadsNumber = -1;
		//}
		//else
		//{
		//	headCollider.gameObject.SetActive(false);
		//}
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
			if ((heads[0].startFrame <= core.AnimationPlayerCompornent.NowFrame && heads[nowPlayHeadsNumber].startFrame > core.AnimationPlayerCompornent.NowFrame)||(heads.Count==1&& heads[nowPlayHeadsNumber].startFrame == core.AnimationPlayerCompornent.NowFrame))
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
            Vector3 tmp = core.Status.headHitBox.localPosition;
            tmp.x *= RightLeft;
            headCollider.center = tmp;
            headCollider.size = core.Status.headHitBox.size;
            return;
        }
    }
	private void DefaultBoxMoving(ref List<FighterSkill.FrameHitBox> _defs,ref int _number,ref BoxCollider _col, FighterStatus.HitBox_ _hit)
	{
		if ((_defs == null) || (_defs.Count == 0)) return;
		if (_defs.Count > _number + 1)
		{
			//現在再生中の次フレームを越えれば
			if (_defs[_number + 1].startFrame <= core.AnimationPlayerCompornent.NowFrame)
			{
				//処理
				_number++;
				Vector3 tmp = _defs[_number].hitBox.localPosition + _hit.localPosition;
				tmp.x *= RightLeft;
				_col.center = tmp;
				_col.size = _hit.size + _defs[_number].hitBox.size;
			}
		}
		//ループ時
		else
		{
			if (_defs[0].startFrame <= core.AnimationPlayerCompornent.NowFrame && _defs[_number].startFrame > core.AnimationPlayerCompornent.NowFrame || (_defs.Count == 1 && _defs[_number].startFrame == core.AnimationPlayerCompornent.NowFrame))
			{
				//処理
				_number = 0;
				Vector3 tmp = _defs[_number].hitBox.localPosition + _hit.localPosition;
				tmp.x *= RightLeft;
				_col.center = tmp;
				_col.size = _hit.size + _defs[_number].hitBox.size;
			}
		}
		if (_number < 0) return;
		if (_defs[_number].endFrame < core.AnimationPlayerCompornent.NowFrame)
		{
			Vector3 tmp = _hit.localPosition;
			tmp.x *= RightLeft;
			_col.center = tmp;
			_col.size = _hit.size;
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
	public void CheckDefaultPushingBox(BoxCollider _col)
	{
		Transform t = _col.gameObject.transform;
		float posX = t.position.x + _col.center.x;
		float posY = t.position.y + _col.center.y;
		float posZ = t.position.z + _col.center.z;
		Vector3 pos = new Vector3(posX,posY,posZ );
		Vector3 siz = new Vector3(_col.size.x / 2.0f, _col.size.y / 2.0f, _col.size.z / 2.0f);
		Collider[] col = Physics.OverlapBox(new Vector3(posX,posY,posZ), siz, Quaternion.identity, -1 - (1 << LayerMask.NameToLayer(CommonConstants.Layers.GetPlayerNumberLayer(core.PlayerNumber))));
		foreach(Collider c in col)
		{
			if (c.gameObject.tag == CommonConstants.Tags.GetTags(HitBoxMode.Pushing))
			{
				int i = 1;
				if(t.position.x>c.transform.position.x)
				{
					i = -1;
				}
				float x = (pos.x + (siz.x * i)) + ((((BoxCollider)c).size.x / 2.0f) * i) + (((BoxCollider)c).center.x);
				float checkX = x - c.gameObject.transform.parent.transform.position.x;
				if(i==1&&checkX<0)
				{
					continue;
				}
				else if(i==-1&&checkX>0)
				{
					continue;
				}
				c.gameObject.transform.parent.transform.position = new Vector3(x, c.transform.position.y, c.transform.position.z);
			}
		}
	}
}
