using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateChange : StateBaseScriptMonoBehaviour
{
    public FighterCore fighter;
    public int i = 0;
    
    //スキル入れ替え
    public void ChangeSkill(FighterSkill _change,int _weightFrame)
    {
        fighter.SetSkill(_change,_weightFrame);
    }
    private void Update()
    {
        i++;
    }
    public bool True_Method()
    {
        Debug.Log(i);
        return true;
    }
}
