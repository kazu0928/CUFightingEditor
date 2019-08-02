using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

public class FighterStateChange : StateBaseScriptMonoBehaviour
{
    public FighterCore fighter;
    
    //スキル入れ替え
    public void ChangeSkill(FighterSkill _change,int _weightFrame)
    {
        fighter.SetSkill(_change,_weightFrame);
    }
    //スキル入れ替え
    public void ChangeSkillConstant(SkillConstants _constants, int _weightFrame)
    {
        fighter.SetSkill(fighter.Status.constantsSkills[(int)_constants], _weightFrame);
    }
    public void Damage(FighterSkill _change,int _weightFrame)
    {
        fighter.SetDamage(new FighterSkill.CustomHitBox(),null);
        fighter.SetSkill(_change, _weightFrame);
    }
	public void ChangeSkillJump(SkillConstants _constants, int _weightFrame)
	{
		fighter.SetIsGround(false);
		fighter.SetSkill(fighter.Status.constantsSkills[(int)_constants], _weightFrame);
	}
	public bool True_Method()
    {
        return true;
    }
}
