using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Fighting/ファイター")]
public class FighterStatus : ScriptableObject
{
	[System.Serializable]
	public struct HitBox_
	{
		public Vector3 size;
		public Vector3 localPosition;
	}
	public HitBox_ headHitBox = new HitBox_();
	public HitBox_ bodyHitBox = new HitBox_();
	public HitBox_ footHitBox = new HitBox_();
    public HitBox_ grabHitBox = new HitBox_();
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
