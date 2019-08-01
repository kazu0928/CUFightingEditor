using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateGuard : StateBaseScriptMonoBehaviour
{
    private FighterStateBase stateBase;
    private void Start()
    {
        stateBase = GetComponent<FighterStateBase>();
    }

}
