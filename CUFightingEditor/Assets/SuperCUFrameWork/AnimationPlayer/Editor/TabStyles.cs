using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public partial class PlayerAnimationEditor : EditorWindow
{
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
}
