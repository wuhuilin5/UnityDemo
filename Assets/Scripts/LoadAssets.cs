using System.Collections;
using System.Collections.Generic;
using UnityDemo.manager;
using UnityEditor.Utils;
using UnityEngine;

public class LoadAssets : MonoBehaviour {

    public static readonly string rootPath = System.IO.Path.Combine( Application.streamingAssetsPath, "/");

	public static readonly string AssetSuffix = ".assetbundle";
	public static Dictionary<string, string[]> AssetMap;
	
	void Start () {
		initAssetMap();
		loadAssets();
	}

	private void initAssetMap()
	{
		AssetMap = new Dictionary<string, string[]>();
	
		AssetMap.Add( "cube_1", null);
		AssetMap.Add( "cube_2", null);
		//AssetMap.Add( "all", new string[]{"cube_1", "cube_2"});
	}
	
	private void loadAssets()
	{
        
		foreach( var item in AssetMap ){
			string filename = item.Key + AssetSuffix;

             string url = FileUtils.getAssetBundlePath(filename);
             StartCoroutine( LoadManager.getIntance().loadUrl(url, 1, onLoadComplete, filename));
		}
	}

    private void onLoadComplete(WWW loader, string filename )
    {
        AssetBundle bundle = loader.assetBundle;
     
        int index = filename.LastIndexOf(".");
        string assetName = filename.Substring(0, index);

        GameObject obj;
        string[] list = AssetMap[assetName];
        if (list == null || list.Length == 0)
        {
            obj = (GameObject)Instantiate(bundle.mainAsset);
            obj.SetActive(true);
        }
        else
        {
            foreach (string name in list)
            {
                obj = (GameObject)Instantiate(bundle.Load(name));
                obj.SetActive(true);
            }
        }

        bundle.Unload(false);
    }
}
