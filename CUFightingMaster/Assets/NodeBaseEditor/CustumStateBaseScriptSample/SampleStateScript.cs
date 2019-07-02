using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine;
using CUEngine.Pattern;
using System.Reflection;
using System;

public class SampleStateScript : StateBaseScriptMonoBehaviour
{
    public void StringDebugDraw(string st)
    {
        Debug.Log(st);
    }
    public bool KeyCheck(KeyCode key,string s)
    {
        if(Input.GetKeyDown(key))
        {
            Debug.Log(s);
            return true;
        }
        return false;
    }
}
