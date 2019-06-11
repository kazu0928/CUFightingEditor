//===============================================================
// ファイル名：PlayerAnimationEditor.cs
// 作成者    ：村上一真
// 作成日　　：20190531
// Animation,当たり判定（アニメーション中）を作成するエディタ
// Editor上でもPlayableAPIをそのまま使用すると停止時の解放等に支障が出るためここではSampleAnimationを使用
//===============================================================
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using UnityEditorInternal;
//using UnityEngine.Animations;

//public partial class PlayerAnimationEditor : EditorWindow
//{
//	   public bool initFlag = false;
//	   public static PlayerAnimationEditor window = null;
//	   [MenuItem("SuperCU/AnimationEditor")]
//	   public static void Open()
//	   {
//	       if (window == null)
//	       {
//	           window = (PlayerAnimationEditor)CreateInstance(typeof(PlayerAnimationEditor));
//	           window.Show();
//	       }
//	   }


//	   SerializedObject skillSerializedObject;
//	   //ノーマルスキル
//	   SerializedProperty nomalSerializedProperty;
//	   ReorderableList nomalReorderable;
//	   //カスタムスキル
//	   SerializedProperty custumSerializedProperty;
//	   ReorderableList custumReorderable;
//	   Color custumRColor = new Color(1, 0.5f, 0.5f, 1f);

//	   //アニメーション管理クラス
//	   AnimationPlayer animationPlayer = null;

//	List<AnimationClip> animations = new List<AnimationClip>(); //AnimationClipPlayable,すべてのアニメーション
//	List<string> animationNames = new List<string>();

//	//管理しているゲームオブジェクト
//	private GameObject gameObject;
//	private GameObject _beforeGameObject;

//	   //バー
//	int value = 0;//現在の位置
//	int rightValue = 0;//最大値
//	int leftValue = 0;//最小値

//	int nowAnimation = 0;
//	int _beforeAnimation = 0;

//	int custumStartNumber = 0;

//	   private Vector2 scrollPos;

//	enum Tab
//	{
//		技設定,
//		当たり判定,
//	}
//	private Tab _tab = Tab.当たり判定;

//	private void OnGUI()
//	{
//		if (initFlag)
//		{
//			Init();
//			initFlag = false;
//		}
//		//ゲームオブジェクトフィールド
//		gameObject = EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true) as GameObject;
//		//アニメーション再生バーの表示
//		BarDraw();


//		//ゲームオブジェクトが入れ替わったとき
//		if (_beforeGameObject != gameObject)
//		{
//			Init();
//		}
//		//現在セットされているアニメーションを表示
//		nowAnimation = EditorGUILayout.Popup(nowAnimation, animationNames.ToArray());
//		//タブ表示
//		using (new EditorGUILayout.HorizontalScope())
//		{
//			GUILayout.FlexibleSpace();
//			// タブを描画する
//			_tab = (Tab)GUILayout.Toolbar((int)_tab, Styles.TabToggles, Styles.TabButtonStyle, Styles.TabButtonSize);
//			GUILayout.FlexibleSpace();
//		}
//		//ゲームオブジェクト変更時
//		if (_beforeAnimation != nowAnimation)
//		{
//			value = 0;
//			_beforeAnimation = nowAnimation;
//			rightValue = (int)(animations[nowAnimation].frameRate * animations[nowAnimation].length);
//		}
//		//ゲームオブジェクトがnullでなくanimationPlayerがnullの時
//		if (gameObject != null)
//		{
//			if (animationPlayer == null)
//			{
//				Init();
//			}
//		}

//		if (_tab == Tab.技設定)
//		{
//			SkillSetting();
//		}
//		else if(_tab == Tab.当たり判定)
//		{
//			if (animationPlayer != null && gameObject!=null)
//			{
//				if (GUILayout.Button("当たり判定作成"))
//				{
//					//Custum
//					if(nowAnimation>=custumStartNumber)
//					{
//						GameObject o = new GameObject();
//						o.transform.parent = gameObject.transform;
//						o.name = "HitBox" + nowAnimation;
//						animationPlayer.custumSkills[nowAnimation - custumStartNumber].hitBoxObjects.Add(o);
//					}
//				}
//			}
//		}
//		if (animationPlayer == null)
//		{
//			return;
//		}
//		if (!EditorApplication.isPlaying)
//		{
//			//アニメーションの再生
//			if (gameObject != null && animationPlayer != null)
//			{
//				animations[nowAnimation].SampleAnimation(gameObject, (float)value / animations[nowAnimation].frameRate);
//			}
//		}
//	}
//	   private void OnEnable()
//	   {
//	       Init();
//	   }
//	private void SkillSetting()
//	{
//		if (animationPlayer != null)
//		{
//			scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
//			skillSerializedObject.Update();
//			nomalReorderable.DoLayoutList();
//			skillSerializedObject.ApplyModifiedProperties();

//			skillSerializedObject.Update();
//			custumReorderable.DoLayoutList();
//			skillSerializedObject.ApplyModifiedProperties();
//			EditorGUILayout.EndScrollView();

//		}
//	}
//	   //数値入力とバーの表示
//	   private void BarDraw()
//	{
//		//数値入力とバーの表示
//		value = EditorGUILayout.IntField(value);
//		value = (int)GUILayout.HorizontalSlider(value, leftValue, rightValue,
//				"box", "box", GUILayout.Height(40), GUILayout.ExpandWidth(true));
//		//最大値最小値
//		if (value > rightValue)
//		{
//			value = rightValue;
//		}
//		else if (value < leftValue)
//		{
//			value = leftValue;
//		}
//	}
//	   //初期化処理
//	   private void Init()
//	   {
//		animationPlayer = null;
//	       if (gameObject != null)
//	       {
//	           //AnimationPlayerがついていれば
//	           if (gameObject.GetComponent(typeof(AnimationPlayer)) != null || gameObject.transform.GetChild(0).GetComponent(typeof(AnimationPlayer)) != null)
//	           {
//	               if (gameObject.transform.GetChild(0).GetComponent(typeof(AnimationPlayer)) != null)
//	               {
//	                   gameObject = gameObject.transform.GetChild(0).gameObject;
//	               }
//	               //animationPlayerの格納
//	               animationPlayer = gameObject.GetComponent(typeof(AnimationPlayer)) as AnimationPlayer;

//	               //animationClipと名前を格納
//	               int i = 0;
//	               animations = new List<AnimationClip>();
//	               animationNames = new List<string>();
//	               foreach (AnimationPlayer.NomalAnim anim in animationPlayer.nomalSkills)
//	               {
//	                   animations.Add(anim.animationClip);
//	                   animationNames.Add(anim.characterAnimation.ToString() + i.ToString());
//	                   i++;
//	               }
//				custumStartNumber = i;
//	               foreach (AnimationPlayer.SkillAnim anim in animationPlayer.custumSkills)
//	               {
//	                   animations.Add(anim.animationClip);
//	                   animationNames.Add(anim.skillName + i.ToString());
//	                   i++;
//	               }
//	               //最大値の設定
//	               value = 0;
//	               nowAnimation = 0;
//	               rightValue = (int)(animations[0].frameRate * animations[0].length);
//	           }
//	       }
//	       if (gameObject == null)
//	       {
//	           animationPlayer = null;
//	       }
//	       _beforeGameObject = gameObject;

//		if (animationPlayer == null)
//		{
//			return;
//		}
//	       skillSerializedObject = new SerializedObject(animationPlayer);

//	       //ノーマルアニメーションのリストの表示
//	       nomalSerializedProperty = skillSerializedObject.FindProperty("nomalSkills");
//	       nomalReorderable = new ReorderableList(skillSerializedObject, nomalSerializedProperty);
//	       nomalReorderable.drawElementCallback = (rect, index, isActive, isFocused) =>
//	       {
//	           var element = nomalSerializedProperty.GetArrayElementAtIndex(index);
//	           var el = element.FindPropertyRelative("animationClip");
//	           Rect rec = new Rect(rect.x, rect.y, rect.width / 2.0f - 20, rect.height - 4);
//	           if (rec.width < 20)
//	           {
//	               rec.width = 20;
//	           }
//	           CharacterAnimation character = animationPlayer.nomalSkills[index].characterAnimation;
//	           animationPlayer.nomalSkills[index].characterAnimation = (CharacterAnimation)EditorGUI.EnumPopup(rec, animationPlayer.nomalSkills[index].characterAnimation);
//	           rec.x += rec.width;
//	           rec.width = rect.width / 2.0f;
//	           if (rec.width + rec.x > rect.width)
//	           {
//	               rec.width = rect.width - rec.x;
//	           }
//	           AnimationClip clip = el.objectReferenceValue as AnimationClip;
//	           EditorGUI.ObjectField(rec, el, typeof(AnimationClip), new GUIContent(""));
//	           if (character != animationPlayer.nomalSkills[index].characterAnimation || clip != (AnimationClip)el.objectReferenceValue)
//	           {
//	               initFlag = true;
//	           }
//	       };
//	       //要素が変わった時
//	       nomalReorderable.onChangedCallback = (list) => { initFlag = true; };
//	       //ヘッダー
//	       nomalReorderable.drawHeaderCallback = (rec) => { EditorGUI.LabelField(rec, "通常スキル"); };

//	       //カスタムアニメーションのリストの表示
//	       custumSerializedProperty = skillSerializedObject.FindProperty("custumSkills");
//	       custumReorderable = new ReorderableList(skillSerializedObject, custumSerializedProperty);
//	       custumReorderable.drawElementCallback = (rect, index, isActive, isFocused) =>
//	       {
//	           var element = custumSerializedProperty.GetArrayElementAtIndex(index);
//	           var el = element.FindPropertyRelative("animationClip");
//	           var eltex = element.FindPropertyRelative("skillName");
//	           Rect rec = new Rect(rect.x, rect.y, rect.width / 2.0f - 20, rect.height - 4);
//	           if (rec.width < 20)
//	           {
//	               rec.width = 20;
//	           }
//	           string s = eltex.stringValue;
//	           eltex.stringValue = EditorGUI.TextField(rec, eltex.stringValue);
//	           rec.x += rec.width;
//	           rec.width = rect.width / 2.0f;
//	           if (rec.width + rec.x > rect.width)
//	           {
//	               rec.width = rect.width - rec.x;
//	           }
//	           AnimationClip clip = el.objectReferenceValue as AnimationClip;
//	           EditorGUI.ObjectField(rec, el, typeof(AnimationClip), new GUIContent(""));
//	           if (s != eltex.stringValue || clip != (AnimationClip)el.objectReferenceValue)
//	           {
//	               initFlag = true;
//	           }
//	       };
//	       //要素が変わった時
//	       custumReorderable.onChangedCallback = (list) => { initFlag = true; };
//	       //ヘッダー
//	       custumReorderable.drawHeaderCallback = (rec) =>
//	       {
//	           EditorGUI.LabelField(rec, "カスタムスキル");
//	       };
//	   }
//}

