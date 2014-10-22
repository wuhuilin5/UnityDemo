using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityDemo.interfaces.manager;
using UnityDemo.manager;
using UnityDemo.Utils;
using UnityEngine;

using UnityDemo;

public class LoadAssets : MonoBehaviour
{
    public static Dictionary<string, string[]> AssetMap;

    private ILoadManger loadMgr;

    void Start()
    {
        loadMgr = Globals.Api.loadManager;
 
        initAssetMap();
        loadAssets();
    }

    private void initAssetMap()
    {
        AssetMap = new Dictionary<string, string[]>();

        AssetMap.Add("cube_1", null);
        AssetMap.Add("cube_2", null);
        //AssetMap.Add( "all", new string[]{"cube_1", "cube_2"});
    }

    private void loadAssets()
    {
        foreach (var item in AssetMap)
        {
            string filename = item.Key;

            string url = FileUtils.getAssetBundlePath(filename);
            loadMgr.loadUrl(url, onLoadComplete);
        }
    }

    private void onLoadComplete(WWW loader)
    {
        GameObject obj;
		AssetBundle asset = loader.assetBundle;
        string[] list = AssetMap[asset.name];
        if (list == null || list.Length == 0)
        {
            obj = (GameObject)Instantiate(asset.mainAsset);
            obj.SetActive(true);
        }
        else
        {
            foreach (string name in list)
            {
                obj = (GameObject)Instantiate(asset.Load(name));
                obj.SetActive(true);
            }
        }
    }
}