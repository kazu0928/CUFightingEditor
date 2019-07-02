﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class FighterCore : MonoBehaviour
{
	[SerializeField] private GameObject playerModel = null;//プレイヤーモデル
	[SerializeField] private PlayerNumber playerNumber; //プレイヤー番号
	[SerializeField] private FightingAnimationPlayer animationPlayer = null;//アニメーション再生クラス
	[SerializeField] private FighterStatus status = null;
	private FighterMover mover = null;
    private HitBoxJudgement hitJudgement = null;
    [SerializeField] private FighterSkill nextAnimation = null;//ここにいれればアニメーションが再生される
	[SerializeField] private FighterSkill nowPlaySkill = null;
	public bool changeSkill { get; private set; }//技が入れ替わったかどうか
	#region Getter
	public GameObject PlayerModel
	{
		get { return playerModel; }
	}
	public FightingAnimationPlayer AnimationPlayerCompornent
	{
        get { return animationPlayer; }
    }
	public FighterSkill NowPlaySkill
	{
		get { return nowPlaySkill; }
	}
	public PlayerNumber PlayerNumber
	{
        get { return playerNumber; }
    }
	public FighterStatus Status
    {
        get
        {
            return status;
        }
    }
    #endregion
    private void Start()
	{
		//アタッチエラーチェック
		if (InitErrorCheck())
		{
			//アニメーションプレイヤーの取得
			animationPlayer = playerModel.GetComponent<FightingAnimationPlayer>();
			mover = new FighterMover(this);
            hitJudgement = new HitBoxJudgement(this);
        }
	}
	private void Update()
	{
		//技の入れ替え
		if(nextAnimation!=null)
		{
            animationPlayer.SetSkillAnimation(nextAnimation);
            nextAnimation = null;
			changeSkill = true;
        }
		//技が入れ替わってから動作させたいのでanimationのアップデートは後
		animationPlayer.UpdateGame();
		//アニメーションが入れ替わってから入れ替わったかどうかチェック
		CheckNowPlaySkill();
		//移動のアップデート
		mover.UpdateGame();
        //当たり判定のアップデート
        hitJudgement.UpdateGame();
        //終了
        UpdateEnd();
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
		if(status == null)
		{
			Debug.LogError("FighterStatusがついていません");
			return false;
		}
		return true;
	}
	#endregion
	#region 現在のスキルチェック
	private void CheckNowPlaySkill()
	{
		//技の取得
		if(nowPlaySkill != animationPlayer.NowPlaySkill)
		{
			changeSkill = true;
			nowPlaySkill = animationPlayer.NowPlaySkill;
		}
	}
	#endregion
	#region Update終了時処理
	private void UpdateEnd()
	{
		changeSkill = false;
	}
	#endregion
	#region ギズモ
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		//足元
        Gizmos.color = Color.black;
        Vector3 vector3 = new Vector3(1, 1, 1);
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + (vector3.y / 2), transform.position.z), vector3 );
        if (status == null) return;
		#region 未再生
		if (!EditorApplication.isPlaying)
		{
			//エディタが開かれているとき
			if (PlayerSkillEditorParameter.instance.window != null)
			{
				//エディタにアタッチされているGameObjectがこのObjectだったら
				if (PlayerSkillEditorParameter.instance.window.previewCharacter == animationPlayer.gameObject)
				{
					Gizmos.color = Color.green;
					Vector3 pos; Vector3 size;
					//技情報のキャッシュ
					var skill = PlayerSkillEditorParameter.instance.window.playerSkill;
					#region Head
					if (skill.headFlag)
					{
						pos = transform.position + status.headHitBox.localPosition;
						size = status.headHitBox.size;
						//フレームごとの移動
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
					if (skill.bodyFlag)
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
					if (skill.grabFlag)
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
							case HitBoxMode.HitBox:
								Gizmos.color = new Color(1, 0, 0, 0.5f);
								Gizmos.DrawCube(pos, size);
								break;
							case HitBoxMode.HurtBox:
								Gizmos.color = Color.green;
								Gizmos.DrawWireCube(pos, size);
								break;
							case HitBoxMode.GrabAndSqueeze:
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
			Vector3 pos; Vector3 size;
			if(nowPlaySkill != null)
			{
				if(nowPlaySkill.headFlag)
				{
					#region Head
					if (nowPlaySkill.headFlag)
					{
						pos = transform.position + status.headHitBox.localPosition;
						size = status.headHitBox.size;
						for (int i = 0; i < nowPlaySkill.plusHeadHitBox.Count; i++)
						{
							if ((nowPlaySkill.plusHeadHitBox[i].startFrame <= animationPlayer.NowFrame) && nowPlaySkill.plusHeadHitBox[i].endFrame >= animationPlayer.NowFrame)
							{
								pos = transform.position + status.headHitBox.localPosition + nowPlaySkill.plusHeadHitBox[i].hitBox.localPosition;
								size = status.headHitBox.size + nowPlaySkill.plusHeadHitBox[i].hitBox.size;
							}
						}
						Gizmos.DrawWireCube(pos, size);
					}
					#endregion
					#region Body
					if (nowPlaySkill.bodyFlag)
					{
						pos = transform.position + status.bodyHitBox.localPosition;
						size = status.bodyHitBox.size;
						for (int i = 0; i < nowPlaySkill.plusBodyHitBox.Count; i++)
						{
							if ((nowPlaySkill.plusBodyHitBox[i].startFrame <= animationPlayer.NowFrame) && nowPlaySkill.plusBodyHitBox[i].endFrame >= animationPlayer.NowFrame)
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
							if ((nowPlaySkill.plusFootHitBox[i].startFrame <= animationPlayer.NowFrame) && nowPlaySkill.plusFootHitBox[i].endFrame >= animationPlayer.NowFrame)
							{
								pos = transform.position + status.footHitBox.localPosition + nowPlaySkill.plusFootHitBox[i].hitBox.localPosition;
								size = status.footHitBox.size + nowPlaySkill.plusFootHitBox[i].hitBox.size;
							}
						}
						Gizmos.DrawWireCube(pos, size);
					}
					#endregion
					#region Grab
					if (nowPlaySkill.grabFlag)
					{
						Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
						pos = transform.position + status.grabHitBox.localPosition;
						size = status.grabHitBox.size;
						for (int i = 0; i < nowPlaySkill.plusGrabHitBox.Count; i++)
						{
							if ((nowPlaySkill.plusGrabHitBox[i].startFrame <= animationPlayer.NowFrame) && nowPlaySkill.plusGrabHitBox[i].endFrame >= animationPlayer.NowFrame)
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
							if ((nowPlaySkill.customHitBox[i].frameHitBoxes[j].startFrame <= animationPlayer.NowFrame) &&
								(nowPlaySkill.customHitBox[i].frameHitBoxes[j].endFrame >= animationPlayer.NowFrame))
							{
								pos = transform.position + nowPlaySkill.customHitBox[i].frameHitBoxes[j].hitBox.localPosition;
								size = nowPlaySkill.customHitBox[i].frameHitBoxes[j].hitBox.size;
							}
						}
						switch (nowPlaySkill.customHitBox[i].mode)
						{
							case HitBoxMode.HitBox:
								Gizmos.color = new Color(1, 0, 0, 0.5f);
								Gizmos.DrawCube(pos, size);
								break;
							case HitBoxMode.HurtBox:
								Gizmos.color = Color.green;
								Gizmos.DrawWireCube(pos, size);
								break;
							case HitBoxMode.GrabAndSqueeze:
								Gizmos.color = new Color(1, 0.92f, 0.016f, 0.5f);
								Gizmos.DrawCube(pos, size);
								break;
						}
					}
					#endregion
				}
			}
			else
			{
				DefaultHitBoxDraw();
			}
		}
		#endregion
	}
	//デフォルトの当たり判定の表示
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