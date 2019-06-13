#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
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
		CustomLabel(fighterStatus.name, Color.white, Color.gray, 20, FontStyle.Italic);
		TabDraw();
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		switch (_tab)
		{
			case Tab.当たり判定:
				HitBoxTabDraw();
				break;
		}
		GUILayout.EndScrollView();
        //エディタ全体の再描画
        EditorApplication.QueuePlayerLoopUpdate();
	}
	#region 当たり判定_Tab
	private bool headFold = false;
	private bool bodyFold = false;
	private bool footFold = false;
    private bool grabFold = false;
	private void HitBoxTabDraw()
	{
		EditorGUILayout.BeginVertical("Box");
		if(headFold = CustomUI.Foldout("Head", headFold))
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
	#region タブ_Styles
	private enum Tab
	{
		当たり判定,
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