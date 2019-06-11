#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlayerSkillEditor : EditorWindow
{
	public static PlayerSkillEditor window;
	public PlayerSkill playerSkill = null;
	public GameObject previewCharacter;
	public GameObject _beforeCharacter = null;
	private Vector2 scrollPos;

	#region ウィンドウオープン
	public static void Open(PlayerSkill ps)
	{
		if (window == null)
		{
			window = (PlayerSkillEditor)CreateInstance(typeof(PlayerSkillEditor));
			window.playerSkill = ps;
			window.Show();
		}
		else
		{
			window.playerSkill = ps;
		}
	}
	#endregion
	private void OnGUI()
	{
		CustomLabel(playerSkill.name, Color.white, Color.gray, 20, FontStyle.Italic);
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("プレビュー用キャラプレハブ");
		previewCharacter = (EditorGUILayout.ObjectField(previewCharacter, typeof(GameObject), true) as GameObject);
		if (_beforeCharacter != previewCharacter)
		{
			if (previewCharacter != null)
			{
				if (previewCharacter.GetComponent(typeof(FightingAnimationPlayer)) == null)
				{
					previewCharacter = _beforeCharacter;
				}
			}
			_beforeCharacter = previewCharacter;
		}
		EditorGUILayout.EndVertical();
		BarDraw();
		TabDraw();
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		switch (_tab)
		{
			case Tab.アニメーション:
				AnimationTabDraw();
				break;
			case Tab.当たり判定:
				HitBoxTabDraw();
				break;
		}
		GUILayout.EndScrollView();
		AnimationPlayFrame();
	}

	#region タブ_Styles
	private enum Tab
	{
		アニメーション,
		当たり判定,
	}
	private Tab _tab = Tab.アニメーション;
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
	#region アニメーション_Tab
	private void AnimationTabDraw()
	{
		EditorGUILayout.BeginVertical("Box");
		//アニメーションクリップ
		EditorGUILayout.BeginHorizontal("Box");
		EditorGUILayout.LabelField("AnimationClip");
		playerSkill.animationClip = EditorGUILayout.ObjectField(playerSkill.animationClip, typeof(AnimationClip), true) as AnimationClip;
		EditorGUILayout.EndHorizontal();
		if (playerSkill.animationClip != null)
		{
			//アニメーション速度
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("アニメーション速度：");
			playerSkill.animationSpeed = EditorGUILayout.FloatField(playerSkill.animationSpeed);
			EditorGUILayout.EndVertical();
			//フレーム数
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("フレーム数：");
			if (playerSkill.animationSpeed == 0)
			{
				EditorGUILayout.LabelField("0");
			}
			else
			{
				EditorGUILayout.LabelField(((int)((playerSkill.animationClip.length * playerSkill.animationClip.frameRate) / playerSkill.animationSpeed)).ToString());
			}
			EditorGUILayout.EndHorizontal();
			//ブレンド
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("再生時ブレンド");
			playerSkill.inBlend = EditorGUILayout.Toggle(playerSkill.inBlend);
			EditorGUILayout.LabelField("終了時ブレンド");
			playerSkill.outBlend = EditorGUILayout.Toggle(playerSkill.outBlend);
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
	}
	#endregion
	#region 当たり判定_Tab
	private void HitBoxTabDraw()
	{
		if (playerSkill != null)
		{
			EditorGUILayout.BeginHorizontal();
			playerSkill.headFrag = EditorGUILayout.Toggle(playerSkill.headFrag);
			EditorGUILayout.LabelField("Head");
			playerSkill.bodyFrag = EditorGUILayout.Toggle(playerSkill.bodyFrag);
			EditorGUILayout.LabelField("Body");
			playerSkill.footFlag = EditorGUILayout.Toggle(playerSkill.footFlag);
			EditorGUILayout.LabelField("Foot");
			EditorGUILayout.EndHorizontal();
		}
		if (playerSkill.headFrag) HitBoxSetting(HitBoxPosition.Head);
		if (playerSkill.bodyFrag) HitBoxSetting(HitBoxPosition.Body);
		if (playerSkill.footFlag) HitBoxSetting(HitBoxPosition.Foot);
	}
	private bool headFold;
	private bool bodyFold;
	private bool footFold;
	private void HitBoxSetting(HitBoxPosition? eHitBox = null)
	{
		switch(eHitBox)
		{
			case HitBoxPosition.Head:
				headFold = CustomUI.Foldout("Head", headFold);
				break;
			case HitBoxPosition.Body:
				bodyFold = CustomUI.Foldout("Body", bodyFold);
				break;
			case HitBoxPosition.Foot:
				footFold = CustomUI.Foldout("Foot", footFold);
				break;
		}
	}
	#endregion
	#region バー_BarDraw()
	int value = 0;//現在の位置
    int rightValue = 0;//最大値
    int leftValue = 0;//最小値

    //数値入力とバーの表示
    private void BarDraw()
    {
		if (playerSkill.animationClip != null)
		{
			rightValue = (int)(((int)(playerSkill.animationClip.length * playerSkill.animationClip.frameRate)) / playerSkill.animationSpeed);
		}
        //数値入力とバーの表示
        value = EditorGUILayout.IntField(value);
        value = (int)GUILayout.HorizontalSlider(value, leftValue, rightValue,
                "box", "box", GUILayout.Height(40), GUILayout.ExpandWidth(true));
        //最大値最小値
        if (value > rightValue)
        {
            value = rightValue;
        }
        else if (value < leftValue)
        {
            value = leftValue;
        }
    }
    #endregion
    #region アニメーション再生
    private void AnimationPlayFrame()
    {
        if (!EditorApplication.isPlaying)
        {
            //アニメーションの再生
            if (previewCharacter != null && playerSkill != null && playerSkill.animationClip != null)
            {
                playerSkill.animationClip.SampleAnimation(previewCharacter, ((float)value / playerSkill.animationClip.frameRate) * playerSkill.animationSpeed );
            }
        }
    }
    #endregion
    #region カスタムラベル_CustumLabel()
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
