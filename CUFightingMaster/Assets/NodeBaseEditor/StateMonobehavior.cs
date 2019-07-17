//===============================================================
// ファイル名：StateProcessor.cs
// 作成者    ：村上一真
// 作成日　　：20190516
// 現在のステートの移行、ステートの挙動を担うクラス。
// MonoBehaviourを継承、このコンポーネントが軸。
//
// ※ステートの中身をプレイ中動的に変えることは出来ないので注意
// 20190531 更新・Debugで読み込んでるバグの修正および名前のないステートでも動作するように
//===============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CUEngine.Pattern
{
    public class StateMonobehavior : MonoBehaviour, IEventable
    {
        //現在処理中の本体
        public List<StateBody> stateBody = new List<StateBody>();
        public StateBody nowPlayStateBody{ get;set; }

        public Dictionary<int, StateBody> bodyDic = new Dictionary<int, StateBody>();
        protected virtual void Awake() 
        {
            foreach(StateBody s in stateBody)
            {
                bodyDic.Add(s.ID.number, s);
            }
            foreach (StateBody s in stateBody)
            {
                foreach (NomalState sb in s.states)
                {
                    if (sb.stateMode == StateMode.SubState)
                    {
                        sb.stateBody = bodyDic[sb.subStateID.number];
                    }
                }
            }
            nowPlayStateBody = stateBody[0];
            nowPlayStateBody.AwakeInit();
            bodyDic.Clear();
        }
        protected virtual void Start() 
        {
            UpdateManager.Instance.AddUpdate(this);
            nowPlayStateBody.StartInit(this);
        }
        public virtual void UpdateGame()
        {
            nowPlayStateBody.UpdateGame();
        }
        //インターフェースを継承しているのでおいておくだけ
        public virtual void LateUpdateGame()
        {
        }
        public virtual void FixedUpdateGame()
        {
        }
    }
    [System.Serializable]
    //ステート本体
    public class StateBody
    {
        public InstanceID ID = new InstanceID();
        public StateBody parant = null;
        private StateMonobehavior mono = null;
        //名前
        public string stateName;

        //変更前のステート名
        private string _beforeStateName;

        public bool stateMove = false;

        //ステート（プロセッサ）
        public StateProcessor stateProcessor = new StateProcessor();
        //使用するすべてのステート
        public List<NomalState> states = new List<NomalState>();

        //TODO:DictionaryではなくIDで管理する//OK
        private Dictionary<int, NomalState> stateDictionary = new Dictionary<int, NomalState>();
        //全部初期化
        public void AwakeInit()
        {
            foreach (NomalState state in states)
            {
                //サブステートの初期化
                if(state.stateMode == StateMode.SubState)
                {
                    state.stateBody.AwakeInit();
                }
                foreach (NextStateJudge judge in state.nextStateJudge)
                {
                    judge.Init();
                }
            }
            foreach (NomalState nomalState in states)
            {
                nomalState.Init();
            }
        }
        //初期化処理
        public void StartInit(StateMonobehavior mono)
        {
            this.mono = mono;
            //ステートがあれば処理
            if (states.Count > 0)
            {
                //0がスタートステート
                stateProcessor.State = states[0];//ステート0をスタートステートに
                                                 //ステートの中身のアップデートデリゲートに登録
                //最初がSubStateだったら
                if(states[0].stateMode == StateMode.SubState)
                {
                    mono.nowPlayStateBody = states[0].stateBody;
                }
                foreach (NomalState ns in states)
                {
                    //サブステートの処理
                    if(ns.stateMode == StateMode.SubState)
                    {
                        ns.stateBody.StartInit(mono);
                        ns.stateBody.SetParant(this);
                    }
                    ns.updateJudgeDelegate = NomalStateJudge;   //判定
                    stateDictionary.Add(ns.ID.number, ns);//辞書追加
                    ns.nextStateJudge.Sort((a, b) => b.priority - a.priority);//優先度順にソート(高ければ高い、降順)
                }
            }
            //ステートの格納
            foreach (NomalState state in states)
            {
                if (state.nextStateJudge.Count > 0)
                {
                    foreach (NextStateJudge judge in state.nextStateJudge)
                    {
                        //TODO:同じ名前のステートでもできるようにする
                        if (judge.nextStateName == null)
                        {
                            judge.nextState = null;
                            continue;
                        }
                        judge.nextState = stateDictionary[judge.nextID.number];
                    }
                }
            }
            //入った時の処理
            if ((stateProcessor.State != null)&&(mono.nowPlayStateBody == this))
            {
                stateProcessor.Execute();
            }
        }
        //常時処理
        public virtual void UpdateGame()
        {
            stateMove = false;
            //ステートがあれば実行
            if (states.Count > 0)
            {
                //現在のステートの常時実行処理
                stateProcessor.PlayUpdate();
            }
        }
		public bool _frag = false;
        /// <summary>
        /// 判定処理、現在のステートの中身から判定処理を実行(delegate)
        /// </summary>
        public void NomalStateJudge()
        {
            //優先度順にジャッジ(ソートは別)
            foreach (NextStateJudge judge in stateProcessor.State.nextStateJudge)
            {
                //次のステートが登録されていなければ移行しない
                if (judge.nextState == null)
                {
                    continue;
                }
                
                if (judge.judgeFunc.Invoke())
                {
                    if (mono.nowPlayStateBody != this)
                    {
                        mono.nowPlayStateBody = this;
                    }
                    stateProcessor.State = judge.nextState;
                    stateMove = true;
					bool subState = false;
                    //サブステート有の場合
                    if(judge.nextState.stateMode == StateMode.SubState)
                    {
                        mono.nowPlayStateBody = judge.nextState.stateBody;
                        judge.nextState.stateBody.stateProcessor.State = judge.nextState.stateBody.states[0];
                        judge.nextState.stateBody.stateMove = true;
                        mono.nowPlayStateBody.stateProcessor.Execute();
						subState = true;
                    }
                    else if(judge.nextState.stateMode == StateMode.EndState)
                    {
                        if(parant != null)
                        {
                            mono.nowPlayStateBody = parant;
                            mono.nowPlayStateBody.stateProcessor.PlayUpdate();
                        }
                    }
					if (stateProcessor.State != null)
					{
						stateProcessor.Execute();
					}
					if (subState)
					{
						if(judge.nextState.stateBody.states[0].isSkipState)
						{
							mono.nowPlayStateBody._frag = true;
							mono.nowPlayStateBody.NomalStateJudge();
						}
					}
                    else if(judge.nextState.isSkipState)
                    {
						mono.nowPlayStateBody._frag = true;
                        mono.nowPlayStateBody.NomalStateJudge();
                    }
					//ステートの値が変更されたら実行処理を行う
					if (stateProcessor.State == null)
					{
						return;
					}
					if (stateMove)
					{
						_beforeStateName = stateProcessor.State.getStateName();
					}
					break;
                }
            }
            StateBody pa = parant;
            //親のジャッジ処理も行う
            while((pa!=null) &&( _frag == false))
            {
                pa.UpdateGame();
                pa = pa.parant;
            }
			_frag = false;
        }
        //親のセット
        public void SetParant(StateBody parant)
        {
            this.parant = parant;
        }
    }
}
