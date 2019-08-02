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
        beforeInput = stateBase.input.GetPlayerMoveDirection(stateBase);
        ChangeMove(beforeInput);
    }
    /* ステート中 */
    public void MoveUpdate()
    {
		if(stateBase.core.Direction == PlayerDirection.Right)
		{
			if(stateBase.core.PlayerNumber==PlayerNumber.Player1)
			{
                Transform t = stateBase.core.AnimationPlayerCompornent.gameObject.transform;
                if (GameManager.Instance.GetPlayFighterCore(PlayerNumber.Player2).gameObject.transform.position.x < stateBase.core.transform.position.x)
				{
					stateBase.core.SetDirection(PlayerDirection.Left);
                    t.localScale = new Vector3(1, 1, -1);
                    t.rotation = Quaternion.Euler(0, 0, 0);
                }
			}
            else if(stateBase.core.PlayerNumber==PlayerNumber.Player2)
            {
                if(GameManager.Instance.GetPlayFighterCore(PlayerNumber.Player1).gameObject.transform.position.x < stateBase.core.transform.position.x)
                {
                    Transform t = stateBase.core.AnimationPlayerCompornent.gameObject.transform;
                    stateBase.core.SetDirection(PlayerDirection.Left);
                    stateBase.core.AnimationPlayerCompornent.gameObject.transform.localScale = new Vector3(1, 1, -1);
                    t.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
		}
        else if (stateBase.core.Direction == PlayerDirection.Left)
        {
            if (stateBase.core.PlayerNumber == PlayerNumber.Player1)
            {
                Transform t = stateBase.core.AnimationPlayerCompornent.gameObject.transform;
                if (GameManager.Instance.GetPlayFighterCore(PlayerNumber.Player2).gameObject.transform.position.x > stateBase.core.transform.position.x)
                {
                    stateBase.core.SetDirection(PlayerDirection.Right);
                    stateBase.core.AnimationPlayerCompornent.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    t.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else if (stateBase.core.PlayerNumber == PlayerNumber.Player2)
            {
                Transform t = stateBase.core.AnimationPlayerCompornent.gameObject.transform;
                if (GameManager.Instance.GetPlayFighterCore(PlayerNumber.Player1).gameObject.transform.position.x > stateBase.core.transform.position.x)
                {
                    stateBase.core.SetDirection(PlayerDirection.Right);
                    stateBase.core.AnimationPlayerCompornent.gameObject.transform.localScale = new Vector3(1, 1, 1);
                    t.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
        Direction inp = stateBase.input.GetPlayerMoveDirection(stateBase);
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
                stateBase.core.SetIsCrouching(false);
                stateBase.ChangeSkillConstant(SkillConstants.Idle, 10);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Idle);
                break;
            case Direction.Front:
                stateBase.core.SetIsCrouching(false);
                stateBase.ChangeSkillConstant(SkillConstants.Front_Walk, 10);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Front_Walk);
                break;
            case Direction.Back:
                stateBase.core.SetIsCrouching(false);
                stateBase.ChangeSkillConstant(SkillConstants.Back_Walk, 10);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Back_Walk);
                break;
            case Direction.Down:
                stateBase.core.SetIsCrouching(true);
                stateBase.ChangeSkillConstant(SkillConstants.Crouching, 10);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Crouching);
                break;
            case Direction.DownBack:
                stateBase.core.SetIsCrouching(true);
                stateBase.ChangeSkillConstant(SkillConstants.Crouching, 10);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Crouching);

                break;
            case Direction.DownFront:
                stateBase.core.SetIsCrouching(true);
                stateBase.ChangeSkillConstant(SkillConstants.Crouching, 10);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Crouching);

                break;
            case Direction.Up:
                stateBase.core.SetIsCrouching(false);
                stateBase.ChangeSkillConstant(SkillConstants.Jump, 0);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Jump);

                break;
            case Direction.UpFront:
                stateBase.core.SetIsCrouching(false);
                stateBase.ChangeSkillConstant(SkillConstants.Front_Jump, 0);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Front_Jump);

                break;
            case Direction.UpBack:
                stateBase.core.SetIsCrouching(false);
                stateBase.ChangeSkillConstant(SkillConstants.Back_Jump, 0);
                stateBase.core.SetPlayerMoveState(PlayerMoveState.Back_Jump);

                break;
        }

    }
    #endregion
}
