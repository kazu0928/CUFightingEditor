using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//enumを使うために必要
using System;

public class TestInput : MonoBehaviour {
	public int playerIndex; //プレイヤー番号
	public string player;
	public string controllerName = ""; //使用するコントローラーの名前
	public Vector2 inputDirection; //ジョイスティックの入力方向
	public string direction; //現在のジョイスティックの方向
	public int lastDir = 5; //前回のジョイスティックの方向
	public string playerDirection; //プレイヤーの入力方向
	public string atkBotton; //攻撃ボタンの名前を格納

	//ジョイスティックの入力方向（方向はNumパッドに依存）

	enum DirJS {
		NONE,
		d1 = 1, //RIGHT_DOWN
		d2 = 2, //DOWN
		d3 = 3, //LEFT_DOWN
		d4 = 4, //RIGHT
		d5 = 5, //CENTER
		d6 = 6, //LEFT
		d7 = 7, //RIGHT_UP
		d8 = 8, //UP
		d9 = 9 //LEFT_UP
	}

	void Start () {
		player = string.Format ("Player{0}_", playerIndex);
	}
	public void UpdateGame () {
		DownKeyCheck ();
	}

	public void SetAxis () {
		inputDirection.x = Input.GetAxisRaw (player + "Horizontal");
		inputDirection.y = Input.GetAxisRaw (player + "Vertical");
	}
	public void SetDirection () {
		SetAxis ();
		float nowDir = 5 + inputDirection.x + (inputDirection.y * 3);
		//方向を調べる
		switch (nowDir) {
			case ((int) DirJS.d1):
				lastDir = (int) DirJS.d1;
				playerDirection = "1";
				break;
			case (int) DirJS.d2:
				lastDir = (int) DirJS.d2;
				playerDirection = "2";
				break;
			case (int) DirJS.d3:
				lastDir = (int) DirJS.d3;
				playerDirection = "3";
				break;
			case (int) DirJS.d4:
				lastDir = (int) DirJS.d4;
				playerDirection = "4";
				break;
			case (int) DirJS.d5:
				lastDir = (int) DirJS.d5;
				playerDirection = "5";
				break;
			case (int) DirJS.d6:
				lastDir = (int) DirJS.d6;
				playerDirection = "6";
				break;
			case (int) DirJS.d7:
				lastDir = (int) DirJS.d7;
				playerDirection = "7";
				break;
			case (int) DirJS.d8:
				lastDir = (int) DirJS.d8;
				playerDirection = "8";
				break;
			case (int) DirJS.d9:
				lastDir = (int) DirJS.d9;
				playerDirection = "9";
				break;
			default:
				lastDir = 0;
				direction = null;
				break;
		}
	}

	void DownKeyCheck () {
		SetDirection ();
		SetAtkBotton ();
		if (Input.anyKey) {
			//ジョイスティックまたはキーボードでの方向


			//攻撃ボタン
			if (atkBotton != "") Debug.Log (atkBotton);
		}

	}

	public void SetAtkBotton () 
	{
		atkBotton = "";
		if (Input.GetButtonDown (player + "Attack1")) atkBotton += "_Atk1";
		if (Input.GetButtonDown (player + "Attack2")) atkBotton += "_Atk2";
		if (Input.GetButtonDown (player + "Attack3")) atkBotton += "_Atk3";
	}

	public string GetPlayerAtk()
    {
        string s = null;
        if (atkBotton != "")
        {
            s = atkBotton;
        }
        return s;
    }

}