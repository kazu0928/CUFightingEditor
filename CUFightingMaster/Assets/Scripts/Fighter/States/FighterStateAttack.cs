using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateAttack : StateBaseScriptMonoBehaviour
{
    private FighterStateBase stateBase;
     private void Start()
    {
        stateBase = GetComponent<FighterStateBase>();
    }
    #region 地上
    public void AttackStart()
    {
        string atk = stateBase.input.GetPlayerAtk();
        switch (atk)
        {
            case "_Atk1":
                if (stateBase.core.PlayerMoveStates == PlayerMoveState.Crouching)
                {
                    stateBase.ChangeSkillConstant(SkillConstants.Crouching_Light_Jab, 0);
                }
                else
                {
                    stateBase.ChangeSkillConstant(SkillConstants.Stand_Light_Jab, 0);

                }
                break;
            case "_Atk2":
                if (stateBase.core.PlayerMoveStates == PlayerMoveState.Crouching)
                {
                    stateBase.ChangeSkillConstant(SkillConstants.Crouching_Middle_Jab, 0);

                }
                else
                {
                    stateBase.ChangeSkillConstant(SkillConstants.Stand_Middle_Jab, 0);
                }
                break;
            case "_Atk3":
                if (stateBase.core.PlayerMoveStates == PlayerMoveState.Crouching)
                {
                    stateBase.ChangeSkillConstant(SkillConstants.Crouching_Strong_Jab, 0);
                }
                else
                {
                    stateBase.ChangeSkillConstant(SkillConstants.Stand_Strong_Jab, 0);
                }
                break;
        }
    }
    public void AttackUpdate()
    {
    }
    //条件
    public bool IsEndAttack()
    {
        return stateBase.core.AnimationPlayerCompornent.EndAnimFrag;
    }
    #endregion

    #region  空中
    public void AirAttackStart()
    {
        string atk = stateBase.input.GetPlayerAtk();
        if (stateBase.core.PlayerMoveStates == PlayerMoveState.Jump)
        {
            switch (atk)
            {
                case "_Atk1":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Light_Jab, 0);
                    break;
                case "_Atk2":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Middle_Jab, 0);
                    break;
                case "_Atk3":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Strong_Jab, 0);
                    break;
            }
        }
        else if (stateBase.core.PlayerMoveStates == PlayerMoveState.Back_Jump)
        {
            switch (atk)
            {
                case "_Atk1":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Back_Light_Jab, 0);
                    break;
                case "_Atk2":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Back_Middle_Jab, 0);
                    break;
                case "_Atk3":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Back_Strong_Jab, 0);
                    break;
            }
        }
        else if (stateBase.core.PlayerMoveStates == PlayerMoveState.Front_Jump)
        {
            switch (atk)
            {
                case "_Atk1":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Front_Light_Jab, 0);
                    break;
                case "_Atk2":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Front_Middle_Jab, 0);
                    break;
                case "_Atk3":
                    stateBase.ChangeSkillConstant(SkillConstants.Air_Front_Strong_Jab, 0);
                    break;
            }
        }
    }
    public bool IsEndAirAttack()
    {
        return stateBase.core.AnimationPlayerCompornent.EndAnimFrag;
    }
    #endregion
}
