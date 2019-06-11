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
	[SerializeField]
	private int frameSpeed = 60;//アニメーションの速さ
	[SerializeField]
	private AnimationClip nowPlayAnimatin = null;//再生中のAnimationClip

	#region Playable_Properties
	private PlayableGraph playableGraph; //playableGraph
	AnimationMixerPlayable mixer;   //animationmixerPlayable
	private AnimationClipPlayable _nowPlayAnimation;
	private AnimationClipPlayable _beforePlayAnimation;
	#endregion

	#region 初期化処理
	private void Awake()
	{
		playableGraph = PlayableGraph.Create();
	}
	private void Start()
    {
		if(nowPlayAnimatin != null)
		{
			// AnimationClipをMixerに登録
			_nowPlayAnimation = AnimationClipPlayable.Create(playableGraph, nowPlayAnimatin);
			mixer = AnimationMixerPlayable.Create(playableGraph, 2, true);
			mixer.ConnectInput(0, _nowPlayAnimation, 0);
			mixer.SetInputWeight(0, 1);
		}
    }
	#endregion
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
		mixer.SetInputWeight(0, 0.5f);
		mixer.SetInputWeight(1, 0.5f);
		playableGraph.Evaluate(1.0f / nomalSkills[0].animationClip.frameRate);
    }
#if UNITY_EDITOR
    public List<Vector3> drawGizmoBox = new List<Vector3>();
    private void OnDrawGizmos()
    {
    }
#endif
}
