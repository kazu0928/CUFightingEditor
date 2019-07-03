#if UNITY_EDITOR
using UnityEngine;

/// <summary>
/// InputManagerを自動的に設定してくれるクラス
/// </summary>
public class InputManagerSetter
{
	/// <summary>
	/// インプットマネージャーを設定します。
	/// </summary>
	public void SetInputManager()
	{
		//接続されているコントローラーの名前を取得
		var controllerNames = Input.GetJoystickNames();
		Debug.Log("インプットマネージャーの設定を開始します。");
		InputManagerGenerator inputManagerGenerator = new InputManagerGenerator();

		Debug.Log("設定を全てクリアします。");
		inputManagerGenerator.Clear();
		for (int i = 0; i < 2 /*controllerNames.Length*/; ++i)
		{
			if (i < controllerNames.Length)
			{
				AddPlayerInputSettings(inputManagerGenerator, i, controllerNames[i]);
				Debug.Log(string.Format("player{0} に {1} を割り当てました。", i, controllerNames[i]));
			}
			else
			{
				AddPlayerInputSettings(inputManagerGenerator, i, "");
				Debug.Log(string.Format("player{0} に キーボード入力のみ割り当てました。", i));
			}
		}

		Debug.Log("インプットマネージャーの設定が完了しました。");
	}


	/// <summary>
	/// プレイヤーごとの入力設定を追加する
	/// </summary>
	/// <param name="inputManagerGenerator">Input manager generator.</param>
	/// <param name="playerIndex">Player index.</param>
	private static void AddPlayerInputSettings(InputManagerGenerator inputManagerGenerator, int playerIndex, string controllerName)
	{
		if (playerIndex < 0 || playerIndex > 3) Debug.LogError("プレイヤーインデックスの値が不正です。");
		string upKey = "", downKey = "", leftKey = "", rightKey = "", attackKey1 = "", attackKey2 = "", attackKey3 = "";
		GetAxisKey(out upKey, out downKey, out leftKey, out rightKey, out attackKey1, out attackKey2, out attackKey3, playerIndex);

		int joystickNum = playerIndex + 1;

		//ここに各コントローラーの設定を作成
		#region BSGPAC02 Series
		if (controllerName == "BSGPAC02 Series")
		{
			//スティック
			//横方向
			{
				var name = string.Format("Player{0}_Horizontal", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 1));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
			}

			//縦方向
			{
				var name = string.Format("Player{0}_Vertical", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 2));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
			}


			//攻撃ボタン
			//弱
			{
				var name = string.Format("Player{0}_Attack1", playerIndex);
				var button = string.Format("joystick {0} button 0", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey1));
			}

			//中
			{
				var name = string.Format("Player{0}_Attack2", playerIndex);
				var button = string.Format("joystick {0} button 1", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey2));
			}

			//強
			{
				var name = string.Format("Player{0}_Attack3", playerIndex);
				var button = string.Format("joystick {0} button 2", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey3));
			}


			//決定キーなどの設定
			{
				//決定
				{
					var name = "OK";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "z", "joystick button 1"));
				}

				//キャンセル
				{
					var name = "Cancel";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "x", "joystick button 2"));
				}

				//ポーズ
				{
					var name = "Pause";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "escape", "joystick button 8"));
				}
			}
		}
		#endregion
		#region RAP.N3
		else if (controllerName == "RAP.N3")
		{
			//スティック
			//横方向
			{
				var name = string.Format("Player{0}_Horizontal", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 1));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
			}

			//縦方向
			{
				var name = string.Format("Player{0}_Vertical", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 2));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
			}


			//攻撃ボタン
			//弱
			{
				//var axis = new InputAxis();
				var name = string.Format("Player{0}_Attack1", playerIndex);
				var button = string.Format("joystick {0} button 1", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey1));
			}

			//中
			{
				var name = string.Format("Player{0}_Attack2", playerIndex);
				var button = string.Format("joystick {0} button 2", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey2));
			}

			//強
			{
				var name = string.Format("Player{0}_Attack3", playerIndex);
				var button = string.Format("joystick {0} button 7", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey3));
			}


			//決定キーなどの設定
			{
				//決定
				{
					var name = "OK";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "z", "joystick button 0"));
				}

				//キャンセル
				{
					var name = "Cancel";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "x", "joystick button 1"));
				}

				//ポーズ
				{
					var name = "Pause";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "escape", "joystick button 8"));
				}
			}
		}
		#endregion
		#region Logicool Dual Action
		else if (controllerName == "Logicool Dual Action")
		{
			//スティック
			//横方向
			{
				var name = string.Format("Player{0}_Horizontal", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 1));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
			}

			//縦方向
			{
				var name = string.Format("Player{0}_Vertical", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 2));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
			}


			//攻撃ボタン
			//弱
			{
				var name = string.Format("Player{0}_Attack1", playerIndex);
				var button = string.Format("joystick {0} button 0", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey1));
			}

			// 中
			{
				var name = string.Format("Player{0}_Attack2", playerIndex);
				var button = string.Format("joystick {0} button 1", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey2));
			}

			//強
			{
				var name = string.Format("Player{0}_Attack3", playerIndex);
				var button = string.Format("joystick {0} button 2", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey3));
			}


			//決定キーなどの設定
			{
				//決定
				{
					var name = "OK";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "z", "joystick button 1"));
				}

				//キャンセル
				{
					var name = "Cancel";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "x", "joystick button0"));
				}

				//ポーズ
				{
					var name = "Pause";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "escape", "joystick button 7"));
				}
			}
		}
		#endregion
		#region Controller (Gamepad F310)
		else if (controllerName == "Controller (Gamepad F310)")
		{
			//スティック
			//横方向
			{
				var name = string.Format("Player{0}_Horizontal", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 1));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
			}

			//縦方向
			{
				var name = string.Format("Player{0}_Vertical", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreatePadAxis(name, controllerName, joystickNum, 2));
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
			}


			//攻撃ボタン
			//弱
			{
				var name = string.Format("Player{0}_Attack1", playerIndex);
				var button = string.Format("joystick {0} button 0", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey1));
			}

			//中
			{
				var name = string.Format("Player{0}_Attack2", playerIndex);
				var button = string.Format("joystick {0} button 1", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey2));
			}

			//強
			{
				var name = string.Format("Player{0}_Attack3", playerIndex);
				var button = string.Format("joystick {0} button 2", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, button, attackKey3));
			}


			//決定キーなどの設定
			{
				//決定
				{
					var name = "OK";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "z", "joystick button 1"));
				}

				//キャンセル
				{
					var name = "Cancel";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "x", "joystick button0"));
				}

				//ポーズ
				{
					var name = "Pause";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "escape", "joystick button 7"));
				}
			}
		}
		#endregion
		//キーボードのみ設定
		else
		{
			#region キーボードのみ
			//スティック
			//横方向
			{
				var name = string.Format("Player{0}_Horizontal", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, leftKey, rightKey, "", ""));
			}

			//縦方向
			{
				var name = string.Format("Player{0}_Vertical", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreateKeyAxis(name, downKey, upKey, "", ""));
			}


			//攻撃ボタン
			//弱
			{
				//var axis = new InputAxis();
				var name = string.Format("Player{0}_Attack1", playerIndex);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "", attackKey1));
			}

			//中
			{
				var name = string.Format("Player{0}_Attack2", playerIndex);
				var button = string.Format("joystick {0} button 1", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "", attackKey2));
			}

			//強
			{
				var name = string.Format("Player{0}_Attack3", playerIndex);
				var button = string.Format("joystick {0} button 2", joystickNum);
				inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "", attackKey3));
			}


			//決定キーなどの設定
			{
				//決定
				{
					var name = "OK";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "z", ""));
				}

				//キャンセル
				{
					var name = "Cancel";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "x", ""));
				}

				//ポーズ
				{
					var name = "Pause";
					inputManagerGenerator.AddAxis(InputAxis.CreateButton(name, controllerName, "escape", ""));
				}
			}
			#endregion
			Debug.Log("***player" + playerIndex + " -> " + controllerName + " の入力が設定されていません キーボードのみ設定します***");
		}
	}

	/// <summary>
	/// キーボードでプレイした場合、割り当たっているキーを取得する
	/// </summary>
	/// <param name="upKey">Up key.</param>
	/// <param name="downKey">Down key.</param>
	/// <param name="leftKey">Left key.</param>
	/// <param name="rightKey">Right key.</param>
	/// <param name="attackKey1">Attack key1.</param>
	/// <param name="attackKey2">Attack key2.</param>
	/// <param name="attackKey3">Attack key3.</param>
	/// <param name="playerIndex">Player index.</param>
	private static void GetAxisKey(out string upKey, out string downKey, out string leftKey, out string rightKey, out string attackKey1, out string attackKey2, out string attackKey3, int playerIndex)
	{
		upKey = "";
		downKey = "";
		leftKey = "";
		rightKey = "";
		attackKey1 = "";
		attackKey2 = "";
		attackKey3 = "";

		switch (playerIndex)
		{
			case 0:
				upKey = "w";
				downKey = "s";
				leftKey = "a";
				rightKey = "d";
				attackKey1 = "f";
				attackKey2 = "g";
				attackKey3 = "h";
				break;
			case 1:
				upKey = "up";
				downKey = "down";
				leftKey = "left";
				rightKey = "right";
				attackKey1 = "[1]";
				attackKey2 = "[2]";
				attackKey3 = "[3]";
				break;
			case 2:
				upKey = "w";
				downKey = "c";
				leftKey = "q";
				rightKey = "e";
				attackKey1 = "f";
				attackKey2 = "g";
				attackKey3 = "h";
				break;
			case 3:
				upKey = "up";
				downKey = "down";
				leftKey = "left";
				rightKey = "right";
				attackKey1 = "[1]";
				attackKey2 = "[2]";
				attackKey3 = "[3]";
				break;
			default:
				Debug.LogError("プレイヤーインデックスの値が不正です。");
				upKey = "";
				downKey = "";
				leftKey = "";
				rightKey = "";
				attackKey1 = "";
				attackKey2 = "";
				attackKey3 = "";
				break;
		}
	}
}
#endif