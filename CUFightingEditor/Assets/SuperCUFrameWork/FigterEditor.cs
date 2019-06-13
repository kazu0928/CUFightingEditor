#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class FighterEditorParameter : ScriptableSingleton<FighterEditorParameter>
{
	public FigterEditor window = null;
}
public class FigterEditor : EditorWindow
{
	public FighterStatus fighterStatus = null;
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
		
	}
}
#endif