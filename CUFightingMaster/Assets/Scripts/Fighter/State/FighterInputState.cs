using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterInputState : StateBaseScriptMonoBehaviour
{
    private FighterStateChange state = null;
    private void Start() {
        state = GetComponent<FighterStateChange>();
    }
    //ジャンプ
    public bool Jump_Input()
    {
        return Input.GetKeyDown(KeyCode.UpArrow);
    }
    public bool Front_Walk()
    {
        return Input.GetKeyDown(KeyCode.RightArrow);
    }
    public bool UpFrontWalk()
    {
        return Input.GetKeyUp(KeyCode.RightArrow);
    }
    public bool GroundCheck()
    {
        return state.fighter.GroundCheck();
    }
}
