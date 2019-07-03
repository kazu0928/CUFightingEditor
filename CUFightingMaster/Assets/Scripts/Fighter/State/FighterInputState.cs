using System.Collections;
using System.Collections.Generic;
using CUEngine.Pattern;
using UnityEngine;

public class FighterInputState : StateBaseScriptMonoBehaviour
{
    private FighterStateChange state = null;
    private TestInput input = null;
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
    //ジャンプ
    public bool Input_Direction(Direction _dir)
    {
        return GetPlayerMoveDirection() == _dir;
    }
    public bool GroundCheck(bool _frag)
    {
        return state.fighter.GroundCheck() == _frag;
    }
}