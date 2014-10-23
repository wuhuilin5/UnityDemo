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
	private IAssetManager mAssetMgr;

	void Awake() {
		Globals.Api.LogLable = mLblLog;
		mAssetMgr = Globals.Api.AssetManager;
	}

	void Start () {
		System.Action<int> callback = (count) => {
			LoadAssetBundle("UIRoot", loadUIRootComplete);
		};

		loadShareAssetBundles(callback);

//		mAssetMgr.LoadAsset("http://www.baidu.com/img/bd_logo1.png", LoadTextureComplete);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void loadShareAssetBundles( System.Action<int> finishCallback)
	{
		int count = 0;
		LoadFunishHandler callback = delegate(AssetBundle asset) {
			count++;
			if(count==2 && finishCallback != null){
				finishCallback(count);
			}
			asset.LoadAll();
			//GameObject go = GameObject.Instantiate(loader.assetBundle.mainAsset) as GameObject;
		};

		mAssetMgr.LoadAsset(FileUtils.getAssetBundlePath("Fantasy Atlas"), callback);
		mAssetMgr.LoadAsset(FileUtils.getAssetBundlePath("Wooden Atlas"), callback);
	}

	void loadUIRootComplete(AssetBundle asset)
	{
		Object prefeb = asset.Load("UIRoot", typeof(GameObject));
		GameObject go = GameObject.Instantiate(prefeb) as GameObject;
		mAnchorLeft = GameObject.Find("Anchor-left");
		mAnchorRight = GameObject.Find("Anchor-right");

		LoadAssetBundle("ItemContainer", LoadItemContainerComplete );
	}

	void LoadAssetBundle(string assetbundleName, LoadFunishHandler callback) {
		string filePath = FileUtils.getAssetBundlePath(assetbundleName);
		Globals.Api.Log(filePath);
		mAssetMgr.LoadAsset(filePath, callback);
	}

	void LoadTextureComplete(AssetBundle asset)
	{
//		mTexture.mainTexture = loader.texture;
//
//		string filepath = FileUtils.GetAssetPath("logo.jpg");
//		if(!File.Exists(filepath))
//			FileUtils.SaveFile("logo.jpg", loader.bytes);
//		GameObject obj = loader.assetBundle.mainAsset as GameObject;
	}

	void LoadItemContainerComplete(AssetBundle asset) {
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
