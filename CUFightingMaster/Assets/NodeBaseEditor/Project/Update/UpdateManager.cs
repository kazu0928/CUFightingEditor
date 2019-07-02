//===============================================================
// ファイル名：UpdateManager.cs
// 作成者    ：村上一真
// 作成日　　：20190516
// Updateを一括で管理
//===============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;

namespace CUEngine
{
    public class UpdateManager : SingletonMono<UpdateManager>
    {
        public List<IEventable> updateList = new List<IEventable>();
        protected override void Awake()
        {
            dontDs = false;     //そのシーンだけしか残らないようにする
        }
        /// <summary>
        /// このメソッドを呼び出してアップデートするものを追加
        /// </summary>
        /// <param name="ev"></param>
        public void AddUpdate(IEventable ev)
        {
            updateList.Add(ev);
        }
        private void Update()
        {
            //アップデートリストにあるすべてを呼び出し
            for(int i = 0;i<updateList.Count;i++)
            {
                updateList[i].UpdateGame();
            }
        }
    }
}
