using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(menuName = "Fighting/キャラクター")]
public class PlayerSkill : ScriptableObject
{
	[System.Serializable]
	public struct HitBox_
	{
		public Vector3 size;
		public Vector3 localPosition;
	}
	[System.Serializable]
	public class FrameHitBox
	{
		public HitBox_ hitBox;
		public int startFrame;
		public int endFrame;
	}
	public AnimationClip animationClip = null;
    public float animationSpeed = 1;
    //ブレンドフラグ
    public bool inBlend = false;
    public bool outBlend = false;

	//当たり判定
	public List<FrameHitBox> plusHeadHitBox = new List<FrameHitBox>();
	public List<FrameHitBox> plusBodyHitBox = new List<FrameHitBox>();
	public List<FrameHitBox> plusFootHitBox = new List<FrameHitBox>();
	//enumFrag
	public bool headFrag = true;
	public bool bodyFrag = true;
	public bool footFlag = true;
    
    #region EDITOR_
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerSkill))]
    public class PlayerSkillInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("OpenSkillEditor"))
            {
				PlayerSkillEditor.Open((PlayerSkill)target);
            }
        }
    }
#endif
    #endregion
}


