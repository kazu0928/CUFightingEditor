#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CUEngine.Pattern;
using UnityEditor;

public class Test : EditorWindow
{
	//変数
	public static InputManagerSetter inputManagerSetter = new InputManagerSetter();
	public string[] controller = new string[2];
    public TestInput[] testInput;
	[MenuItem("Window/aaaas")]
	public static void Open()
	{
        var controllerNames = Input.GetJoystickNames();
		inputManagerSetter.SetInputManager();
    }

	////コントローラーを設定する
	//void SetController()
	//{
	//	var controllerNames = Input.GetJoystickNames();
	//	for (int i = 0; i < 2; ++i)
	//	{
	//		Debug.Log(string.Format("{0} *** {1}", controller[i], controllerNames[i]));
	//		if (controller[i] != controllerNames[i])
	//		{
	//			//InputManager自動生成させる
	//		}
	//	}
	//	controller = controllerNames;
	//}
}
#endif