using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateDown : StateBaseScriptMonoBehaviour
{
    private FighterStateBase stateBase;
    private float downCount = 0;
    private float downTime = 1.0f;
    private bool isWakeUp = false;
    private void Start()
    {
        stateBase = GetComponent<FighterStateBase>();
    }
    public void DownStart()
    {
        downCount = 0;
        isWakeUp = false;
        stateBase.ChangeSkillConstant(SkillConstants.Down, 0);
    }
    public void UpdateDown()
    {
        downCount += 1.0f / 60.0f;
        if(downCount >= downTime)
        {
            isWakeUp = true;
        }
    }
    public bool IsWakeUp()
    {
        return isWakeUp;
    }
    //起き上がり
    public void WakeUpStart()
    {
        downCount = 0;
        isWakeUp = false;
        stateBase.ChangeSkillConstant(SkillConstants.Wake_Up, 0);
    }
    public bool IsEndWakeUp()
    {
        return stateBase.core.AnimationPlayerCompornent.EndAnimFrag;
    }
    //打ち付け
    public void GroundKnockStart()
    {
        stateBase.ChangeSkillConstant(SkillConstants.Ground_Knock, 0);
    }
    public bool IsEndGroundKnock()
    {
        return stateBase.core.AnimationPlayerCompornent.EndAnimFrag;
    }
}
