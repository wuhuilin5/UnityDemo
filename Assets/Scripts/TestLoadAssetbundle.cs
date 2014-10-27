using UnityEngine;
using System.Collections;
using System.IO;

using UnityDemo;
using UnityDemo.Utils;
using UnityDemo.manager;
using UnityDemo.interfaces.manager;
using Proto;

public class TestLoadAssetbundle : MonoBehaviour {

	public UILabel mLblLog;
	GameObject mAnchorLeft;
	GameObject mAnchorRight;

	public UITexture mTexture;

	// Use this for initialization
	private IAssetManager mAssetMgr;

	void Awake() {
		mAssetMgr = Globals.Api.AssetManager;
	}

	void Start () {
		LoadFunishHandler loadComplete = (asset)=>{
			Globals.Api.loadFileManager.initVersionInfo(asset.mainAsset as TextAsset);
			asset.Unload(true);
			initShareAssetBundles();
		};

		mAssetMgr.LoadAsset(FileUtils.getAssetBundlePath("files"), loadComplete);

		TextAsset txtAsset = Resources.Load("Data/test_proto") as TextAsset;
		IProto proto = new test_proto();
		proto.LoadFromJson(txtAsset.text);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void initShareAssetBundles()
	{
		System.Action callback = () => {
			LoadAssetBundle("UIRoot", loadUIRootComplete);
		};
		
		loadShareAssetBundles(callback);
	}

	void loadShareAssetBundles( System.Action finishCallback)
	{
		int count = 0;
		LoadFunishHandler callback = delegate(AssetBundle asset) {
			count++;
			if(count==2 && finishCallback != null){
				finishCallback();
			}
			asset.LoadAll();
			//GameObject go = GameObject.Instantiate(loader.assetBundle.mainAsset) as GameObject;
		};

		mAssetMgr.LoadAsset(FileUtils.getAssetBundlePath("FantasyAtlas"), callback);
		mAssetMgr.LoadAsset(FileUtils.getAssetBundlePath("WoodenAtlas"), callback);
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
		mAssetMgr.LoadAsset(filePath, callback);
	}

	void LoadTextureComplete(AssetBundle asset)
	{
		TextAsset data = asset.mainAsset as TextAsset;

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
