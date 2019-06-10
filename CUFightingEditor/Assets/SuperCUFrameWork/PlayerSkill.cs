using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(menuName = "Fighting/キャラクター")]
public class PlayerSkill : ScriptableObject
{
    public AnimationClip animationClip = null;
    public float animationSpeed = 1;
    //ブレンドフラグ
    public bool inBlend = false;
    public bool outBlend = false;

    //当たり判定
    
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


