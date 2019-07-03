using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField]
    private FighterCore Player_one;
    [SerializeField]
    private FighterCore Player_two;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
