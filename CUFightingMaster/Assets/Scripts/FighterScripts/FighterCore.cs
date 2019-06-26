using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterCore : MonoBehaviour
{
	[SerializeField] private GameObject playerModel = null;
	[SerializeField] private PlayerNumber playerNumber; //プレイヤー番号
	private FightingAnimationPlayer animationPlayer;//アニメーション再生クラス

	private void Start()
	{
		if (InitErrorCheck())
		{
			animationPlayer = playerModel.GetComponent<FightingAnimationPlayer>();
		}
	}
	private void Update()
	{
		animationPlayer.UpdateAnimation();
	}
	private bool InitErrorCheck()
	{
		if (playerModel == null)
		{
			Debug.LogError("PlayerModelがありません");
			return false;
		}
		else
		{
			if (playerModel.GetComponent(typeof(FightingAnimationPlayer)) == null)
			{
				Debug.LogError("FightingAnimationPlayerがついていません");
				return false;
			}
		}
		return true;
	}
}
