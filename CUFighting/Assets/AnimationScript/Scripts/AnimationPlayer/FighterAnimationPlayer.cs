using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAnimationPlayer : AnimationPlayer
{
	//再生中の技
	[SerializeField]
	private PlayerSkill nowPlaySkill = null;
	private PlayerSkill beforeNowPlaySkill = null;
	public PlayerSkill NowPlaySkill
	{
		get { return nowPlaySkill; }
	}
	private new void Start()
	{
		//スキルセット
		if(nowPlaySkill!=null)
		{
			if (nowPlaySkill.animationClip != null)
			{
				SetSkillAnimation(nowPlaySkill);
			}
		}
		base.Start();
	}
	private new void Update()
	{
		//技のセット
		if (nowPlaySkill != beforeNowPlaySkill)
		{
			SetSkillAnimation(nowPlaySkill);
		}
		base.Update();
	}
	/// <summary>
	/// 再生する技の変更
	/// </summary>
	/// <param name="skill"></param>
	/// <param name="speed"></param>
	/// <param name="weightFrame"></param>
	public void SetSkillAnimation(PlayerSkill skill, int weightFrame = 0)
	{
		nowPlaySkill = skill;
		beforeNowPlaySkill = nowPlaySkill;
		SetPlayAnimation(skill.animationClip, skill.animationSpeed, weightFrame);
	}
}
