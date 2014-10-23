using UnityEngine;
using System.Collections;
using System.IO;

using UnityDemo;
using UnityDemo.Utils;
using UnityDemo.manager;
using UnityDemo.interfaces.manager;

public class TestLoadAssetbundle : MonoBehaviour {

	public UILabel mLblLog;
	GameObject mAnchorLeft;
	GameObject mAnchorRight;

	public UITexture mTexture;

	// Use this for initialization
	private ILoadManger mLoadManager;

	void Awake() {
		Globals.Api.LogLable = mLblLog;
		mLoadManager = Globals.Api.loadManager;
	}

	void Start () {
		System.Action<int> callback = (count) => {
			LoadAssetBundle("UIRoot", loadUIRootComplete);
		};

		loadShareAssetBundles(callback);

//		mLoadManager.loadUrl("http://www.baidu.com/img/bd_logo1.png", LoadTextureComplete);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void loadShareAssetBundles( System.Action<int> finishCallback)
	{
		int count = 0;
		LoadFunishHandler callback = delegate(WWW loader) {
			count++;
			if(count==2 && finishCallback != null){
				finishCallback(count);
			}
			loader.assetBundle.LoadAll();
			//GameObject go = GameObject.Instantiate(loader.assetBundle.mainAsset) as GameObject;
		};

		mLoadManager.loadUrl(FileUtils.getAssetBundlePath("Fantasy Atlas"), callback, false);
		mLoadManager.loadUrl(FileUtils.getAssetBundlePath("Wooden Atlas"), callback, false);
	}

	void loadUIRootComplete(WWW loader)
	{
		Object prefeb = loader.assetBundle.Load("UIRoot", typeof(GameObject));
		GameObject go = GameObject.Instantiate(prefeb) as GameObject;
		mAnchorLeft = GameObject.Find("Anchor-left");
		mAnchorRight = GameObject.Find("Anchor-right");

		LoadAssetBundle("ItemContainer", LoadItemContainerComplete );
	}

	void LoadAssetBundle(string assetbundleName, LoadFunishHandler callback) {
		string filePath = FileUtils.getAssetBundlePath(assetbundleName);
		Globals.Api.Log(filePath);
		mLoadManager.loadUrl(filePath, callback, true);
	}

	void LoadTextureComplete(WWW loader)
	{
//		mTexture.mainTexture = loader.texture;
//
//		string filepath = FileUtils.GetAssetPath("logo.jpg");
//		if(!File.Exists(filepath))
//			FileUtils.SaveFile("logo.jpg", loader.bytes);
//		GameObject obj = loader.assetBundle.mainAsset as GameObject;
	}

	void LoadItemContainerComplete(WWW loader) {
		AssetBundle asset = loader.assetBundle;
		Object prefeb = asset.Load("ItemContainer", typeof(GameObject));

		//GameObject obj = NGUITools.AddChild(this.gameObject, prefeb);
		GameObject leftContainer = AddChildToParent(mAnchorLeft, prefeb);
		leftContainer.name = "ItemContainer-left";

		GameObject rightContainer = AddChildToParent(mAnchorRight, prefeb);
		rightContainer.name = "ItemContainer-right";
	}

	GameObject AddChildToParent( GameObject parent, Object prefeb )
	{
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
