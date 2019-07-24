#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
public class FighterEditorParameter : ScriptableSingleton<FighterEditorParameter>
{
	public FigterEditor window = null;
}
public class FigterEditor : EditorWindow
{
	public FighterStatus fighterStatus = null;
	private Vector2 scrollPos;
	#region ウィンドウオープン
	public static void Open(FighterStatus fs)
	{
		if (FighterEditorParameter.instance.window == null)
		{
			FighterEditorParameter.instance.window = (FigterEditor)CreateInstance(typeof(FigterEditor));
			FighterEditorParameter.instance.window.fighterStatus = fs;
			FighterEditorParameter.instance.window.Show();
		}
		else
		{
			FighterEditorParameter.instance.window.fighterStatus = fs;
		}
	}
	#endregion
	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		CustomLabel(fighterStatus.name, Color.white, Color.gray, 20, FontStyle.Italic);
		if (GUILayout.Button("保存", GUILayout.Height(50), GUILayout.Width(80)))
		{
			//ダーティとしてマークする(変更があった事を記録する)
			EditorUtility.SetDirty(fighterStatus);

			//保存する
			AssetDatabase.SaveAssets();
		}
		EditorGUILayout.EndHorizontal();
		TabDraw();
		scrollPos = GUILayout.BeginScrollView(scrollPos);
        if (fighterStatus.constantsSkills.Length != CommonConstants.Skills.SkillCount)
        {
            Array.Resize(ref fighterStatus.constantsSkills, CommonConstants.Skills.SkillCount);
        }
        switch (_tab)
		{
            case Tab.ステータス:
                StatusTabDraw();
                break;
            case Tab.当たり判定:
				HitBoxTabDraw();
                break;
            case Tab.移動アニメーション:
                MoveAnimationTabDraw();
                break;
			case Tab.攻撃:
                AttackTabDraw();
                break;
        }
		GUILayout.EndScrollView();
        //エディタ全体の再描画
        EditorApplication.QueuePlayerLoopUpdate();
	}

    #region  ステータス_Tab
    private void StatusTabDraw()
    {
        EditorGUILayout.BeginVertical("Box");
        fighterStatus.HP = EditorGUILayout.IntField("HP",fighterStatus.HP);
        EditorGUILayout.EndVertical();
    }
    #endregion

    #region 当たり判定_Tab
    private bool headFold = false;
	private bool bodyFold = false;
	private bool footFold = false;
    private bool grabFold = false;
	private bool pushingFold = false;
	private void HitBoxTabDraw()
	{
		EditorGUILayout.BeginVertical("Box");
		if (headFold = CustomUI.Foldout("Head", headFold))
		{
			HitFoldOut(ref fighterStatus.headHitBox);
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical("Box");
		if (bodyFold = CustomUI.Foldout("Body", bodyFold))
		{
			HitFoldOut(ref fighterStatus.bodyHitBox);
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical("Box");
		if (footFold = CustomUI.Foldout("Foot", footFold))
		{
			HitFoldOut(ref fighterStatus.footHitBox);
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical("Box");
		if (grabFold = CustomUI.Foldout("Grab", grabFold))
		{
			HitFoldOut(ref fighterStatus.grabHitBox);
		}
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical("Box");
		if (pushingFold = CustomUI.Foldout("Pushing", pushingFold))
		{
			HitFoldOut(ref fighterStatus.pushingHitBox);
		}
		EditorGUILayout.EndVertical();

	}
    private void HitFoldOut(ref FighterStatus.HitBox_ hitBox_)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("Position");
		//Undoに対応
		Undo.RecordObject(fighterStatus, "fighterStatus");
		hitBox_.localPosition.x = EditorGUILayout.FloatField("X", hitBox_.localPosition.x);
		hitBox_.localPosition.y = EditorGUILayout.FloatField("Y", hitBox_.localPosition.y);
		hitBox_.localPosition.z = EditorGUILayout.FloatField("Z", hitBox_.localPosition.z);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("サイズ");
		hitBox_.size.x = EditorGUILayout.FloatField("X", hitBox_.size.x);
		hitBox_.size.y = EditorGUILayout.FloatField("Y", hitBox_.size.y);
		hitBox_.size.z = EditorGUILayout.FloatField("Z", hitBox_.size.z);
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

	}
    #endregion

    #region アニメーション_Tab
    private bool move = false;
    private bool standHitDamage = false;
    private bool crouchingHitDamage = false;
    private bool airHitDamage = false;
    private bool down = false;
    private void MoveAnimationTabDraw()
	{
		if(move = CustomUI.Foldout("移動系", move))
		{
			fighterStatus.constantsSkills[CommonConstants.Skills.Idle] = (FighterSkill)EditorGUILayout.ObjectField("待機",fighterStatus.constantsSkills[CommonConstants.Skills.Idle],typeof(FighterSkill),false);
            fighterStatus.constantsSkills[CommonConstants.Skills.Front_Walk] = (FighterSkill)EditorGUILayout.ObjectField("前歩き", fighterStatus.constantsSkills[CommonConstants.Skills.Front_Walk], typeof(FighterSkill), false);
            fighterStatus.constantsSkills[CommonConstants.Skills.Back_Walk] = (FighterSkill)EditorGUILayout.ObjectField("後歩き", fighterStatus.constantsSkills[CommonConstants.Skills.Back_Walk], typeof(FighterSkill), false);
            fighterStatus.constantsSkills[CommonConstants.Skills.Crouching] = (FighterSkill)EditorGUILayout.ObjectField("しゃがみ", fighterStatus.constantsSkills[CommonConstants.Skills.Crouching], typeof(FighterSkill), false);
            fighterStatus.constantsSkills[CommonConstants.Skills.Jump] = (FighterSkill)EditorGUILayout.ObjectField("ジャンプ", fighterStatus.constantsSkills[CommonConstants.Skills.Jump], typeof(FighterSkill), false);
            fighterStatus.constantsSkills[CommonConstants.Skills.Front_Jump] = (FighterSkill)EditorGUILayout.ObjectField("前ジャンプ", fighterStatus.constantsSkills[CommonConstants.Skills.Front_Jump], typeof(FighterSkill), false);
            fighterStatus.constantsSkills[CommonConstants.Skills.Back_Jump] = (FighterSkill)EditorGUILayout.ObjectField("後ジャンプ", fighterStatus.constantsSkills[CommonConstants.Skills.Back_Jump], typeof(FighterSkill), false);
			fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Guard] = (FighterSkill)EditorGUILayout.ObjectField("立ちガード", fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Guard], typeof(FighterSkill), false);
			fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Guard] = (FighterSkill)EditorGUILayout.ObjectField("しゃがみガード", fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Guard], typeof(FighterSkill), false);

		}
		if (standHitDamage = CustomUI.Foldout("立ちやられ",standHitDamage))
		{
            SSet("上弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Light_Top_HitMotion]);
            SSet("上中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Middle_Top_HitMotion]);
            SSet("上強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Strong_Top_HitMotion]);
            SSet("中弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Light_Middle_HitMotion]);
            SSet("中中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Middle_Middle_HitMotion]);
            SSet("中強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Strong_Middle_HitMotion]);
            SSet("下弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Light_Bottom_HitMotion]);
            SSet("下中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Middle_Bottom_HitMotion]);
            SSet("下強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Strong_Bottom_HitMotion]);
        }
        if (crouchingHitDamage = CustomUI.Foldout("しゃがみやられ", crouchingHitDamage))
        {
            SSet("上弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Light_Top_HitMotion]);
            SSet("上中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Middle_Top_HitMotion]);
            SSet("上強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Strong_Top_HitMotion]);
            SSet("中弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Light_Middle_HitMotion]);
            SSet("中中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Middle_Middle_HitMotion]);
            SSet("中強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Strong_Middle_HitMotion]);
            SSet("下弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Light_Bottom_HitMotion]);
            SSet("下中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Middle_Bottom_HitMotion]);
            SSet("下強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Strong_Bottom_HitMotion]);
        }
		if (airHitDamage = CustomUI.Foldout("空中やられ", airHitDamage))
        {
            SSet("上弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Light_Top_HitMotion]);
            SSet("上中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Middle_Top_HitMotion]);
            SSet("上強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Strong_Top_HitMotion]);
            SSet("中弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Light_Middle_HitMotion]);
            SSet("中中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Middle_Middle_HitMotion]);
            SSet("中強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Strong_Middle_HitMotion]);
            SSet("下弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Light_Bottom_HitMotion]);
            SSet("下中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Middle_Bottom_HitMotion]);
            SSet("下強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Strong_Bottom_HitMotion]);
        }
        if (down = CustomUI.Foldout("ダウン", down))
        {
            SSet("ダウン", ref fighterStatus.constantsSkills[CommonConstants.Skills.Down]);
            SSet("起き上がり", ref fighterStatus.constantsSkills[CommonConstants.Skills.Wake_Up]);
			SSet("空中受け身", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Passive]);
			SSet("空中前受け身", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Front_Passive]);
			SSet("空中後ろ受け身", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Back_Passive]);
		}

	}
	private void SSet(string _s,ref FighterSkill _fs)
	{
		_fs = (FighterSkill)EditorGUILayout.ObjectField(_s,_fs,typeof(FighterSkill),false);
	}
    #endregion

    #region 攻撃_Tab
    private bool standJab = false;
    private bool crouchingJab = false;
    private bool airJab = false;
    private bool airFrontJab = false;
    private bool airBackJab = false;
    private bool standKick = false;
    private bool crouchingKick = false;
    private bool airKick = false;
    private bool airFrontKick = false;
    private bool airBackKick = false;
    private bool UniqueSkill = false;
    private bool specialSkill = false;
    private bool CASkill = false;

    private void AttackTabDraw()
	{
		if(standJab = CustomUI.Foldout("立ちP",standJab))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Light_Jab]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Middle_Jab]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Strong_Jab]);
        }
		if(crouchingJab = CustomUI.Foldout("しゃがみP",crouchingJab))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Light_Jab]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Middle_Jab]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Strong_Jab]);
        }
		if(airJab = CustomUI.Foldout("N空中P",airJab))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Light_Jab]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Middle_Jab]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Strong_Jab]);
        }
		if(airFrontJab = CustomUI.Foldout("前空中P",airFrontJab))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Front_Light_Jab]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Front_Middle_Jab]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Front_Strong_Jab]);
		}
        if (airBackJab = CustomUI.Foldout("後空中P", airBackJab))
        {
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Back_Light_Jab]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Back_Middle_Jab]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Back_Strong_Jab]);
        }
		if(standKick = CustomUI.Foldout("立ちK",standKick))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Light_Kick]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Middle_Kick]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Stand_Strong_Kick]);
        }
		if(crouchingKick = CustomUI.Foldout("しゃがみK",crouchingKick))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Light_Kick]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Middle_Kick]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Crouching_Strong_Kick]);
        }
		if(airKick = CustomUI.Foldout("N空中K",airKick))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Light_Kick]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Middle_Kick]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Strong_Kick]);
        }
		if(airFrontKick = CustomUI.Foldout("前空中K",airFrontKick))
		{
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Front_Light_Kick]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Front_Middle_Kick]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Front_Strong_Kick]);
		}
        if (airBackKick = CustomUI.Foldout("後空中K", airBackKick))
        {
            SSet("弱", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Back_Light_Kick]);
            SSet("中", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Back_Middle_Kick]);
            SSet("強", ref fighterStatus.constantsSkills[CommonConstants.Skills.Air_Back_Strong_Kick]);
        }
        if (UniqueSkill = CustomUI.Foldout("特殊技", UniqueSkill))
        {
            EditorGUILayout.BeginVertical("Box");
            if (GUILayout.Button("特殊技作成", GUILayout.Width(80)))
            {
                fighterStatus.uniqueSkills.Add(new FighterStatus.SkillAnimationCustom());
            }
            for (int i = 0; i < fighterStatus.uniqueSkills.Count; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                bool removeFrag = false;
                if (GUILayout.Button("×", GUILayout.Width(20)))
                {
                    removeFrag = true;
                }
                fighterStatus.uniqueSkills[i].Command = EditorGUILayout.TextField("コマンド", fighterStatus.uniqueSkills[i].Command);
                SSet("技", ref fighterStatus.uniqueSkills[i].skill);
                //削除ボタン
                if (removeFrag) fighterStatus.uniqueSkills.RemoveAt(i);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        if (specialSkill = CustomUI.Foldout("必殺技", specialSkill))
        {
            EditorGUILayout.BeginVertical("Box");
            if (GUILayout.Button("必殺技作成", GUILayout.Width(80)))
            {
                fighterStatus.specialSkills.Add(new FighterStatus.SkillAnimationCustom());
            }
            for (int i = 0; i < fighterStatus.specialSkills.Count; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                bool removeFrag = false;
                if (GUILayout.Button("×", GUILayout.Width(20)))
                {
                    removeFrag = true;
                }
                fighterStatus.specialSkills[i].Command = EditorGUILayout.TextField("コマンド", fighterStatus.specialSkills[i].Command);
                SSet("技", ref fighterStatus.specialSkills[i].skill);
                //削除ボタン
                if (removeFrag) fighterStatus.specialSkills.RemoveAt(i);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        if (CASkill = CustomUI.Foldout("超必殺技", CASkill))
        {
            EditorGUILayout.BeginVertical("Box");
            if (GUILayout.Button("超必殺技作成", GUILayout.Width(80)))
            {
                fighterStatus.CASkills.Add(new FighterStatus.SkillAnimationCustom());
            }
            for (int i = 0; i < fighterStatus.CASkills.Count; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                bool removeFrag = false;
                if (GUILayout.Button("×", GUILayout.Width(20)))
                {
                    removeFrag = true;
                }
                fighterStatus.CASkills[i].Command = EditorGUILayout.TextField("コマンド", fighterStatus.CASkills[i].Command);
                SSet("技", ref fighterStatus.CASkills[i].skill);
                //削除ボタン
                if (removeFrag) fighterStatus.CASkills.RemoveAt(i);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
	#endregion

	#region タブ_Styles
	private enum Tab
	{
        ステータス,
		当たり判定,
		移動アニメーション,
		攻撃,
	}
	private Tab _tab = Tab.当たり判定;
	// Style定義
	private static class Styles
	{
		private static GUIContent[] _tabToggles = null;
		public static GUIContent[] TabToggles
		{
			get
			{
				if (_tabToggles == null)
				{
					_tabToggles = System.Enum.GetNames(typeof(Tab)).Select(x => new GUIContent(x)).ToArray();
				}
				return _tabToggles;
			}
		}

		public static readonly GUIStyle TabButtonStyle = "LargeButton";

		// GUI.ToolbarButtonSize.FitToContentsも設定できる
		public static readonly GUI.ToolbarButtonSize TabButtonSize = GUI.ToolbarButtonSize.Fixed;
	}
	private void TabDraw()
	{
		//タブ表示
		using (new EditorGUILayout.HorizontalScope())
		{
			GUILayout.FlexibleSpace();
			// タブを描画する
			_tab = (Tab)GUILayout.Toolbar((int)_tab, Styles.TabToggles, Styles.TabButtonStyle, Styles.TabButtonSize);
			GUILayout.FlexibleSpace();
		}
	}
	#endregion
	#region カスタムラベル_CustomLabel()
	void CustomLabel(string text, Color textColor, Color backColor, int fontSize, FontStyle fontStyle = FontStyle.Bold)
	{
		Color beforeBackColor = GUI.backgroundColor;

		GUIStyle guiStyle = new GUIStyle();
		GUIStyleState styleState = new GUIStyleState();

		styleState.textColor = textColor;
		styleState.background = Texture2D.whiteTexture;
		GUI.backgroundColor = backColor;
		guiStyle.normal = styleState;
		guiStyle.fontSize = fontSize;
		GUILayout.Label(text, guiStyle); //labelFieldだとうまくいかない？

		GUI.backgroundColor = beforeBackColor;
	}
	#endregion
}
#endif