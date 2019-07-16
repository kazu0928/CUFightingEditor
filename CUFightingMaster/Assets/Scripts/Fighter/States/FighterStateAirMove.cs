using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateAirMove : StateBaseScriptMonoBehaviour
{
    private FighterStateBase stateBase;
    private void Start()
    {
        stateBase = GetComponent<FighterStateBase>();
    }
    public void AirMoveStart()
    {

    }
}
