using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateBase : StateBaseScriptMonoBehaviour
{
    public FighterCore core;
    public TestInput input = null;
	public FighterStateGuard stateGuard;

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
		stateGuard = GetComponent<FighterStateGuard>();
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
		if(stateGuard.isGuard == true)
		{
			return false;
		}
		return core.GetDamage.frameHitBoxes.Count > 0;
	}

	//スキル入れ替え
	public void ChangeSkillConstant(SkillConstants _constants, int _weightFrame)
    {
        core.SetSkill(core.Status.constantsSkills[(int)_constants], _weightFrame);
    }
    //スキル入れ替え（移動カスタム）
    public void ChangeSkillCustomMoveConstant(SkillConstants _constants, int _weightFrame,List<FighterSkill.Move> _move,List<FighterSkill.GravityMove> _grav,bool _con)
    {
        FighterSkill s = Instantiate(core.Status.constantsSkills[(int)_constants]);
        s.movements = _move;
        s.gravityMoves = _grav;
        s.isContinue = _con;
        core.SetSkill(s, _weightFrame);
    }
}
