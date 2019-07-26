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
		stateBase.core.SetKnockBack(box.knockBack,stateBase.core.EnemyNumber);
		stateBase.core.SetDamage(new FighterSkill.CustomHitBox());
	}
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
		stateBase.core.SetDamage(new FighterSkill.CustomHitBox());
	}

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
		if (hitCount >= hitRigor)
		{
			isEndStun = true;
		}
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
