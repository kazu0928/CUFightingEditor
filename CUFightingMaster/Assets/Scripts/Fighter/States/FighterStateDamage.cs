using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateDamage : StateBaseScriptMonoBehaviour
{
	private FighterStateBase stateBase;
	private void Start()
	{
		stateBase = GetComponent<FighterStateBase>();
	}
	//やられ
	public void HitStunStart()
	{
		FighterSkill.CustomHitBox box = stateBase.core.GetDamage;
		if (stateBase.core.GetDamage.frameHitBoxes.Count > 0)
		{
			stateBase.ChangeSkillConstant(SkillConstants.Stand_Light_Middle_HitMotion, 0);
		}
	}
}
