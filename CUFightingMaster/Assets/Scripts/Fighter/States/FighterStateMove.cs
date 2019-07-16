//地上移動に関する挙動
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateMove : StateBaseScriptMonoBehaviour
{
    private FighterStateBase stateBase;
    private Direction beforeInput = Direction.Neutral;
    #region 初期化
    private void Start()
    {
        stateBase = GetComponent<FighterStateBase>();
    }
    #endregion

    /* ステートに入った時 */
    public void MoveStart()
    {
        beforeInput = GetPlayerMoveDirection();
        ChangeMove(beforeInput);
    }
    /* ステート中 */
    public void MoveUpdate()
    {
        Direction inp = GetPlayerMoveDirection();
        if (inp == beforeInput) return;
        ChangeMove(inp);
        beforeInput = inp;
    }
    /* 条件式 */
    public bool Input_Atk_True()
    {
        return stateBase.input.GetPlayerAtk() != null;
    }

    #region 取得系
    private void ChangeMove(Direction _dir)
    {
        switch (_dir)
        {
            case Direction.Neutral:
                stateBase.ChangeSkillConstant(SkillConstants.Idle, 5);
                break;
            case Direction.Front:
                stateBase.ChangeSkillConstant(SkillConstants.Front_Walk, 0);
                break;
            case Direction.Back:
                stateBase.ChangeSkillConstant(SkillConstants.Back_Walk, 0);
                break;
            case Direction.Down:
                stateBase.ChangeSkillConstant(SkillConstants.Crouching, 5);
                break;
            case Direction.DownBack:
                stateBase.ChangeSkillConstant(SkillConstants.Crouching, 5);
                break;
            case Direction.DownFront:
                stateBase.ChangeSkillConstant(SkillConstants.Crouching, 5);
                break;
            case Direction.Up:
                stateBase.ChangeSkillConstant(SkillConstants.Jump, 0);
                break;
            case Direction.UpFront:
                stateBase.ChangeSkillConstant(SkillConstants.Front_Jump, 0);
                break;
            case Direction.UpBack:
                stateBase.ChangeSkillConstant(SkillConstants.Back_Jump, 0);
                break;
        }

    }
    //移動の取得
    private Direction GetPlayerMoveDirection()
    {
        switch (stateBase.input.playerDirection)
        {
            case "1":
                if (stateBase.core.Direction == PlayerDirection.Right)
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
                if (stateBase.core.Direction == PlayerDirection.Right)
                {
                    return Direction.DownFront;
                }
                else
                {
                    return Direction.DownBack;
                }
            case "4":
                if (stateBase.core.Direction == PlayerDirection.Right)
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
                if (stateBase.core.Direction == PlayerDirection.Right)
                {
                    return Direction.Front;
                }
                else
                {
                    return Direction.Back;
                }
            case "7":
                if (stateBase.core.Direction == PlayerDirection.Right)
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
                if (stateBase.core.Direction == PlayerDirection.Right)
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
}
