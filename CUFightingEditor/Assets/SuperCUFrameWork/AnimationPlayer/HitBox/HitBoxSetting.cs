//===============================================================
// ファイル名：HitBoxSetting.cs
// 作成者    ：村上一真
// 作成日　　：20190530
// HitBoxの初期化、管理用
//===============================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxSetting : MonoBehaviour
{
	private void Awake()
	{
		gameObject.SetActive(false);
	}

	public void OnHitBox()
	{
		this.gameObject.SetActive(true);
	}
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		BoxCollider col = GetComponent<BoxCollider>();
		Gizmos.color = new Color(1,0,0,0.5f);
		if(gameObject.activeSelf == true)
		{
			Gizmos.DrawCube (transform.position + col.center, new Vector3(col.size.z,col.size.y,col.size.x) );
		}
	}
#endif
}
