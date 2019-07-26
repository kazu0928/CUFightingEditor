#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

public class PlayerSkillEditorParameter : ScriptableSingleton<PlayerSkillEditorParameter>
{
	public PlayerSkillEditor window;
}

public class PlayerSkillEditor : EditorWindow
{
	public FighterSkill playerSkill = null;
	public GameObject previewCharacter;
	public GameObject _beforeCharacter = null;
	private Vector2 scrollPos;

	#region ウィンドウオープン
	public static void Open(FighterSkill ps)
	{
		if (PlayerSkillEditorParameter.instance.window == null)
		{
			PlayerSkillEditorParameter.instance.window = (PlayerSkillEditor)CreateInstance(typeof(PlayerSkillEditor));
			PlayerSkillEditorParameter.instance.window.playerSkill = ps;
			PlayerSkillEditorParameter.instance.window.Show();
		}
		else
		{
			PlayerSkillEditorParameter.instance.window.playerSkill = ps;
		}
	}
	#endregion
	private void OnGUI()
	{
		EditorGUILayout.BeginHorizontal();
		CustomLabel(playerSkill.name, Color.white, Color.gray, 20, FontStyle.Italic);
		if (GUILayout.Button("保存", GUILayout.Height(50), GUILayout.Width(80)))
		{
			//ダーティとしてマークする(変更があった事を記録する)
			EditorUtility.SetDirty(playerSkill);

			//保存する
			AssetDatabase.SaveAssets();
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("プレビュー用キャラプレハブ");
		previewCharacter = (EditorGUILayout.ObjectField(previewCharacter, typeof(GameObject), true) as GameObject);
		if (_beforeCharacter != previewCharacter)
		{
			if (previewCharacter != null)
			{
				if (previewCharacter.GetComponent(typeof(FighterCore)) != null)
				{
                    previewCharacter = previewCharacter.GetComponent<FighterCore>().PlayerModel;
                }
				if (previewCharacter.GetComponent(typeof(AnimationPlayerBase)) == null)
				{
					previewCharacter = _beforeCharacter;
				}
			}
			_beforeCharacter = previewCharacter;
		}
		EditorGUILayout.EndVertical();
		BarDraw();
		TabDraw();
		scrollPos = GUILayout.BeginScrollView(scrollPos);
		//Undoに対応
		Undo.RecordObject(playerSkill, "fighterStatus");
		switch (_tab)
		{
			case Tab.アニメーション:
				AnimationTabDraw();
				break;
			case Tab.当たり判定:
				HitBoxTabDraw();
				break;
			case Tab.移動速度設定:
				MoveSettingDraw();
				break;
		}
		GUILayout.EndScrollView();
		AnimationPlayFrame();
        //エディタ全体の再描画
        EditorApplication.QueuePlayerLoopUpdate();
	}

    #region タブ_Styles
    private enum Tab
	{
		アニメーション,
		当たり判定,
		移動速度設定,
	}
	private Tab _tab = Tab.アニメーション;
	// Style定義
	private static class Styles
	{
		private static GUIContent[] _tabToggles = null;
		public static GUIContent[] TabToggles
		{
			get
			{
				if (_tabToggles == null)
				{
					_tabToggles = System.Enum.GetNames(typeof(Tab)).Select(x => new GUIContent(x)).ToArray();
				}
				return _tabToggles;
			}
		}

		public static readonly GUIStyle TabButtonStyle = "LargeButton";

		// GUI.ToolbarButtonSize.FitToContentsも設定できる
		public static readonly GUI.ToolbarButtonSize TabButtonSize = GUI.ToolbarButtonSize.Fixed;
	}
	private void TabDraw()
	{
		//タブ表示
		using (new EditorGUILayout.HorizontalScope())
		{
			GUILayout.FlexibleSpace();
			// タブを描画する
			_tab = (Tab)GUILayout.Toolbar((int)_tab, Styles.TabToggles, Styles.TabButtonStyle, Styles.TabButtonSize);
			GUILayout.FlexibleSpace();
		}
	}
	#endregion
	#region アニメーション_Tab
	private void AnimationTabDraw()
	{
		EditorGUILayout.BeginVertical("Box");
		//アニメーションクリップ
		EditorGUILayout.BeginHorizontal("Box");
		EditorGUILayout.LabelField("AnimationClip");
		playerSkill.animationClip = EditorGUILayout.ObjectField(playerSkill.animationClip, typeof(AnimationClip), true) as AnimationClip;
		EditorGUILayout.EndHorizontal();
		if (playerSkill.animationClip != null)
		{
			//アニメーション速度
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("アニメーション速度：");
			playerSkill.animationSpeed = EditorGUILayout.FloatField(playerSkill.animationSpeed);
			EditorGUILayout.EndVertical();
			//フレーム数
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("フレーム数：");
			if (playerSkill.animationSpeed == 0)
			{
				EditorGUILayout.LabelField("0");
			}
			else
			{
				EditorGUILayout.LabelField(((int)((playerSkill.animationClip.length * playerSkill.animationClip.frameRate) / playerSkill.animationSpeed)).ToString());
			}
			EditorGUILayout.EndHorizontal();
			//ブレンド
			EditorGUILayout.BeginHorizontal();
			playerSkill.inBlend = EditorGUILayout.Toggle("再生時ブレンド",playerSkill.inBlend);
			playerSkill.outBlend = EditorGUILayout.Toggle("終了時ブレンド",playerSkill.outBlend);
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical("Box");
        playerSkill.status = (SkillStatus)EditorGUILayout.EnumPopup("属性", playerSkill.status);
        playerSkill.hitMode = (HitMode)EditorGUILayout.EnumPopup("ヒットモード", playerSkill.hitMode);
		if(playerSkill.hitMode == HitMode.Grab)
		{
            EditorGUILayout.BeginVertical("Box");
            playerSkill.throwMotion = (AnimationClip)EditorGUILayout.ObjectField("投げモーション",playerSkill.throwMotion,typeof(AnimationClip),false);
            playerSkill.enemyThrowMotion = (AnimationClip)EditorGUILayout.ObjectField("敵投げられモーション", playerSkill.enemyThrowMotion, typeof(AnimationClip), false);
            EditorGUILayout.EndVertical();
        }
        playerSkill.cancelFrag = (SkillStatus)EditorGUILayout.EnumFlagsField("キャンセル属性", playerSkill.cancelFrag);
        EditorGUILayout.BeginHorizontal();
        playerSkill.barrageCancelFrag = EditorGUILayout.Toggle("連打キャンセル", playerSkill.barrageCancelFrag);
        playerSkill.cancelLayer = EditorGUILayout.IntField("キャンセルレイヤー",playerSkill.cancelLayer);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }
    #endregion
    #region 当たり判定_Tab
	private class FoldOutFlags
	{
		public bool foldOutFlag = false;
        public bool statusFlag = false;
        public bool effectFlag = false;
    }
    private List<FoldOutFlags> foldOutFlags = new List<FoldOutFlags>();
	private void HitBoxTabDraw()
	{
		if (playerSkill != null)
		{
			EditorGUILayout.BeginHorizontal();
			playerSkill.headFlag = EditorGUILayout.Toggle("Head",playerSkill.headFlag);
			playerSkill.bodyFlag = EditorGUILayout.Toggle("Body",playerSkill.bodyFlag);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			playerSkill.footFlag = EditorGUILayout.Toggle("Foot",playerSkill.footFlag);
            playerSkill.grabFlag = EditorGUILayout.Toggle("Grab", playerSkill.grabFlag);
			EditorGUILayout.EndHorizontal();
			playerSkill.pushingFlag = EditorGUILayout.Toggle("Pushing", playerSkill.pushingFlag);
		}
		if (playerSkill.headFlag) HitBoxSetting(HitBoxPosition.Head);
		if (playerSkill.bodyFlag) HitBoxSetting(HitBoxPosition.Body);
		if (playerSkill.footFlag) HitBoxSetting(HitBoxPosition.Foot);
        if (playerSkill.grabFlag) HitBoxSetting(HitBoxPosition.Grab);
		if (playerSkill.pushingFlag) HitBoxSetting(HitBoxPosition.Pushing);
        int i = 0;
        if (GUILayout.Button("当たり判定作成", GUILayout.Width(100),GUILayout.Height(30)))
        {
            playerSkill.customHitBox.Add(new FighterSkill.CustomHitBox());
        }
        List<int> removeNumber = new List<int>();
        //カスタム当たり判定設定
        foreach (FighterSkill.CustomHitBox box in playerSkill.customHitBox)
        {
            if (foldOutFlags.Count < i + 1)
            {
                while (foldOutFlags.Count < i + 1)
                {
                    foldOutFlags.Add(new FoldOutFlags());
                }
            }
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.BeginHorizontal();
            //mode選択
            box.mode = (HitBoxMode)EditorGUILayout.EnumPopup(box.mode);
            if (GUILayout.Button("×", GUILayout.Width(20))) removeNumber.Add(i);
            EditorGUILayout.EndHorizontal();
            bool temp = foldOutFlags[i].foldOutFlag;
            //FoldOut
            foldOutFlags[i].foldOutFlag = FoldOutHitBox(box.frameHitBoxes, i.ToString(), ref temp, box);
            if (box != null)
            {
                if ((foldOutFlags[i].foldOutFlag) && (foldOutFlags[i].statusFlag = CustomUI.Foldout("ステータス", foldOutFlags[i].statusFlag)))
                {
                    if (box.mode == HitBoxMode.HitBox)
                    {
                        EditorGUILayout.BeginHorizontal();
                        box.hitPoint = (HitPoint)EditorGUILayout.EnumPopup("上中下", box.hitPoint);
                        box.hitStrength = (HitStrength)EditorGUILayout.EnumPopup("強弱", box.hitStrength);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        box.isDown = EditorGUILayout.Toggle("ダウン", box.isDown);
                        box.hitStop = EditorGUILayout.IntField("ヒットストップ値", box.hitStop);
                        EditorGUILayout.EndHorizontal();
                        if (box.isDown)
                        {
                            EditorGUILayout.BeginHorizontal();
                            box.isFaceDown = EditorGUILayout.Toggle("うつ伏せダウン", box.isFaceDown);
                            box.isPassiveNotPossible = EditorGUILayout.Toggle("受け身不可", box.isPassiveNotPossible);
                            EditorGUILayout.EndHorizontal();
                        }
                        EditorGUILayout.BeginHorizontal();
                        box.damage = EditorGUILayout.IntField("ダメージ量", box.damage);
                        box.stanDamage = EditorGUILayout.IntField("スタン値", box.stanDamage);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        box.knockBack = EditorGUILayout.FloatField("ノックバック値", box.knockBack);
                        box.plusGauge = EditorGUILayout.IntField("ゲージ増加量", box.plusGauge);
                        EditorGUILayout.EndHorizontal();
                        if (!box.isDown)
                        {
                            EditorGUILayout.BeginHorizontal();
                            box.hitRigor = EditorGUILayout.IntField("ヒット硬直", box.hitRigor);
                            box.guardHitRigor = EditorGUILayout.IntField("ガード硬直", box.guardHitRigor);
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                if ((foldOutFlags[i].foldOutFlag) && (foldOutFlags[i].effectFlag = CustomUI.Foldout("エフェクト", foldOutFlags[i].effectFlag)))
                {
                    EditorGUILayout.BeginVertical("Box");
                    if (box.mode == HitBoxMode.HitBox)
                    {
                        if (GUILayout.Button("ヒットエフェクト作成", GUILayout.Width(150), GUILayout.Height(20)))
                        {
                            box.hitEffects.Add(new FighterSkill.HitEffects());
                        }
                        for (int ef = 0; ef < box.hitEffects.Count; ef++)
                        {
                            EditorGUILayout.BeginVertical("Box");
                            //削除
                            bool f = false;
                            EditorGUILayout.BeginHorizontal();
                            box.hitEffects[ef].effect = EditorGUILayout.ObjectField("エフェクト", box.hitEffects[ef].effect, typeof(GameObject), true) as GameObject;
                            if (GUILayout.Button("×", GUILayout.Width(20)))
                            {
                                f = true;
                            }
                            EditorGUILayout.EndHorizontal();
                            box.hitEffects[ef].position = EditorGUILayout.Vector3Field("ポジション", box.hitEffects[ef].position);
                            EditorGUILayout.EndHorizontal();
                            if (f)
                            {
                                box.hitEffects.Remove(box.hitEffects[ef]);
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }

            }

            i++;
            EditorGUILayout.EndVertical();
        }
        //削除
        for(int j = 0; j<removeNumber.Count; j++)
        {
            playerSkill.customHitBox.RemoveAt(removeNumber[j]);
        }
	}
	private bool headFold;
	private bool bodyFold;
	private bool footFold;
    private bool grabFold;
	private bool pushingFold;
    //デフォルトヒットBox
	private void HitBoxSetting(HitBoxPosition? eHitBox = null)
	{
		EditorGUILayout.BeginVertical("Box");
		switch (eHitBox)
		{
			case HitBoxPosition.Head:
                FoldOutHitBox(playerSkill.plusHeadHitBox, "Head_Default", ref headFold);
				break;
			case HitBoxPosition.Body:
                FoldOutHitBox(playerSkill.plusBodyHitBox, "Body_Default", ref bodyFold);
                break;
			case HitBoxPosition.Foot:
                FoldOutHitBox(playerSkill.plusFootHitBox, "Foot_Default", ref footFold);
                break;
            case HitBoxPosition.Grab:
                FoldOutHitBox(playerSkill.plusGrabHitBox, "Grab_Default", ref grabFold);
                break;
			case HitBoxPosition.Pushing:
				FoldOutHitBox(playerSkill.plusPushingHitBox, "Push_Default", ref pushingFold);
                break;
		}
		EditorGUILayout.EndVertical();
	}
    //ヒットボックス個々（FoldOutした中身）
	private bool FoldOutHitBox(List<FighterSkill.FrameHitBox> frameHitBox,string label,ref bool frag,FighterSkill.CustomHitBox cus = null)
	{
		if (frag = CustomUI.Foldout(label, frag))
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(frameHitBox.Count.ToString());
			if (GUILayout.Button("判定作成",GUILayout.Width(80)))
			{
				frameHitBox.Add(new FighterSkill.FrameHitBox());
			}
			EditorGUILayout.EndHorizontal();
			//スライダーその他の作成
			for(int i = 0; i<frameHitBox.Count; i++)
			{
				EditorGUILayout.BeginVertical("Box");
				//数値の入れ替え
				float frameStart = frameHitBox[i].startFrame;
				float frameEnd = frameHitBox[i].endFrame;
				EditorGUILayout.BeginHorizontal();
				//左
				frameStart = (int)EditorGUILayout.FloatField(frameStart,GUILayout.Width(30));
				if (frameStart > frameEnd) frameStart = frameEnd - 1;
				if (frameStart < 0) frameStart = 0;
				//スライダー
				EditorGUILayout.MinMaxSlider(ref frameStart, ref frameEnd, 0, rightValue);
				//右
				frameEnd = (int)EditorGUILayout.FloatField(frameEnd,GUILayout.Width(30));
				if (frameEnd < frameStart) frameEnd = frameEnd - 1;
				if (frameEnd > rightValue) frameEnd = rightValue;
                //削除
                bool f = false;
                if (GUILayout.Button("×", GUILayout.Width(20)))
                {
                    f = true;
                }
                EditorGUILayout.EndHorizontal();

                frameHitBox[i].startFrame = (int)frameStart;
				frameHitBox[i].endFrame = (int)frameEnd;

				//当たり判定の設定
				HitFoldOut(frameHitBox[i]);

                EditorGUILayout.EndVertical();
                if(f)
                {
                    frameHitBox.Remove(frameHitBox[i]);
                }
                
            }
		}
        return frag;
	}

    //当たり判定
    private void HitFoldOut(FighterSkill.FrameHitBox hitBox_)
	{
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("Position");
		hitBox_.hitBox.localPosition.x = EditorGUILayout.FloatField("X", hitBox_.hitBox.localPosition.x);
		hitBox_.hitBox.localPosition.y = EditorGUILayout.FloatField("Y", hitBox_.hitBox.localPosition.y);
		hitBox_.hitBox.localPosition.z = EditorGUILayout.FloatField("Z", hitBox_.hitBox.localPosition.z);
		EditorGUILayout.EndVertical();
		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.LabelField("サイズ");
		hitBox_.hitBox.size.x = EditorGUILayout.FloatField("X", hitBox_.hitBox.size.x);
		hitBox_.hitBox.size.y = EditorGUILayout.FloatField("Y", hitBox_.hitBox.size.y);
		hitBox_.hitBox.size.z = EditorGUILayout.FloatField("Z", hitBox_.hitBox.size.z);
		EditorGUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();

    }

	#endregion
	#region 移動速度設定_Tab
	private void MoveSettingDraw()
	{
		playerSkill.isContinue = EditorGUILayout.Toggle("制動継続", playerSkill.isContinue);
		if(GUILayout.Button("移動作成",GUILayout.Width(80)))
		{
			playerSkill.movements.Add(new FighterSkill.Move());
		}
		for (int i = 0; i < playerSkill.movements.Count; i++)
		{
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			bool removeFrag = false;
			playerSkill.movements[i].startFrame = EditorGUILayout.IntField("スタートフレーム",playerSkill.movements[i].startFrame);
			//削除ボタン
		   if(GUILayout.Button("×",GUILayout.Width(20)))
			{
				removeFrag = true;
			}
		   //削除
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical("Box");
			//Vector3入力
			playerSkill.movements[i].movement = EditorGUILayout.Vector3Field("移動量", playerSkill.movements[i].movement);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
			if (removeFrag) playerSkill.movements.RemoveAt(i);
		}
		if (GUILayout.Button("制動作成", GUILayout.Width(80)))
		{
			playerSkill.gravityMoves.Add(new FighterSkill.GravityMove());
		}
		for (int i = 0; i < playerSkill.gravityMoves.Count; i++)
		{
			EditorGUILayout.BeginVertical("Box");
			EditorGUILayout.BeginHorizontal();
			bool removeFrag = false;
			playerSkill.gravityMoves[i].startFrame = EditorGUILayout.IntField("スタートフレーム", playerSkill.gravityMoves[i].startFrame);
			//削除ボタン
			if (GUILayout.Button("×", GUILayout.Width(20)))
			{
				removeFrag = true;
			}
			//削除
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical("Box");
			//Vector3入力
			playerSkill.gravityMoves[i].movement = EditorGUILayout.Vector3Field("移動量", playerSkill.gravityMoves[i].movement);
			//Vector3入力
			playerSkill.gravityMoves[i].limitMove = EditorGUILayout.Vector3Field("移動量限界", playerSkill.gravityMoves[i].limitMove);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
			if (removeFrag) playerSkill.gravityMoves.RemoveAt(i);
		}
	}
	#endregion
	#region バー_BarDraw()
	public int value = 0;//現在の位置
    int rightValue = 0;//最大値
    int leftValue = 0;//最小値

    //数値入力とバーの表示
    private void BarDraw()
    {
		if (playerSkill.animationClip != null)
		{
			rightValue = (int)(((int)(playerSkill.animationClip.length * playerSkill.animationClip.frameRate)) / playerSkill.animationSpeed);
		}
        //数値入力とバーの表示
        value = EditorGUILayout.IntField(value);
        value = (int)GUILayout.HorizontalSlider(value, leftValue, rightValue,
                "box", "box", GUILayout.Height(40), GUILayout.ExpandWidth(true));
        //最大値最小値
        if (value > rightValue)
        {
            value = rightValue;
        }
        else if (value < leftValue)
        {
            value = leftValue;
        }
    }
    #endregion
    #region アニメーション再生
    private void AnimationPlayFrame()
    {
        if (!EditorApplication.isPlaying)
        {
            //アニメーションの再生
            if (previewCharacter != null && playerSkill != null && playerSkill.animationClip != null)
            {
                playerSkill.animationClip.SampleAnimation(previewCharacter, ((float)value / playerSkill.animationClip.frameRate) * playerSkill.animationSpeed );
            }
        }
    }
    #endregion
    #region カスタムラベル_CustomLabel()
    void CustomLabel(string text, Color textColor, Color backColor, int fontSize, FontStyle fontStyle = FontStyle.Bold)
    {
        Color beforeBackColor = GUI.backgroundColor;

        GUIStyle guiStyle = new GUIStyle();
        GUIStyleState styleState = new GUIStyleState();

        styleState.textColor = textColor;
        styleState.background = Texture2D.whiteTexture;
        GUI.backgroundColor = backColor;
        guiStyle.normal = styleState;
        guiStyle.fontSize = fontSize;
        GUILayout.Label(text, guiStyle); //labelFieldだとうまくいかない

        GUI.backgroundColor = beforeBackColor;
    }
    #endregion

}
#endif

