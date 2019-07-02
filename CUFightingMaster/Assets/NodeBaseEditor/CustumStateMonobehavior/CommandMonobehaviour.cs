using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class CommandMonobehaviour : StateMonobehavior
{
	[SerializeField]
	private int graceFrame = 0;
	private int frame;

	protected override void Start()
	{
		base.Start();
		frame = graceFrame;
	}

	public override void UpdateGame()
	{
		base.UpdateGame();
		if (nowPlayStateBody.stateMove)
		{
			//Debug.Log(stateProcessor.State.getStateName());
			frame = graceFrame;
		}
		if (nowPlayStateBody.stateProcessor.State != nowPlayStateBody.states[0])
		{
			frame--;
		}
		if (frame <= 0)
		{
			nowPlayStateBody.stateProcessor.State = nowPlayStateBody.states[0];
		}
	}
}
