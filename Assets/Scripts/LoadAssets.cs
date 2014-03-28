using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityDemo.interfaces.manager;
using UnityDemo.manager;
using UnityDemo.Utils;
using UnityEngine;

public class LoadAssets : MonoBehaviour
{
    public static Dictionary<string, string[]> AssetMap;

    private ILoadManger loadMgr;

    void Start()
    {
        loadMgr = LoadManager.getIntance();
 
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
            StartCoroutine( loadMgr.loadUrl(url, onLoadComplete, filename));
        }
    }

    private void onLoadComplete(AssetBundle asset, string filename)
    {
        GameObject obj;
        string[] list = AssetMap[filename];
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