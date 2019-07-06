//===============================================================
// ファイル名：AnimationPlayer.cs
// 作成者    ：村上一真
// 作成日　　：20190531
// Animationを再生,ブレンドする基底クラス
//===============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public abstract class AnimationPlayerBase : MonoBehaviour
{
    //現在再生中のAnimationClip
    [SerializeField] private AnimationClip nowClip = null;
    //次に再生するAnimationClip
    [SerializeField] private AnimationClip setClip = null;
    //再生中のアニメーションの速度
    [SerializeField] private float animationSpeed = 1.0f;
    //何フレームでアニメーションを移行させるか（ブレンド）
    [SerializeField] private int changeWeightFrame = 0;
    //現在再生中のアニメーションのフレーム数
    [SerializeField] private int nowFrame = 0;
	//再生されてからの合計のフレーム数
	[SerializeField] private int frameCount = 0;
	public int NowFrame
    {
        get { return nowFrame; }
    }
    public int FrameCount
    {
        get { return frameCount; }
    }
    //アニメーションが再生終了したかどうかのフラグ
    private bool endAnimFrag = false;
    public bool EndAnimFrag
    {
        get { return endAnimFrag; }
    }
    //フレーム開始時に再生していたAnimationClip
    private AnimationClip beforeClip = null;
	private double beforeClipTime = 0;
    #region Playable系プロパティ
    private PlayableGraph playableGraph; //playableGraph
    private AnimationMixerPlayable mixer; //mixer
    private AnimationClipPlayable nowClipPlayable; //再生中のAnimationClipPlayable
    private AnimationClipPlayable beforeClipPlayable; //前に再生されていたAnimationClipPlayable、ブレンドにも使用
    #endregion
 
    #region 初期化処理
    protected virtual void Awake()
    {
        //playableGraphを生成し、Updateをマニュアル(明示的に呼び出し)に変更
        playableGraph = PlayableGraph.Create();
        playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
    }
    protected virtual void Start()
    {
        //再生時にアニメーションクリップが登録されていたら再生
        if (nowClip != null)
        {
            beforeClip = nowClip;
            //AnimationClipをMixerに登録
            nowClipPlayable = AnimationClipPlayable.Create(playableGraph, nowClip);
            mixer = AnimationMixerPlayable.Create(playableGraph, 2, true);
            mixer.ConnectInput(0, nowClipPlayable, 0);
            mixer.SetInputWeight(0, 1);
            //Animatorにセットアップする（output）
            var output = AnimationPlayableOutput.Create(playableGraph, "output", GetComponent<Animator>());
            output.SetSourcePlayable(mixer);
            //再生
            playableGraph.Play();
        }
    }
    #endregion

    //次のアニメーションをセットするときに呼び出す
    public void SetPlayAnimation(AnimationClip clip, float speed, int weightFrame = 0)
    {
        setClip = clip;
        changeWeightFrame = weightFrame;
        animationSpeed = speed;
        frameCount = 0;
        if (playableGraph.IsValid() && mixer.IsValid())
        {
			beforeClipTime = nowClipPlayable.GetTime();
            mixer.SetTime(0);
            nowClipPlayable.SetTime(0);
            if (beforeClipPlayable.IsValid()) beforeClipPlayable.SetTime(0);
        }
    }

    protected void UpdateGame()
    {
        AnimationPlayableSetting();//再生中のPlayableの設定
        AnimationFrameUpdate();
    }
 
    #region 再生中のアニメーションの設定
    //Updateの最初に処理する
    private void AnimationPlayableSetting()
    {
        //playableGraphが登録されていなければ登録
        if (!(playableGraph.IsValid()))
        {
            playableGraph = PlayableGraph.Create();
        }
        //再生中のアニメーションがなければmixerを削除して停止
        if (nowClip == null)
        {
            if (mixer.IsValid()) mixer.Destroy();//mixerの削除
            beforeClip = null;
        }
        //再生中のAnimationClipが変更されたら再生を変更(Editor上で入れ替える時に使用、同じアニメーションクリップを連続で再生は出来ない)
        else if (beforeClip != nowClip)
        {
            SetPlayAnimation(nowClip, animationSpeed, changeWeightFrame);
        }
        //別のAnimationが次に再生するアニメーションとしてセットされたらPlayableを変更する
        SetAnimationPlayable();
    }
    //別のAnimationが次に再生するアニメーションとしてセットされたらPlayableを変更する
    private void SetAnimationPlayable()
    {
        //次に再生するアニメーションが登録されていればアニメーションを変更
        if (setClip != null)
        {
            frameCount = 0;
            if (playableGraph.IsValid() && mixer.IsValid())
            {
                mixer.SetTime(0);
                nowClipPlayable.SetTime(0);
                if (beforeClipPlayable.IsValid()) beforeClipPlayable.SetTime(0);
            }

            //mixerがすでに生成されていればmixerの登録を削除
            if (mixer.IsValid())
            {
                playableGraph.Disconnect(mixer, 0);
                playableGraph.Disconnect(mixer, 1);
            }
            //されていなければ生成、登録
            else
            {
                mixer = AnimationMixerPlayable.Create(playableGraph, 2, true);
                var output = AnimationPlayableOutput.Create(playableGraph, "output", GetComponent<Animator>());
                output.SetSourcePlayable(mixer);
            }
            //前のアニメーションがあればそれを削除
            if (beforeClipPlayable.IsValid())
            {
                beforeClipPlayable.Destroy();
            }
            //前のアニメーションに現在のアニメーションを登録
            if (nowClipPlayable.IsValid())
            {
                beforeClipPlayable = nowClipPlayable;
            }
            //現在のアニメーションにセットされたアニメーションを設定
            nowClip = setClip;
            beforeClip = nowClip;
            setClip = null;
            //アニメーションプレイアブル作成
            nowClipPlayable = AnimationClipPlayable.Create(playableGraph, nowClip);
            mixer.ConnectInput(0, nowClipPlayable, 0);
            mixer.ConnectInput(1, beforeClipPlayable, 0);
        }
    }
    #endregion

    #region  アニメーションの再生
    private void AnimationFrameUpdate()
    {
        //mixerがない、アニメーション速度が0以下
        if ((!(mixer.IsValid())) || (animationSpeed <= 0))
        {
            return;
        }
        mixer.SetTime(0);
        //ブレンドフレームが設定されていれば
        if (changeWeightFrame > 0)
        {
            float changeMinus = (1.0f - ((float)frameCount / (float)changeWeightFrame));
            //フレームごとにブレンド
            mixer.SetInputWeight(0, 1.0f - changeMinus);
            mixer.SetInputWeight(1, changeMinus);
            //完全にブレンドしたら数字を0に
            if (frameCount >= changeWeightFrame)
            {
                changeWeightFrame = 0;
            }
        }
        //なければ普通に再生
        else
        {
            mixer.SetInputWeight(0, 1.0f);
            mixer.SetInputWeight(1, 0);
        }
        //更新
        if (beforeClipPlayable.IsValid()) beforeClipPlayable.SetTime(beforeClipTime);
        playableGraph.Evaluate((1.0f / nowClip.frameRate) * animationSpeed);
		//フレーム取得
		nowFrame = (int)(((float)nowClipPlayable.GetTime() * nowClip.frameRate) * (1 / animationSpeed));
        bool endAnimation = (nowFrame >= (int)((nowClip.length * nowClip.frameRate * (1.0f / animationSpeed))));
        //現在のフレーム数が最大であれば
        if (endAnimation)
        {
            if(nowClip.isLooping)
            {
                mixer.SetTime(0);
                nowClipPlayable.SetTime(0);
                if(beforeClipPlayable.IsValid())beforeClipPlayable.SetTime(0);
            }
        }
        //アニメーションが終わっているかどうかの判定
        endAnimFrag = endAnimation;

        frameCount++;
        if(frameCount>=int.MaxValue-1)
        {
            frameCount = 0;
        }
    }
    #endregion

    //終了時の破棄
    private void OnDestroy()
    {
        if (playableGraph.IsValid()) playableGraph.Destroy();
    }
}