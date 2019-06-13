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
	private float frameSpeed = 1;//アニメーションの速さ
	[SerializeField]
	private AnimationClip nowPlayAnimation = null;//再生中のAnimationClip
	private AnimationClip _beforeNowPlayClip = null;
	[SerializeField]
	private AnimationClip setPlayAnimation;
	[SerializeField]
	private int changeWeightFrame = 0;
	private int _nowChangeFrame = 0;

	[SerializeField]
	private int changeFrameNow = 10;

	#region Playable_Properties
	private PlayableGraph playableGraph; //playableGraph
	AnimationMixerPlayable mixer;   //animationmixerPlayable
	private AnimationClipPlayable _nowPlayAnimation;
	private AnimationClipPlayable _beforePlayAnimation;
	#endregion

	[Range(0, 1)]
	public float num = 0;

	#region 初期化処理
	private void Awake()
	{
		playableGraph = PlayableGraph.Create();
		playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
	}
	private void Start()
    {
		if (nowPlayAnimation != null)
		{
			_beforeNowPlayClip = nowPlayAnimation;
			// AnimationClipをMixerに登録
			_nowPlayAnimation = AnimationClipPlayable.Create(playableGraph, nowPlayAnimation);
			mixer = AnimationMixerPlayable.Create(playableGraph, 2, true);
			mixer.ConnectInput(0, _nowPlayAnimation, 0);
			mixer.SetInputWeight(0, 1);
			var output = AnimationPlayableOutput.Create(playableGraph, "output", GetComponent<Animator>());
			output.SetSourcePlayable(mixer);
			playableGraph.Play();
		}
		
	}
	#endregion
	private void Update()
    {
		if(nowPlayAnimation == null)
		{
			if(mixer.IsValid())mixer.Destroy();
			_beforeNowPlayClip = null;
		}
		else if (_beforeNowPlayClip != nowPlayAnimation)
		{
			SetPlayAnimation(nowPlayAnimation, frameSpeed , changeFrameNow);
		}
		SetNextAnimationPlayable();
		if (playableGraph.IsValid() && mixer.IsValid())
		{
			if (frameSpeed <= 0)
			{
				return;
			}
			if (changeWeightFrame > 0)
			{
				mixer.SetInputWeight(0, (1.0f - num) - ((1.0f) - (((float)_nowChangeFrame) / ((float)changeWeightFrame))));
				mixer.SetInputWeight(1, num + ((1.0f) - (((float)_nowChangeFrame) / ((float)changeWeightFrame))));
				if (_nowChangeFrame >= changeWeightFrame)
				{
					changeWeightFrame = 0;
					_nowChangeFrame = 0;
				}
				_nowChangeFrame++;
			}
			else
			{
				mixer.SetInputWeight(0, (1.0f - num));
				mixer.SetInputWeight(1, num);
			}
			playableGraph.Evaluate((1.0f / nowPlayAnimation.frameRate) * frameSpeed);
		}
	}
	private void SetNextAnimationPlayable()
	{
		if (setPlayAnimation != null && playableGraph.IsValid())
		{
			if (mixer.IsValid())
			{
				playableGraph.Disconnect(mixer, 0);
				playableGraph.Disconnect(mixer, 1);
			}
			else
			{
				mixer = AnimationMixerPlayable.Create(playableGraph, 2, true);
				var output = AnimationPlayableOutput.Create(playableGraph, "output", GetComponent<Animator>());
				output.SetSourcePlayable(mixer);
			}
			if (_beforePlayAnimation.IsValid())
			{
				_beforePlayAnimation.Destroy();
			}
			if (_nowPlayAnimation.IsValid())
			{
				_beforePlayAnimation = _nowPlayAnimation;
			}
			//今のアニメーションに設定
			nowPlayAnimation = setPlayAnimation;
			_beforeNowPlayClip = nowPlayAnimation;
			setPlayAnimation = null;
			//アニメーションプレイアブル作成
			_nowPlayAnimation = AnimationClipPlayable.Create(playableGraph, nowPlayAnimation);
			mixer.ConnectInput(0, _nowPlayAnimation, 0);
			mixer.ConnectInput(1, _beforePlayAnimation, 0);
		}
	}
	public void SetPlayAnimation(AnimationClip clip, float speed , int weightFrame = 0)
	{
		setPlayAnimation = clip;
		changeWeightFrame = weightFrame;
		_nowChangeFrame = 0;
		frameSpeed = speed;
	}

	private void OnDestroy()
	{
		if(playableGraph.IsValid()) playableGraph.Destroy();
	}

 //   public void AnimationInit()
 //   {
	//	//playableGraphの設定
	//	playableGraph = PlayableGraph.Create();
	//	playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
	//	//animationPlayableOutputの作成
	//	var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", gameObject.GetComponent<Animator>());
	//	//mixerの作成
	//	mixer = AnimationMixerPlayable.Create(playableGraph);

	//	//mixerの設定
	//	//mixerにanimationClipPlayableを追加
	//	foreach (KeyValuePair<CharacterAnimation,AnimationClipPlayable> pair in nomalSkillPlayables)
	//	{
	//		mixer.AddInput(pair.Value, 0);
	//	}
	//	foreach(KeyValuePair<string,AnimationClipPlayable> pair in custumSkillPlayables)
	//	{
	//		mixer.AddInput(pair.Value, 0);
	//	}
	//	//mixerをoutputに接続
	//	playableOutput.SetSourcePlayable(mixer);
	//	playableGraph.Play();
	//}
	//public void UpdateAnimate()
 //   {
	//	if(frameSpeed<=0)
	//	{
	//		return;
	//	}
	//	mixer.SetInputWeight(0, 0.5f);
	//	mixer.SetInputWeight(1, 0.5f);
	//	playableGraph.Evaluate(1.0f / nomalSkills[0].animationClip.frameRate);
 //   }
#if UNITY_EDITOR
    public List<Vector3> drawGizmoBox = new List<Vector3>();
    private void OnDrawGizmos()
    {
    }
#endif
}
