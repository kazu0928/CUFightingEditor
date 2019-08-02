using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateGuard : StateBaseScriptMonoBehaviour
{
    private FighterStateBase stateBase;
	public bool isGuard = false;

	private int hitRigor = 0;
	private int hitCount = 0;

	private bool isGuardEnd = false;

	private void Start()
    {
        stateBase = GetComponent<FighterStateBase>();
    }
	//ガードになる条件
	public bool IsApplyGuard()
	{
		if (stateBase.core.GetDamage.frameHitBoxes.Count > 0)
		{
			Direction inp = stateBase.input.GetPlayerMoveDirection(stateBase);
			Debug.Log(inp);

			if (stateBase.core.GetDamage.hitPoint == HitPoint.Top || stateBase.core.GetDamage.hitPoint == HitPoint.Middle)
			{
				if (inp == Direction.Back)
				{
					isGuard = true;
				}
			}
			if(stateBase.core.GetDamage.hitPoint == HitPoint.Bottom|| stateBase.core.GetDamage.hitPoint == HitPoint.Top)
			{
				if (inp == Direction.DownBack)
				{
					isGuard = true;
				}
			}
		}
		return isGuard;
	}
	//ガード開始
	public void GuardStart()
	{
		isGuard = false;
		isGuardEnd = false;
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

		Direction inp = stateBase.input.GetPlayerMoveDirection(stateBase);
		if (inp == Direction.Back)
		{
			stateBase.ChangeSkillConstant(SkillConstants.Stand_Guard, 0);
		}
		else if (inp == Direction.DownBack)
		{
			stateBase.ChangeSkillConstant(SkillConstants.Crouching_Guard, 0);
		}
		//ノックバックのセット
		PlayerDirection tmpDir;
		//右向きにノックバックするか左向きにノックバックするか
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
			if (box.hitEffects[i].guardEffect != null)
			{
				if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Right)
				{
					Object.Instantiate(box.hitEffects[i].guardEffect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.identity);
				}
				else if (GameManager.Instance.GetPlayFighterCore(stateBase.core.EnemyNumber).Direction == PlayerDirection.Left)
				{
					Object.Instantiate(box.hitEffects[i].guardEffect, new Vector3(t.position.x + _bCol.center.x + box.hitEffects[i].position.x, t.position.y + _bCol.center.y + box.hitEffects[i].position.y, t.position.z + _bCol.center.z + box.hitEffects[i].position.z), Quaternion.Euler(0, 180, 0));
				}
			}
		}

		stateBase.core.SetKnockBack(box.guardKnockBack, stateBase.core.EnemyNumber, tmpDir);
		//ダメージを受けたのでリセット
		stateBase.core.SetDamage(new FighterSkill.CustomHitBox(),null);
	}
	//ヒット硬直時間をプラス
	public void GuardUpdate()
	{
		if (GameManager.Instance.GetHitStop(stateBase.core.PlayerNumber) <= 0)
		{
			hitCount++;
		}
	}

	//ヒット硬直時間が終わったら
	public bool IsEndGuardCount()
	{
		return hitCount >= hitRigor;
	}

	//ガードから抜ける
	public bool IsEndGuard()
	{
		return isGuardEnd;
	}
	//ステートエンド
	public void EndGuardFlag()
	{
		isGuardEnd = true;
	}

}
