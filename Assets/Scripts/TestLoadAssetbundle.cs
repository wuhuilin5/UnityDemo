using UnityEngine;
using System.Collections;

using UnityDemo;
using UnityDemo.Utils;
using UnityDemo.manager;
using UnityDemo.interfaces.manager;

public class TestLoadAssetbundle : MonoBehaviour {

	public UILabel mDataPath;

	// Use this for initialization
	private ILoadManger mLoadManager;

	void Awake() {
		Globals.Api.LogLable = mDataPath;
		mLoadManager = Globals.Api.loadManager;
	}

	void Start () {
		LoadAssetBundle("ItemContainer");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LoadAssetBundle(string assetbundleName) {
		string filePath = FileUtils.getAssetBundlePath(assetbundleName);
		Globals.Api.Log(filePath);

		mLoadManager.loadUrl(filePath, loadComplete);
	}

	void loadComplete(AssetBundle asset) {
		Globals.Api.Log("loadComplete 2");
		if( asset == null )
			Globals.Api.Log("asset is null");

		Object prefeb = asset.Load("ItemContainer");
		Globals.Api.Log("aseet load");

		///GameObject obj = NGUITools.AddChild(this.gameObject, prefeb);
		GameObject obj = AddChildToParent(this.gameObject, prefeb);
		obj.name = "ItemContainer-right";

//		Vector3 pos = obj.transform.position;
//		obj.transform.position = new Vector3(-240, pos.y, pos.z);
	}

	GameObject AddChildToParent( GameObject parent, Object prefeb )
	{
		mDataPath.text = "Instance";
		GameObject go = GameObject.Instantiate(prefeb) as GameObject;
		Globals.Api.Log("add to parent");

		if (parent != null && go != null) {
			Transform t = go.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
			go.layer = parent.layer;
		}

		return go;
	}
}
