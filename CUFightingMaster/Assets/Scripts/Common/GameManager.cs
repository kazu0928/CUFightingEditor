using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;
using CUEngine;

public class GameManager : SingletonMono<GameManager>
{
    [SerializeField]
    private FighterCore Player_one;
    [SerializeField]
    private FighterCore Player_two;
	[SerializeField]
	private TestInput input_one;
	[SerializeField]
	private TestInput input_two;
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
}
