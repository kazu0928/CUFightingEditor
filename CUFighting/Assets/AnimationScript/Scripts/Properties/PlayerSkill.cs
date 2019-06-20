using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(menuName = "Fighting/スキル")]
public class PlayerSkill : ScriptableObject
{
    public enum HitBoxMode
    {
        HitBox,
        HurtBox,
        GrabAndSqueeze,
    }
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
    [System.Serializable]
    public class CustomHitBox
    {
        public HitBoxMode mode;
        public List<FrameHitBox> frameHitBoxes = new List<FrameHitBox>();
    }
	//移動用
	[System.Serializable]
	public class Move
	{
		public Vector3 movement;
		public int startFrame;
	}
	[System.Serializable]
	public class GravityMove
	{
		public Vector3 movement;
		public Vector3 limitMove;
		public int startFrame;
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
    public List<FrameHitBox> plusGrabHitBox = new List<FrameHitBox>();

    public List<CustomHitBox> customHitBox = new List<CustomHitBox>();

	//移動
	public List<Move> movements = new List<Move>();
	public List<GravityMove> gravityMove = new List<GravityMove>();

	//enumFrag
	public bool headFrag = true;
	public bool bodyFrag = true;
	public bool footFlag = true;
    public bool grabFrag = true;
    
    #region EDITOR_
#if UNITY_EDITOR
    [CustomEditor(typeof(PlayerSkill))]
    public class PlayerSkillInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if(GUILayout.Button("技設定画面を開く"))
            {
				PlayerSkillEditor.Open((PlayerSkill)target);
            }
        }
    }
#endif
    #endregion
}


