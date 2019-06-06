//===============================================================
// ファイル名：NextState.cs
// 作成者    ：村上一真
// 作成日　　：20190530
// キャラについている当たり判定を管理する
//===============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBoxEventController : MonoBehaviour
{
	[System.Serializable]
	public struct HitObject
	{
		public int ID;
		public GameObject gameObject;
	}

	private List<int> nowHitEventIDAll = new List<int>();//現在Onになっている当たり判定のリスト,ID
	public List<HitObject> hitObjectsAll = new List<HitObject>();//当たり判定のリスト

	public Dictionary<int, GameObject> hitObjctTable = new Dictionary<int, GameObject>();

	public void ActivateHitBox(int ID)
	{
		//hitObjectsAll[ID].SetActive(true);
		nowHitEventIDAll.Add(ID);
	}
}
