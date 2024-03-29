﻿//===============================================================
// ファイル名：SingletonMono.cs
// 作成者    ：村上一真
// MonoBehaviour用のシングルトン、継承して使用
// ゲーム中ずっと残すことも可能
//===============================================================
using UnityEngine;
using System;

namespace CUEngine.Pattern
{
    public abstract class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        public bool dontDs;

        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    if (instance == null)
                    {
                        Debug.LogError(t + " をアタッチしているGameObjectはありません");
                    }
                }

                return instance;
            }
        }

        virtual protected void Awake()
        {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する
            CheckInstance();


        }

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = this as T;
                if (dontDs == true)
                {
                    DontDestroyOnLoad(this.gameObject);
                }
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }
            Destroy(this.gameObject);
            return false;
        }
    }
}
