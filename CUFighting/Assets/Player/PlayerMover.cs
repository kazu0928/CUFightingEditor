using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
	private FighterBase fb;
	public bool changeSkillFrag = false;
	private int nowPlayMoveNumber = -1;
	private int nowPlayGravityNumber = -1;
	private List<PlayerSkill.Move> moves = new List<PlayerSkill.Move>();
	private List<PlayerSkill.GravityMove> gravity = new List<PlayerSkill.GravityMove>();

	void Start()
	{
		fb = gameObject.GetComponent<FighterBase>();
	}
	public void UpdateGame()
	{
		if (changeSkillFrag)
		{
			if (fb.NowPlaySkill != null)
			{
				moves = new List<PlayerSkill.Move>(fb.NowPlaySkill.movements);
				gravity = new List<PlayerSkill.GravityMove>(fb.NowPlaySkill.gravityMove);
				if (moves.Count != 0)
				{
					moves.Sort((a, b) => a.startFrame - b.startFrame);
				}
				if (gravity.Count != 0)
				{
					gravity.Sort((a, b) => a.startFrame - b.startFrame);
				}
				nowPlayMoveNumber = -1;
			}
			else
			{
				moves = null;
				gravity = null;
			}
			changeSkillFrag = false;
		}
		MovementSkill();
		//重力(制動力)
		GravityMovementSkill();
	}
	private void MovementSkill()
	{
		if (moves == null)
		{
			return;
		}
		if (moves.Count == 0)
		{
			return;
		}
		if (moves.Count > nowPlayMoveNumber + 1)
		{
			if (moves[nowPlayMoveNumber + 1].startFrame <= fb.animationPlayer.GetAnimationTime())
			{
				nowPlayMoveNumber++;
			}
		}
		//移動
		if (nowPlayMoveNumber < 0)
		{
			return;
		}
		transform.Translate(moves[nowPlayMoveNumber].movement);
	}
	private void GravityMovementSkill()
	{
		if (gravity == null)
		{
			return;
		}
		if (gravity.Count == 0)
		{
			return;
		}
		if (gravity.Count > nowPlayGravityNumber + 1)
		{
			if (gravity[nowPlayGravityNumber + 1].startFrame <= fb.animationPlayer.GetAnimationTime())
			{
				nowPlayGravityNumber++;
			}
		}
		//移動
		if (nowPlayGravityNumber < 0)
		{
			return;
		}
		transform.Translate(gravity[nowPlayGravityNumber].movement * fb.animationPlayer.GetAnimationTime() * 0.1f);
	}
}
