using System.Collections;
using System.Collections.Generic;
using CUEngine.Pattern;
using UnityEngine;

public class FighterInputState : StateBaseScriptMonoBehaviour
{
    private FighterStateChange state = null;
    private TestInput input = null;
    private Direction beforeInput = Direction.Neutral;
    private void Start()
    {
        state = GetComponent<FighterStateChange>();
        switch (state.fighter.PlayerNumber)
        {
            case PlayerNumber.Player1:
                input = InputManager.Instance.testInput[0];
                break;
            case PlayerNumber.Player2:
                input = InputManager.Instance.testInput[1];
                break;
        }
    }
	#region 移動
	//移動の取得
	private Direction GetPlayerMoveDirection()
    {
        switch (input.playerDirection)
        {
            case "1":
                if (state.fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.DownBack;
                }
                else
                {
                    return Direction.DownFront;
                }
            case "2":
                return Direction.Down;
            case "3":
                if (state.fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.DownFront;
                }
                else
                {
                    return Direction.DownBack;
                }
            case "4":
                if (state.fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.Back;
                }
                else
                {
                    return Direction.Front;
                }
            case "5":
                return Direction.Neutral;
            case "6":
                if (state.fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.Front;
                }
                else
                {
                    return Direction.Back;
                }
            case "7":
                if (state.fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.UpBack;
                }
                else
                {
                    return Direction.UpFront;
                }
            case "8":
                return Direction.Up;
            case "9":
                if (state.fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.UpFront;
                }
                else
                {
                    return Direction.UpBack;
                }
        }
        return Direction.Neutral;
    }
	#endregion
	#region 攻撃取得
	private string GetPlayerAtk()
	{
		string s = null;
		if(input.atkBotton == "_Atk1")
		{
			s = "_Atk1";
		}
		return s;
	}
	#endregion
	#region 攻撃系
	public bool Input_Atk(string _atk)
	{
		return _atk == GetPlayerAtk();
	}
	public bool Input_Atk_True()
	{
		return GetPlayerAtk() != null;
	}
	public bool GetEndAnim()
	{
		return state.fighter.AnimationPlayerCompornent.EndAnimFrag;
	}
	#endregion
	//ジャンプ
	public bool Input_Direction(Direction _dir)
    {
        beforeInput = GetPlayerMoveDirection();
        return GetPlayerMoveDirection() == _dir;
    }
    public bool GroundCheck(bool _frag)
    {
        return state.fighter.GroundCheck() == _frag;
    }
    public bool ApplyDamage()
    {
        return state.fighter.GetDamage.frameHitBoxes.Count > 0;
    }
    public bool InputChange()
    {
        return beforeInput != GetPlayerMoveDirection();
    }
}