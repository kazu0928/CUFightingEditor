using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class InputManager : SingletonMono<InputManager>
{
    public TestInput[] testInput;
    private new void Awake()
    {
        base.Awake();
        testInput = GetComponents<TestInput>();
    }
}
