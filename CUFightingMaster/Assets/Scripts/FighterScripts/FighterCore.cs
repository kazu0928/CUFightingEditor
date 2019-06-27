using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterCore : MonoBehaviour
{
	[SerializeField] private GameObject playerModel = null;
	public GameObject PlayerModel
    {
        get { return playerModel; }
    }
	[SerializeField] private PlayerNumber playerNumber; //プレイヤー番号
	[SerializeField] private FightingAnimationPlayer animationPlayer;//アニメーション再生クラス
    [SerializeField] private FighterSkill nextAnimation = null;//ここにいれればアニメーションが再生される
    public FightingAnimationPlayer AnimationPlayerCompornent
	{
        get { return animationPlayer; }
    }
	private void Start()
	{
		if (InitErrorCheck())
		{
			animationPlayer = playerModel.GetComponent<FightingAnimationPlayer>();
		}
	}
	private void Update()
	{
		if(nextAnimation!=null)
		{
            animationPlayer.SetSkillAnimation(nextAnimation);
            nextAnimation = null;
        }
		//技が入れ替わってから動作させたいのでanimationのアップデートは後
		animationPlayer.UpdateAnimation();
	}
	#region 初期化時エラーチェック
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
	#endregion
}
