using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FighterCreateEditor : EditorWindow
{
	#region window
	public static FighterCreateEditor window;
	[MenuItem("SuperCU/キャラ作成")]
	public static void Open()
	{
		if(window == null)
		{
			window = (FighterCreateEditor)CreateInstance<FighterCreateEditor>();
			window.Show();
		}
	}
	#endregion
	private GameObject modelObj; //モデル（ゲームオブジェクト）
	private void OnGUI()
	{
	}
}
