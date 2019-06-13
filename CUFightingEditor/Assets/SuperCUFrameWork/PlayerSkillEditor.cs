#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlayerSkillEditorParameter : ScriptableSingleton<PlayerSkillEditorParameter>
{
	public PlayerSkillEditor window;
}

public class PlayerSkillEditor : EditorWindow
{
	public PlayerSkill playerSkill = null;
	public GameObject previewCharacter;
	public GameObject _beforeCharacter = null;
	private Vector2 scrollPos;

	#region ウィンドウオープン
	public static void Open(PlayerSkill ps)
	{
		if (PlayerSkillEditorParameter.instance.window == null)
		{
			PlayerSkillEditorParameter.instance.window = (PlayerSkillEditor)CreateInstance(typeof(PlayerSkillEditor));
			PlayerSkillEditorParameter.instance.window.playerSkill = ps;
			PlayerSkillEditorParameter.instance.window.Show();
		}
		else
		{
			PlayerSkillEditorParameter.instance.window.playerSkill = ps;
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
				if (previewCharacter.GetComponent(typeof(FighterBase)) != null)
				{
					previewCharacter = previewCharacter.GetComponent<FighterBase>().AnimationPlayerCompornent.gameObject;
				}
				if (previewCharacter.GetComponent(typeof(AnimationPlayer)) == null)
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
			playerSkill.inBlend = EditorGUILayout.Toggle("再生時ブレンド",playerSkill.inBlend);
			playerSkill.outBlend = EditorGUILayout.Toggle("終了時ブレンド",playerSkill.outBlend);
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
			playerSkill.headFrag = EditorGUILayout.Toggle("Head",playerSkill.headFrag);
			playerSkill.bodyFrag = EditorGUILayout.Toggle("Body",playerSkill.bodyFrag);
			playerSkill.footFlag = EditorGUILayout.Toggle("Foot",playerSkill.footFlag);
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
		EditorGUILayout.BeginVertical("Box");
		switch (eHitBox)
		{
			case HitBoxPosition.Head:
				FoldOutHitBox(playerSkill.plusHeadHitBox);
				break;
			case HitBoxPosition.Body:
				bodyFold = CustomUI.Foldout("Body_Default", bodyFold);
				break;
			case HitBoxPosition.Foot:
				footFold = CustomUI.Foldout("Foot_Default", footFold);
				break;
		}
		EditorGUILayout.EndVertical();
	}
	private void FoldOutHitBox(List<PlayerSkill.FrameHitBox> frameHitBox)
	{
		if (headFold = CustomUI.Foldout("Head_Default", headFold))
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(frameHitBox.Count.ToString());
			if (GUILayout.Button("判定作成"))
			{
				frameHitBox.Add(new PlayerSkill.FrameHitBox());
			}
			EditorGUILayout.EndHorizontal();
			//スライダーその他の作成
			for(int i = 0; i<frameHitBox.Count; i++)
			{
				EditorGUILayout.BeginVertical("Box");
				//数値の入れ替え
				float frameStart = frameHitBox[i].startFrame;
				float frameEnd = frameHitBox[i].endFrame;

				EditorGUILayout.BeginHorizontal();
				//左
				frameStart = (int)EditorGUILayout.FloatField(frameStart,GUILayout.Width(30));
				if (frameStart > frameEnd) frameStart = frameEnd - 1;
				if (frameStart < 0) frameStart = 0;
				//スライダー
				EditorGUILayout.MinMaxSlider(ref frameStart, ref frameEnd, 0, rightValue);
				//右
				frameEnd = (int)EditorGUILayout.FloatField(frameEnd,GUILayout.Width(30));
				if (frameEnd < frameStart) frameEnd = frameEnd - 1;
				if (frameEnd > rightValue) frameEnd = rightValue;
				EditorGUILayout.EndHorizontal();

				frameHitBox[i].startFrame = (int)frameStart;
				frameHitBox[i].endFrame = (int)frameEnd;

				//当たり判定の設定
				HitFoldOut(frameHitBox[i]);

				EditorGUILayout.EndVertical();
			}
		}
	}
	private void HitFoldOut(PlayerSkill.FrameHitBox hitBox_)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("Position");
		//Undoに対応
		Undo.RecordObject(playerSkill, "fighterStatus");
		hitBox_.hitBox.localPosition.x = EditorGUILayout.FloatField("X", hitBox_.hitBox.localPosition.x);
		hitBox_.hitBox.localPosition.y = EditorGUILayout.FloatField("Y", hitBox_.hitBox.localPosition.y);
		hitBox_.hitBox.localPosition.z = EditorGUILayout.FloatField("Z", hitBox_.hitBox.localPosition.z);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("サイズ");
		hitBox_.hitBox.size.x = EditorGUILayout.FloatField("X", hitBox_.hitBox.size.x);
		hitBox_.hitBox.size.y = EditorGUILayout.FloatField("Y", hitBox_.hitBox.size.y);
		hitBox_.hitBox.size.z = EditorGUILayout.FloatField("Z", hitBox_.hitBox.size.z);
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

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
        GUILayout.Label(text, guiStyle); //labelFieldだとうまくいかない

        GUI.backgroundColor = beforeBackColor;
    }
    #endregion

}
#endif
