using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentObjectPool<T>　where T : Component
{
    private List<GameObject> poolObjList;
	private List<T> componentList;
    public GameObject parantObject = null;
    //返す用
    public struct Objs
    {
        public GameObject gameObject;
        public T component;
    }
    //オブジェクトプールの作成
    public ComponentObjectPool(int maxCount, string name, GameObject parant = null)
    {
        //オブジェクトプール用親オブジェクト生成
        var obj = new GameObject();
        if(parant!=null)obj.transform.parent = parant.transform;//親子付け
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.name = name;
        //リストの初期化
        poolObjList = new List<GameObject>();
        componentList = new List<T>();
        //親の設定
        parantObject = obj;
        parantObject.transform.position = parant.transform.position;
        parantObject.transform.rotation = parant.transform.rotation;
        //カウント分オブジェクト生成
        for (int i = 0; i < maxCount; i++)
        {
            GameObject newObj = CreateNewObject();
            poolObjList.Add(newObj);
            componentList.Add(newObj.GetComponent<T>());
            newObj.SetActive(false);
        }
    }
    //新しいゲームオブジェクトの生成して返す
	private GameObject CreateNewObject()
	{
		GameObject newObj = new GameObject();
        //componentの名前
		newObj.name = typeof(T).Name + (poolObjList.Count + 1);
		T com = newObj.AddComponent(typeof(T)) as T;
		newObj.transform.parent = parantObject.transform;
        newObj.transform.localPosition = Vector3.zero;
        newObj.transform.localRotation = Quaternion.identity;
        return newObj;
	}

    public Objs GetObjects()
    {
        // 使用中でないものを探して返す
		for(int i =0;i< poolObjList.Count;i++)
		{
			if (poolObjList[i].activeSelf == false)
			{
				Objs objs = new Objs();
				poolObjList[i].SetActive(true);
				objs.gameObject = poolObjList[i];
				objs.component = componentList[i];
				return objs;
			}
		}
        //全て使用中だったら新しく作る
        GameObject newObj = CreateNewObject();
        Objs o = new Objs();
        o.gameObject = newObj;
        newObj.SetActive(true);
        poolObjList.Add(newObj);
        componentList.Add(newObj.GetComponent<T>());
        return o;
    }
}
