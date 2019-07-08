﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Fighting/ファイター")]
public class FighterStatus : ScriptableObject
{
	[System.Serializable]
	public class HitBox_
	{
		public Vector3 size;
		public Vector3 localPosition;
	}

	public class SkillAnimationCustom
	{
		public string Name;
		public string Command;
		public FighterSkill skill;
	}
	public class SkillAnimation
	{
		public PlayerSkillStatus status;
		public FighterSkill skill;
	}

    //当たり判定
    public HitBox_ headHitBox = new HitBox_();
	public HitBox_ bodyHitBox = new HitBox_();
	public HitBox_ footHitBox = new HitBox_();
    public HitBox_ grabHitBox = new HitBox_();

	public List<SkillAnimation> skills;

	#region EDITOR_
#if UNITY_EDITOR
	[CustomEditor(typeof(FighterStatus))]
	public class FigterStatusInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("ファイター設定画面を開く"))
			{
				FigterEditor.Open((FighterStatus)target);
			}
		}
	}
#endif
	#endregion
}
