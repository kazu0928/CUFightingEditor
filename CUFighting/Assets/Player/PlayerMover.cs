using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
	private FighterBase fb;
    void Start()
    {
		fb = gameObject.GetComponent<FighterBase>();
    }
	float x = 2;
    void Update()
    {
		//移動
		//transform.Translate(x, 0, 0);
		//x -= 0.2f;
		
		//fb.NowPlaySkill;
		//重力
	}
}
