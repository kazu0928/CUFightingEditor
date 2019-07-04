//===============================================================
// ファイル名：NextState.cs
// 作成者    ：村上一真
// 作成日　　：20190516
// 次のステートの情報、入れ替わる条件などを格納した情報群。
// エディタ拡張との掛け合いでインスタンス化などの影響があるため、
// 構造体ではなくクラスとして作成。リストとして使用
//===============================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;

namespace CUEngine.Pattern
{
    public enum JudgeFuncArgMode
    {
        NoArg,
        Arg1,
        Arg2,
    }

    public enum ArgType
    {
        UnityObject,
        Int,
        Float,
        String,
        Bool,
        Enum,
    }

    [System.Serializable]
    public class NextStateJudge
    {
        // シリアライズ化されない情報があるため、初期化処理を実行する必要がある
        public void Init()
        {
            if (argMode == JudgeFuncArgMode.NoArg)
            {
                judgeFunc = Activator.CreateInstance(typeof(FuncBool)) as DelegateBoolBase;
                typeof(FuncBool).GetMethod("Init").Invoke(judgeFunc, new object[] { judgeFuncInstance, judgeFuncName });
            }
            else if (argMode == JudgeFuncArgMode.Arg1)
            {
                //一つ目の引数のタイプ（Typeがシリアル化不可のため）
                Type type1 = typeof(object);
                switch (argType1)
                {
                    case ArgType.UnityObject:
                        type1 = arg1.GetTypeArg(ArgType.UnityObject).GetType();
                        break;
                    case ArgType.Bool:
                        type1 = typeof(bool);
                        break;
                    case ArgType.Float:
                        type1 = typeof(float);
                        break;
                    case ArgType.Int:
                        type1 = typeof(int);
                        break;
                    case ArgType.String:
                        type1 = typeof(string);
                        break;
                    case ArgType.Enum:
                        type1 = typeof(int);
                        break;
                }

                Type type = typeof(FuncBool<>).MakeGenericType(type1);
                judgeFunc = Activator.CreateInstance(type) as DelegateBoolBase;
                //delegate作成
                if(argType1==ArgType.Enum)
                {
                    type.GetMethod("Init").Invoke(judgeFunc, new object[] { judgeFuncInstance, judgeFuncName, arg1.GetTypeArg(argType1),arg1.GetEnumType(),arg1.assembly_Name });
                }
                else
                {
                    type.GetMethod("Init").Invoke(judgeFunc, new object[] { judgeFuncInstance, judgeFuncName, arg1.GetTypeArg(argType1),null,null });
                }
            }
            else if (argMode == JudgeFuncArgMode.Arg2)
            {
                //一つ目の引数のタイプ（Typeがシリアル化不可のため）
                Type type1 = typeof(object);
                switch (argType1)
                {
                    case ArgType.UnityObject:
                        type1 = arg1.GetTypeArg(ArgType.UnityObject).GetType();
                        break;
                    case ArgType.Bool:
                        type1 = typeof(bool);
                        break;
                    case ArgType.Float:
                        type1 = typeof(float);
                        break;
                    case ArgType.Int:
                        type1 = typeof(int);
                        break;
                    case ArgType.String:
                        type1 = typeof(string);
                        break;
                    case ArgType.Enum:
                        type1 = typeof(int);
                        break;
                }
                //二つ目の引数のタイプ（Typeがシリアル化不可のため）
                Type type2 = typeof(object);
                switch (argType2)
                {
                    case ArgType.UnityObject:
                        type2 = arg1.GetTypeArg(ArgType.UnityObject).GetType();
                        break;
                    case ArgType.Bool:
                        type2 = typeof(bool);
                        break;
                    case ArgType.Float:
                        type2 = typeof(float);
                        break;
                    case ArgType.Int:
                        type2 = typeof(int);
                        break;
                    case ArgType.String:
                        type2 = typeof(string);
                        break;
                    case ArgType.Enum:
                        type2 = typeof(int);
                        break;
                }
                Type type = typeof(FuncBool<,>).MakeGenericType(type1, type2);
                judgeFunc = Activator.CreateInstance(type) as DelegateBoolBase;
                string enum1 = null, enum2 = null, enumAs1 = null, enumAs2 = null;
                if (argType1 == ArgType.Enum)
                {
                    enum1 = arg1.enum_Name;
                    enumAs1 = arg1.assembly_Name;
                }
                if (argType2 == ArgType.Enum)
                {
                    enum2 = arg2.enum_Name;
                    enumAs2 = arg2.assembly_Name;
                }
                //delegate作成
                type.GetMethod("Init").Invoke(judgeFunc, new object[] { judgeFuncInstance, judgeFuncName, arg1.GetTypeArg(argType1), arg2.GetTypeArg(argType2),enum1,enum2,enumAs1,enumAs2 });
            }
        }
        //次に入れ替わることの出来るステートの名前
        public string nextStateName;
        //次に入れ替わるステート本体(シリアライズ化不可)
        [NonSerialized] 
        public StateBase nextState;
        //次に入れ替わるステートのID
        public InstanceID nextID = new InstanceID();
        //次に入れ替わるステートへ移行する戻り値boolの条件式、デリゲート(シリアライズ化不可)
        //TODO
        public DelegateBoolBase judgeFunc;
        //引数
        public Argument arg1 = new Argument();
        public Argument arg2 = new Argument();
        //引数がいくつか
        public JudgeFuncArgMode argMode = JudgeFuncArgMode.NoArg;
        //引数のタイプ
        public ArgType argType1;
        public ArgType argType2;
        //public Func<bool> judgeFunc;
        public string judgeFuncName = null;                 //上記のデリゲートに入れるメソッドの名前
        public UnityEngine.Object judgeFuncInstance = null; //上記のデリゲートに入れるメソッドのあるクラスのインスタンス（コンポーネント）
                                                            //次に入れ替わるステートの優先度、優先度順で移行される
        public int priority;
        public string comment;

    }
    //引数のタイプ
    [System.Serializable]
    public class Argument
    {
        public UnityEngine.Object object_Arg = null;
        public int int_Arg;
        public float float_Arg;
        public string string_Arg;
        public bool bool_Arg;
        public int enum_Arg;
        public string enum_Name;
        public string assembly_Name;

        public object GetTypeArg(ArgType type)
        {
            switch (type)
            {
                case ArgType.UnityObject:
                    return object_Arg;
                case ArgType.Bool:
                    return bool_Arg;
                case ArgType.Float:
                    return float_Arg;
                case ArgType.Int:
                    return int_Arg;
                case ArgType.String:
                    return string_Arg;
                case ArgType.Enum:
                    return enum_Arg;
            }
            return null;
        }
        public string GetEnumType()
        {
            return enum_Name;
        }
    }
}
