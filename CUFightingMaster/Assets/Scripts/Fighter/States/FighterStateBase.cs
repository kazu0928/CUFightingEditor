using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateBase : StateBaseScriptMonoBehaviour
{
    public FighterCore core;
    public TestInput input = null;
    #region 初期化
    private void Start()
    {
        switch (core.PlayerNumber)
        {
            case PlayerNumber.Player1:
                input = InputManager.Instance.testInput[0];
                break;
            case PlayerNumber.Player2:
                input = InputManager.Instance.testInput[1];
                break;
        }
    }
    #endregion


    /*　汎用条件式　*/
    //ゲーム開始する条件
    public bool IsStartGame()
    {
        //現在条件なし
        return true;
    }
    public bool IsMoveFighter()
    {
        return true;
    }
    public bool IsGroundCheck(bool ground)
    {
        return core.GroundCheck() == ground;
    }
	//ダメージ受けたとき
	public bool IsApplyDamage()
	{
		return core.GetDamage.frameHitBoxes.Count > 0;
	}

	//スキル入れ替え
	public void ChangeSkillConstant(SkillConstants _constants, int _weightFrame)
    {
        core.SetSkill(core.Status.constantsSkills[(int)_constants], _weightFrame);
    }
}
