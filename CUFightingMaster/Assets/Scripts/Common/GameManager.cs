using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;
using CUEngine;

public class GameManager : SingletonMono<GameManager>
{
    public FighterCore Player_one;
    public FighterCore Player_two;
	public TestInput input_one;
	public TestInput input_two;
	void Start()
	{
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}
	private void Update()
	{
		input_one.UpdateGame();
		input_two.UpdateGame();
		UpdateManager.Instance.UpdateGame();
		Player_one.UpdateGame();
		Player_two.UpdateGame();
	}
	public FighterCore GetPlayFighterCore(PlayerNumber _mode)
	{
		switch (_mode)
		{
			case PlayerNumber.Player1:
                return Player_one;
			case PlayerNumber.Player2:
                return Player_two;
        }
        return null;
    }
	public FighterCore GetPlayFighterCore(int _layer)
	{
		if(_layer == LayerMask.NameToLayer(CommonConstants.Layers.Player_One))
		{
            return Player_one;
        }
		else if(_layer == LayerMask.NameToLayer(CommonConstants.Layers.Player_Two))
		{
            return Player_two;
        }
        return null;
    }
}
