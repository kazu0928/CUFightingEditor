//===============================================================
// ファイル名：AnimationPlayer.cs
// 作成者    ：村上一真
// 作成日　　：20190531
// Animationを再生,ブレンドするクラス
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class AnimationPlayer : MonoBehaviour
{
	[SerializeField]
	protected float frameSpeed = 1;//アニメーションの速さ
	[SerializeField]
	protected AnimationClip nowPlayAnimation = null;//再生中のAnimationClip
	protected AnimationClip _beforeNowPlayClip = null;
	[SerializeField]
	protected AnimationClip setPlayAnimation;
	[SerializeField]
	protected int changeWeightFrame = 0;
	protected int _nowChangeFrame = 0;

	[SerializeField]
	protected int changeFrameNow = 10;
    //現在のフレーム
    [SerializeField]
    protected int nowFrame = 0;
	#region Playable_Properties
	private PlayableGraph playableGraph; //playableGraph
	AnimationMixerPlayable mixer;   //animationmixerPlayable
	private AnimationClipPlayable _nowPlayAnimation;
	private AnimationClipPlayable _beforePlayAnimation;
	#endregion

	[Range(0, 1)]
	public float num = 0;

	#region 初期化処理
	protected void Awake()
	{
		playableGraph = PlayableGraph.Create();
		playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
	}
	protected void Start()
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
	protected void Update()
    {
		if (nowPlayAnimation == null)
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
            nowFrame = (int)(((float)mixer.GetTime() * nowPlayAnimation.frameRate) * (1 / frameSpeed));//フレーム取得
																								 //ループ
			if (nowFrame >= (int)((nowPlayAnimation.length * nowPlayAnimation.frameRate *(1.0f / frameSpeed)))&&nowPlayAnimation.isLooping)
            {
                mixer.SetTime(0);
                _nowPlayAnimation.SetTime(0);
                if(_beforePlayAnimation.IsValid()) _beforePlayAnimation.SetTime(0);
            }
        }
	}
	//時間取得
    public int GetAnimationTime()
    {
        return nowFrame;
    }
    //次のアニメーションのプレイアブル作成
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
    /// <summary>
    /// 次のアニメーションのセット
    /// </summary>
    /// <param name="clip">AnimationClip</param>
    /// <param name="speed">速度</param>
    /// <param name="weightFrame">ウェイト</param>
	public void SetPlayAnimation(AnimationClip clip, float speed , int weightFrame = 0)
	{
		setPlayAnimation = clip;
		changeWeightFrame = weightFrame;
		_nowChangeFrame = 0;
		frameSpeed = speed;
		//再生時間を0にする
		if (playableGraph.IsValid() && mixer.IsValid())
		{
			mixer.SetTime(0);
			_nowPlayAnimation.SetTime(0);
			if (_beforePlayAnimation.IsValid()) _beforePlayAnimation.SetTime(0);
		}
	}

	private void OnDestroy()
	{
		if(playableGraph.IsValid()) playableGraph.Destroy();
	}
}
