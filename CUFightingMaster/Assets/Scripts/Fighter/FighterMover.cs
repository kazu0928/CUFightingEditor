using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterMover
{
	private FighterCore core;
	private Transform transform;//動かすTransform
	private int nowPlayMoveNumber = -1;		//現在再生中の移動配列の要素数(-1だと動かない)
	private int nowPlayGravityNumber = -1;  //現在再生中の重力配列の要素数(-1だと動かない)
	private List<FighterSkill.Move> moves = new List<FighterSkill.Move>();//移動配列
	private List<FighterSkill.GravityMove> gravity = new List<FighterSkill.GravityMove>();//重力配列
	private int gravityFrame = 0;   //重力用のフレーム数

    //初期化
    public FighterMover(FighterCore fighterCore)
	{
		core = fighterCore;
		transform = fighterCore.transform;
	}
    public void UpdateGame()
    {
		ChangeSkillInit();//技の入れ替え
		MovementSkill();//移動
		GravityMovementSkill();
		gravityFrame++;
    }
	#region 技入れ替えチェック
	//入れ替わり処理
	private void ChangeSkillInit()
	{
		//入れ替わったかどうか
		if (core.changeSkill == false) return;
		if (core.NowPlaySkill != null)
		{
			//重力の継続をするか否か
			if(!core.NowPlaySkill.isContinue)
			{
				gravityFrame = 0;
			}
			moves = new List<FighterSkill.Move>(core.NowPlaySkill.movements);
            if (!core.NowPlaySkill.isContinue)
            {
                gravity = new List<FighterSkill.GravityMove>(core.NowPlaySkill.gravityMoves);
            }
            //移動配列のソート、フレームが近い順に並べる
            if(moves.Count > 1)
			{
				moves.Sort((a, b) => a.startFrame - b.startFrame);
			}
			if(gravity.Count > 1)
			{
				moves.Sort((a, b) => a.startFrame - b.startFrame);
			}
			nowPlayMoveNumber = -1;
			nowPlayGravityNumber = -1;
		}
		//なければなし
		else
		{
			moves = null;
			gravity = null;
			gravityFrame = 0;
		}
	}
    #endregion
    #region 移動
    //移動
    private void MovementSkill()
    {
        if ((moves == null) || (moves.Count == 0)) return;//nullチェック
                                                          //次があるかどうか
        if (moves.Count > nowPlayMoveNumber + 1)
        {
            //現在再生中の移動の次の移動フレームを越えれば
            if (moves[nowPlayMoveNumber + 1].startFrame <= core.AnimationPlayerCompornent.NowFrame)
            {
                nowPlayMoveNumber++;
            }
        }
        //ループ時
        else
        {
            if(moves[0].startFrame<=core.AnimationPlayerCompornent.NowFrame && moves[nowPlayMoveNumber].startFrame > core.AnimationPlayerCompornent.NowFrame)
            {
                nowPlayMoveNumber = 0;
            }
        }
        if (nowPlayMoveNumber < 0) return;//-1なら動かない
        int xDirection = 1;
        if (core.Direction == PlayerDirection.Left) xDirection = -1;
        Vector3 move = moves[nowPlayMoveNumber].movement;
        move.x *= xDirection;
        //移動
        transform.Translate(move * 0.1f);
    }
    //重力移動
    private void GravityMovementSkill()
    {
        if ((gravity == null) || (gravity.Count == 0)) return;
        if (gravity.Count > nowPlayGravityNumber + 1)
        {
            //現在再生中の移動の次の移動フレームを越えれば
            if (gravity[nowPlayGravityNumber + 1].startFrame <= core.AnimationPlayerCompornent.NowFrame)
            {
                nowPlayGravityNumber++;
            }
        }
        if (nowPlayGravityNumber < 0) return;
        //移動保存用
        Vector3 move = gravity[nowPlayGravityNumber].movement * gravityFrame;
        int xDirection = 1;
        if (core.Direction == PlayerDirection.Left) xDirection = -1;
        if (Mathf.Abs(move.x) > Mathf.Abs(gravity[nowPlayGravityNumber].limitMove.x))
        {
            move.x = gravity[nowPlayGravityNumber].limitMove.x;
        }
        if (Mathf.Abs(move.y) > Mathf.Abs(gravity[nowPlayGravityNumber].limitMove.y))
        {
            move.y = gravity[nowPlayGravityNumber].limitMove.y;
        }
        move.x *= xDirection;
        transform.Translate(move * 0.1f);
    }
    #endregion
}
