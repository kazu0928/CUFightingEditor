using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateDamage : StateBaseScriptMonoBehaviour
{
	private FighterStateBase stateBase;
	private int hitRigor = 0;
	private int hitCount = 0;

	private bool isEndStun = false;
	private void Start()
	{
		stateBase = GetComponent<FighterStateBase>();
	}
	#region 地上やられ
	//やられ
	public void HitStunStart()
	{
		isEndStun = false;
		FighterSkill.CustomHitBox box = stateBase.core.GetDamage;
		//硬直
		hitRigor = box.hitRigor;
		hitCount = 0;

		//ヒットストップ
		GameManager.Instance.SetHitStop(stateBase.core.PlayerNumber, box.hitStop);
		//相打ち時に受けたほうを優先する
		if (GameManager.Instance.GetHitStop(stateBase.core.EnemyNumber) <= 0)
		{
			GameManager.Instance.SetHitStop(stateBase.core.EnemyNumber, box.hitStop);
		}
		//立ちやられ
		if (stateBase.core.GetDamage.frameHitBoxes.Count > 0)
		{
            //ダメージ処理
            stateBase.core.HP -= box.damage;
			if(stateBase.core.HP<0)
			{
                stateBase.core.HP = 0;
            }
            switch(box.hitPoint)
			{
				case HitPoint.Bottom:
					switch(box.hitStrength)
					{
						case HitStrength.Light:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Light_Bottom_HitMotion, 0);
							break;
						case HitStrength.Middle:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Middle_Bottom_HitMotion, 0);
							break;
						case HitStrength.Strong:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Strong_Bottom_HitMotion, 0);
							break;
					}
					break;
				case HitPoint.Middle:
					switch (box.hitStrength)
					{
						case HitStrength.Light:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Light_Middle_HitMotion, 0);
							break;
						case HitStrength.Middle:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Middle_Middle_HitMotion, 0);
							break;
						case HitStrength.Strong:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Strong_Middle_HitMotion, 0);
							break;
					}
					break;
				case HitPoint.Top:
					switch (box.hitStrength)
					{
						case HitStrength.Light:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Light_Top_HitMotion, 0);
							break;
						case HitStrength.Middle:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Middle_Top_HitMotion, 0);
							break;
						case HitStrength.Strong:
							stateBase.ChangeSkillConstant(SkillConstants.Stand_Strong_Top_HitMotion, 0);
							break;
					}
					break;
			}
		}
		//ノックバックのセット
        PlayerDirection tmpDir;
		//右向きにノックバックするか左向きにノックバックするか
        if(GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Right)
		{
            tmpDir = PlayerDirection.Left;
        }
		else 
		{
            tmpDir = PlayerDirection.Right;
        }
		//エフェクト再生
		BoxCollider _bCol = stateBase.core.GetDamageCollider;
		Transform t = _bCol.gameObject.transform;
		for (int i = 0; i < box.hitEffects.Count; i++)
		{
			if (box.hitEffects[i].effect != null)
			{
				if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Right)
				{
					Object.Instantiate(box.hitEffects[i].effect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.identity);
				}
				else if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Left)
				{
					Object.Instantiate(box.hitEffects[i].effect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.Euler(0, 180, 0));
				}
			}
		}

		stateBase.core.SetKnockBack(box.knockBack,stateBase.core.EnemyNumber,tmpDir);
		//ダメージを受けたのでリセット
		stateBase.core.SetDamage(new FighterSkill.CustomHitBox(),null);
	}
	#endregion

	#region 空中やられ
    //やられ
    public void AirHitStunStart()
    {
        isEndStun = false;
        FighterSkill.CustomHitBox box = stateBase.core.GetDamage;
        //硬直
        hitRigor = box.hitRigor;
        hitCount = 0;

        //ヒットストップ
        GameManager.Instance.SetHitStop(stateBase.core.PlayerNumber, box.hitStop);
        //相打ち時に受けたほうを優先する
        if (GameManager.Instance.GetHitStop(stateBase.core.EnemyNumber) <= 0)
        {
            GameManager.Instance.SetHitStop(stateBase.core.EnemyNumber, box.hitStop);
        }
        //立ちやられ
        if (stateBase.core.GetDamage.frameHitBoxes.Count > 0)
        {
            //ダメージ処理
            stateBase.core.HP -= box.damage;
            if (stateBase.core.HP < 0)
            {
                stateBase.core.HP = 0;
            }
            switch (box.hitPoint)
            {
                case HitPoint.Bottom:
                    switch (box.hitStrength)
                    {
                        case HitStrength.Light:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Light_Bottom_HitMotion, 0);
                            break;
                        case HitStrength.Middle:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Middle_Bottom_HitMotion, 0);
                            break;
                        case HitStrength.Strong:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Strong_Bottom_HitMotion, 0);
                            break;
                    }
                    break;
                case HitPoint.Middle:
                    switch (box.hitStrength)
                    {
                        case HitStrength.Light:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Light_Middle_HitMotion, 0);
                            break;
                        case HitStrength.Middle:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Middle_Middle_HitMotion, 0);
                            break;
                        case HitStrength.Strong:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Strong_Middle_HitMotion, 0);
                            break;
                    }
                    break;
                case HitPoint.Top:
                    switch (box.hitStrength)
                    {
                        case HitStrength.Light:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Light_Top_HitMotion, 0);
                            break;
                        case HitStrength.Middle:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Middle_Top_HitMotion, 0);
                            break;
                        case HitStrength.Strong:
                            stateBase.ChangeSkillConstant(SkillConstants.Air_Strong_Top_HitMotion, 0);
                            break;
                    }
                    break;
            }
        }
        PlayerDirection tmpDir;

        //ノックバックのセット
        if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Right)
        {
            tmpDir = PlayerDirection.Left;
        }
        else
        {
            tmpDir = PlayerDirection.Right;
        }

		//エフェクト再生
		BoxCollider _bCol = stateBase.core.GetDamageCollider;
		Transform t = _bCol.gameObject.transform;
		for (int i = 0; i < box.hitEffects.Count; i++)
		{
			if (box.hitEffects[i].effect != null)
			{
				if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Right)
				{
					Object.Instantiate(box.hitEffects[i].effect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.identity);
				}
				else if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Left)
				{
					Object.Instantiate(box.hitEffects[i].effect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.Euler(0, 180, 0));
				}
			}
		}

		//ノックバックのセット
		stateBase.core.SetKnockBack(box.airKnockBack, stateBase.core.EnemyNumber,tmpDir, 6);
        stateBase.core.SetDamage(new FighterSkill.CustomHitBox(),null);
    }
	#endregion

	#region 飛ばし（ダウン技）
	public void DownHitStart()
	{
		isEndStun = false;
        FighterSkill.CustomHitBox box = stateBase.core.GetDamage;
        //ヒット硬直（空中受け身まで）
        hitRigor = box.hitRigor;
        hitCount = 0;

        //ヒットストップ
        GameManager.Instance.SetHitStop(stateBase.core.PlayerNumber, box.hitStop);
        //相打ち時に受けたほうを優先する
        if (GameManager.Instance.GetHitStop(stateBase.core.EnemyNumber) <= 0)
        {
            GameManager.Instance.SetHitStop(stateBase.core.EnemyNumber, box.hitStop);
        }
        //ダウンやられ
        if (stateBase.core.GetDamage.frameHitBoxes.Count > 0)
        {
            //ダメージ処理
            stateBase.core.HP -= box.damage;
            if (stateBase.core.HP < 0)
            {
                stateBase.core.HP = 0;
            }
            stateBase.ChangeSkillCustomMoveConstant(SkillConstants.Damage_Fly_HitMotion,0,box.movements,box.gravityMoves,box.isContinue);
        }
        PlayerDirection tmpDir;

        //ノックバックのセット
        if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Right)
        {
            tmpDir = PlayerDirection.Left;
        }
        else
        {
            tmpDir = PlayerDirection.Right;
        }

		//エフェクト再生
		BoxCollider _bCol = stateBase.core.GetDamageCollider;
		Transform t = _bCol.gameObject.transform;
		for (int i = 0; i < box.hitEffects.Count; i++)
		{
			if (box.hitEffects[i].effect != null)
			{
				if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Right)
				{
					Object.Instantiate(box.hitEffects[i].effect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.identity);
				}
				else if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Left)
				{
					Object.Instantiate(box.hitEffects[i].effect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.Euler(0, 180, 0));
				}
			}
		}

		//ノックバックのセット
		stateBase.core.SetKnockBack(box.airKnockBack, stateBase.core.EnemyNumber,tmpDir, 6);
        stateBase.core.SetDamage(new FighterSkill.CustomHitBox(),null);
	}
	#endregion

	//ヒット硬直時間をプラス
	public void HitStunUpdate()
	{
		if (GameManager.Instance.GetHitStop(stateBase.core.PlayerNumber) <= 0)
		{
			hitCount++;
		}
	}
	//受け身再生
	public void PlayPassive()
	{
        stateBase.core.SetKnockBack(0,PlayerNumber.None, PlayerDirection.Right);
        Direction dir = stateBase.input.GetPlayerMoveDirection(stateBase);
		if (dir == Direction.Back)
		{
			stateBase.ChangeSkillConstant(SkillConstants.Air_Back_Passive, 0);
		}
		else if(dir == Direction.Front)
		{
			stateBase.ChangeSkillConstant(SkillConstants.Air_Front_Passive, 0);
		}
		else
		{
			stateBase.ChangeSkillConstant(SkillConstants.Air_Passive, 0);
		}
	}
    //ステートエンド
    public void EndHitStunFlag()
    {

        isEndStun = true;

    }
    //ダウンかどうか
    public bool isDownHit()
    {
        return stateBase.core.GetDamage.isDown;
    }
    //ヒット硬直時間が終わったら
    public bool IsEndHitStunCount()
	{
		return hitCount >= hitRigor;
	}
	public bool IsEndHitStun()
	{
		return isEndStun;
	}
	public bool IsPassiveInput()
	{
		if(stateBase.input.atkBotton != "")
		{
			stateBase.input.atkBotton = "";
			return true;
		}
		return false;
	}
}
