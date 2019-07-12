using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateUpdate : StateBaseScriptMonoBehaviour
{
    public FighterCore fighter;
    private TestInput input = null;
    private Direction beforeInput = Direction.Neutral;
    private void Start()
    {
        //TODO::技のコマンドをDictionaryに
        switch (fighter.PlayerNumber)
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
                if (fighter.Direction == PlayerDirection.Right)
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
                if (fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.DownFront;
                }
                else
                {
                    return Direction.DownBack;
                }
            case "4":
                if (fighter.Direction == PlayerDirection.Right)
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
                if (fighter.Direction == PlayerDirection.Right)
                {
                    return Direction.Front;
                }
                else
                {
                    return Direction.Back;
                }
            case "7":
                if (fighter.Direction == PlayerDirection.Right)
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
                if (fighter.Direction == PlayerDirection.Right)
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
    public Direction Input_Direction()
    {
        beforeInput = GetPlayerMoveDirection();
        return beforeInput;
    }
    //スキル入れ替え
    public void ChangeSkillConstant(SkillConstants _constants, int _weightFrame)
    {
        fighter.SetSkill(fighter.Status.constantsSkills[(int)_constants], _weightFrame);
    }
    //スキル入れ替え
    public void ChangeSkill(FighterSkill _change, int _weightFrame)
    {
        fighter.SetSkill(_change, _weightFrame);
    }
    public bool True_Method()
    {
        return true;
    }
    public void MoveStart()
    {
        ChangeSkill(fighter.Status.constantsSkills[CommonConstants.Skills.Idle], 5);
    }
    //移動系
    public void MoveUpdate()
    {
        var dir = GetPlayerMoveDirection();
        if (beforeInput != dir)
        {
            beforeInput = dir;
        }
        else
        {
            return;
        }
        switch (GetPlayerMoveDirection())
        {
            case Direction.Neutral:
                ChangeSkill(fighter.Status.constantsSkills[CommonConstants.Skills.Idle], 5);
                break;
            case Direction.Up:
                ChangeSkill(fighter.Status.constantsSkills[CommonConstants.Skills.Jump], 0);
                break;
            case Direction.UpFront:
                ChangeSkill(fighter.Status.constantsSkills[CommonConstants.Skills.Jump], 0);
                break;
            case Direction.UpBack:
                ChangeSkill(fighter.Status.constantsSkills[CommonConstants.Skills.Jump], 0);
                break;
            case Direction.Down:
                ChangeSkill(fighter.Status.constantsSkills[CommonConstants.Skills.Jump], 5);
                break;
        }
    }
}
