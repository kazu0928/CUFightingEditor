using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class FighterBase : MonoBehaviour
{
    public AnimationPlayer animationPlayer = null;
	public PlayerMover playerMover = null;
    public AnimationPlayer AnimationPlayerCompornent
    {
        get { return animationPlayer; }
    }
    [SerializeField]
    private FighterStatus status = null;
    [SerializeField]
    private PlayerSkill nowPlaySkill = null;
	public PlayerSkill NowPlaySkill
	{
		get { return nowPlaySkill; }
	}
	private bool changeSkill = false;
	private void Start()
	{
		playerMover = GetComponent<PlayerMover>();
	}
	private void Update()
	{
		//現在の技の取得
		if (nowPlaySkill != ((FighterAnimationPlayer)animationPlayer).NowPlaySkill)
		{
			playerMover.changeSkillFrag = true;
			nowPlaySkill = ((FighterAnimationPlayer)animationPlayer).NowPlaySkill;
		}
		playerMover.UpdateGame();
	}

	#region ギズモ
#if UNITY_EDITOR
	private void OnDrawGizmos()
    {
        if (status == null)
        {
            return;
        }
        #region 未再生
        if (!EditorApplication.isPlaying)
        {
            if (PlayerSkillEditorParameter.instance.window != null)
            {
                if (PlayerSkillEditorParameter.instance.window.previewCharacter == animationPlayer.gameObject)
                {
                    Gizmos.color = Color.green;
                    //代入用
                    Vector3 pos;
                    Vector3 size;
                    //技情報のキャッシュ
                    var skill = PlayerSkillEditorParameter.instance.window.playerSkill;
                    #region Head
                    if (skill.headFrag)
                    {
                        pos = transform.position + status.headHitBox.localPosition;
                        size = status.headHitBox.size;
                        for (int i = 0; i < skill.plusHeadHitBox.Count; i++)
                        {
                            if ((skill.plusHeadHitBox[i].startFrame <= PlayerSkillEditorParameter.instance.window.value) && skill.plusHeadHitBox[i].endFrame >= PlayerSkillEditorParameter.instance.window.value)
                            {
                                pos = transform.position + status.headHitBox.localPosition + skill.plusHeadHitBox[i].hitBox.localPosition;
                                size = status.headHitBox.size + skill.plusHeadHitBox[i].hitBox.size;
                            }
                        }
                        Gizmos.DrawWireCube(pos, size);
                    }
                    #endregion
                    #region Body
                    if (skill.bodyFrag)
                    {
                        pos = transform.position + status.bodyHitBox.localPosition;
                        size = status.bodyHitBox.size;
                        for (int i = 0; i < skill.plusBodyHitBox.Count; i++)
                        {
                            if ((skill.plusBodyHitBox[i].startFrame <= PlayerSkillEditorParameter.instance.window.value) && skill.plusBodyHitBox[i].endFrame >= PlayerSkillEditorParameter.instance.window.value)
                            {
                                pos = transform.position + status.bodyHitBox.localPosition + skill.plusBodyHitBox[i].hitBox.localPosition;
                                size = status.bodyHitBox.size + skill.plusBodyHitBox[i].hitBox.size;
                            }
                        }
                        Gizmos.DrawWireCube(pos, size);
                    }
                    #endregion
                    #region Foot
                    if (skill.footFlag)
                    {
                        pos = transform.position + status.footHitBox.localPosition;
                        size = status.footHitBox.size;
                        for (int i = 0; i < skill.plusFootHitBox.Count; i++)
                        {
                            if ((skill.plusFootHitBox[i].startFrame <= PlayerSkillEditorParameter.instance.window.value) && skill.plusFootHitBox[i].endFrame >= PlayerSkillEditorParameter.instance.window.value)
                            {
                                pos = transform.position + status.footHitBox.localPosition + skill.plusFootHitBox[i].hitBox.localPosition;
                                size = status.footHitBox.size + skill.plusFootHitBox[i].hitBox.size;
                            }
                        }
                        Gizmos.DrawWireCube(pos, size);
                    }
                    #endregion
                    #region Grab
                    if (skill.grabFrag)
                    {
                        Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
                        pos = transform.position + status.grabHitBox.localPosition;
                        size = status.grabHitBox.size;
                        for (int i = 0; i < skill.plusGrabHitBox.Count; i++)
                        {
                            if ((skill.plusGrabHitBox[i].startFrame <= PlayerSkillEditorParameter.instance.window.value) && skill.plusGrabHitBox[i].endFrame >= PlayerSkillEditorParameter.instance.window.value)
                            {
                                pos = transform.position + status.grabHitBox.localPosition + skill.plusGrabHitBox[i].hitBox.localPosition;
                                size = status.grabHitBox.size + skill.plusGrabHitBox[i].hitBox.size;
                            }
                        }
                        Gizmos.DrawCube(pos, size);
                    }
                    #endregion
                    #region Custom
                    for (int i = 0; i < skill.customHitBox.Count; i++)
                    {
                        pos = transform.position;
                        size = Vector3.zero;
                        for (int j = 0; j < skill.customHitBox[i].frameHitBoxes.Count; j++)
                        {
                            if ((skill.customHitBox[i].frameHitBoxes[j].startFrame <= PlayerSkillEditorParameter.instance.window.value) &&
                                (skill.customHitBox[i].frameHitBoxes[j].endFrame >= PlayerSkillEditorParameter.instance.window.value))
                            {
                                pos = transform.position + skill.customHitBox[i].frameHitBoxes[j].hitBox.localPosition;
                                size = skill.customHitBox[i].frameHitBoxes[j].hitBox.size;
                            }
                        }
                        switch (skill.customHitBox[i].mode)
                        {
                            case PlayerSkill.HitBoxMode.HitBox:
                                Gizmos.color = new Color(1, 0, 0, 0.5f);
                                Gizmos.DrawCube(pos, size);
                                break;
                            case PlayerSkill.HitBoxMode.HurtBox:
                                Gizmos.color = Color.green;
                                Gizmos.DrawWireCube(pos, size);
                                break;
                            case PlayerSkill.HitBoxMode.GrabAndSqueeze:
                                Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
                                Gizmos.DrawCube(pos, size);
                                break;
                        }
                    }
                    #endregion
                }
                else
                {
                    DefaultHitBoxDraw();
                }
            }
            else
            {
                DefaultHitBoxDraw();
            }
        }
        #endregion
        #region 再生時
        else
        {
            Gizmos.color = Color.green;
            //代入用
            Vector3 pos;
            Vector3 size;
            if (nowPlaySkill != null)
            {
                #region Head
                if (nowPlaySkill.headFrag)
                {
                    pos = transform.position + status.headHitBox.localPosition;
                    size = status.headHitBox.size;
                    for (int i = 0; i < nowPlaySkill.plusHeadHitBox.Count; i++)
                    {
                        if ((nowPlaySkill.plusHeadHitBox[i].startFrame <= animationPlayer.GetAnimationTime()) && nowPlaySkill.plusHeadHitBox[i].endFrame >= animationPlayer.GetAnimationTime())
                        {
                            pos = transform.position + status.headHitBox.localPosition + nowPlaySkill.plusHeadHitBox[i].hitBox.localPosition;
                            size = status.headHitBox.size + nowPlaySkill.plusHeadHitBox[i].hitBox.size;
                        }
                    }
                    Gizmos.DrawWireCube(pos, size);
                }
                #endregion
                #region Body
                if (nowPlaySkill.bodyFrag)
                {
                    pos = transform.position + status.bodyHitBox.localPosition;
                    size = status.bodyHitBox.size;
                    for (int i = 0; i < nowPlaySkill.plusBodyHitBox.Count; i++)
                    {
                        if ((nowPlaySkill.plusBodyHitBox[i].startFrame <= animationPlayer.GetAnimationTime()) && nowPlaySkill.plusBodyHitBox[i].endFrame >= animationPlayer.GetAnimationTime())
                        {
                            pos = transform.position + status.bodyHitBox.localPosition + nowPlaySkill.plusBodyHitBox[i].hitBox.localPosition;
                            size = status.bodyHitBox.size + nowPlaySkill.plusBodyHitBox[i].hitBox.size;
                        }
                    }
                    Gizmos.DrawWireCube(pos, size);
                }
                #endregion
                #region Foot
                if (nowPlaySkill.footFlag)
                {
                    pos = transform.position + status.footHitBox.localPosition;
                    size = status.footHitBox.size;
                    for (int i = 0; i < nowPlaySkill.plusFootHitBox.Count; i++)
                    {
                        if ((nowPlaySkill.plusFootHitBox[i].startFrame <= animationPlayer.GetAnimationTime()) && nowPlaySkill.plusFootHitBox[i].endFrame >= animationPlayer.GetAnimationTime())
                        {
                            pos = transform.position + status.footHitBox.localPosition + nowPlaySkill.plusFootHitBox[i].hitBox.localPosition;
                            size = status.footHitBox.size + nowPlaySkill.plusFootHitBox[i].hitBox.size;
                        }
                    }
                    Gizmos.DrawWireCube(pos, size);
                }
                #endregion
                #region Grab
                if (nowPlaySkill.grabFrag)
                {
                    Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
                    pos = transform.position + status.grabHitBox.localPosition;
                    size = status.grabHitBox.size;
                    for (int i = 0; i < nowPlaySkill.plusGrabHitBox.Count; i++)
                    {
                        if ((nowPlaySkill.plusGrabHitBox[i].startFrame <= animationPlayer.GetAnimationTime()) && nowPlaySkill.plusGrabHitBox[i].endFrame >= animationPlayer.GetAnimationTime())
                        {
                            pos = transform.position + status.grabHitBox.localPosition + nowPlaySkill.plusGrabHitBox[i].hitBox.localPosition;
                            size = status.grabHitBox.size + nowPlaySkill.plusGrabHitBox[i].hitBox.size;
                        }
                    }
                    Gizmos.DrawCube(pos, size);
                }
                #endregion
                #region Custom
                for (int i = 0; i < nowPlaySkill.customHitBox.Count; i++)
                {
                    pos = transform.position;
                    size = Vector3.zero;
                    for (int j = 0; j < nowPlaySkill.customHitBox[i].frameHitBoxes.Count; j++)
                    {
                        if ((nowPlaySkill.customHitBox[i].frameHitBoxes[j].startFrame <= animationPlayer.GetAnimationTime()) &&
                            (nowPlaySkill.customHitBox[i].frameHitBoxes[j].endFrame >= animationPlayer.GetAnimationTime()))
                        {
                            pos = transform.position + nowPlaySkill.customHitBox[i].frameHitBoxes[j].hitBox.localPosition;
                            size = nowPlaySkill.customHitBox[i].frameHitBoxes[j].hitBox.size;
                        }
                    }
                    switch (nowPlaySkill.customHitBox[i].mode)
                    {
                        case PlayerSkill.HitBoxMode.HitBox:
                            Gizmos.color = new Color(1, 0, 0, 0.5f);
                            Gizmos.DrawCube(pos, size);
                            break;
                        case PlayerSkill.HitBoxMode.HurtBox:
                            Gizmos.color = Color.green;
                            Gizmos.DrawWireCube(pos, size);
                            break;
                        case PlayerSkill.HitBoxMode.GrabAndSqueeze:
                            Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
                            Gizmos.DrawCube(pos, size);
                            break;
                    }
                }
                #endregion
            }
            else
            {
                DefaultHitBoxDraw();
            }
        }

    }
    #endregion
    private void DefaultHitBoxDraw()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + status.headHitBox.localPosition, status.headHitBox.size);
        Gizmos.DrawWireCube(transform.position + status.bodyHitBox.localPosition, status.bodyHitBox.size);
        Gizmos.DrawWireCube(transform.position + status.footHitBox.localPosition, status.footHitBox.size);
        Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
        Gizmos.DrawCube(transform.position + status.grabHitBox.localPosition, status.grabHitBox.size);
    }
#endif
    #endregion
}
