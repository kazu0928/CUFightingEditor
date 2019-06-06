//===============================================================
// ファイル名：AnimationPlayer.cs
// 作成者    ：村上一真
// 作成日　　：20190531
// Animation,当たり判定（アニメーション中）を管理するクラス
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class AnimationPlayer : MonoBehaviour
{
	//キャラクタの持つアニメーション
	[System.Serializable]
	public class NomalAnim
	{
		public CharacterAnimation characterAnimation;
		public AnimationClip animationClip;
		public List<GameObject> hitBoxObjects;
	}
	[System.Serializable]
	public class SkillAnim
	{
		public string skillName;
		public AnimationClip animationClip;
	}
	public List<NomalAnim> nomalSkills;//通常のアニメーション
	public List<SkillAnim> custumSkills;//追加アニメーション
	//--------------------------------------------------------
	[SerializeField]
	private int frameSpeed = 60;//アニメーションの速さ

    private PlayableGraph playableGraph; //playableGraph
	AnimationMixerPlayable mixer;   //animationmixerPlayable
	//通常アニメーションのプレイアブル
	private Dictionary<CharacterAnimation, AnimationClipPlayable> nomalSkillPlayables = new Dictionary<CharacterAnimation, AnimationClipPlayable>();
	//追加アニメーションのプレイアブル
	private Dictionary<string, AnimationClipPlayable> custumSkillPlayables = new Dictionary<string, AnimationClipPlayable>();
	private AnimationClipPlayable _nowPlayAnimation;
	private AnimationClipPlayable _beforePlayAnimation;

    void Start()
    {
        AnimationInit();
    }
    private void Update()
    {
        UpdateAnimate();
    }
    void OnDisable()
    {
        // グラフで作成されたすべての Playables と PlayableOutputs を破棄します
        playableGraph.Destroy();
    }

    public void AnimationInit()
    {
		//playableGraphの設定
		playableGraph = PlayableGraph.Create();
		playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
		//animationPlayableOutputの作成
		var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", gameObject.GetComponent<Animator>());
		//mixerの作成
		mixer = AnimationMixerPlayable.Create(playableGraph);

		//animationClipをラップして格納
		foreach (AnimationPlayer.NomalAnim anim in nomalSkills)
		{
			nomalSkillPlayables.Add(anim.characterAnimation,AnimationClipPlayable.Create(playableGraph, anim.animationClip));
		}
		foreach (AnimationPlayer.SkillAnim anim in custumSkills)
		{
			custumSkillPlayables.Add(anim.skillName, AnimationClipPlayable.Create(playableGraph, anim.animationClip));
		}

		//mixerの設定
		//mixerにanimationClipPlayableを追加
		foreach (KeyValuePair<CharacterAnimation,AnimationClipPlayable> pair in nomalSkillPlayables)
		{
			mixer.AddInput(pair.Value, 0);
		}
		foreach(KeyValuePair<string,AnimationClipPlayable> pair in custumSkillPlayables)
		{
			mixer.AddInput(pair.Value, 0);
		}
		//mixerをoutputに接続
		playableOutput.SetSourcePlayable(mixer);
		playableGraph.Play();
	}
	public void UpdateAnimate()
    {
		if(frameSpeed<=0)
		{
			return;
		}
		mixer.SetInputWeight(0, 1);
		playableGraph.Evaluate(1.0f / frameSpeed);
    }
}
