using System;
using System.Collections;
using UnityDemo.interfaces.manager;
using UnityEngine;

namespace UnityDemo.manager
{
    public delegate void LoadFunishHandler( AssetBundle asset, string filename = null);

    public class LoadManager : MonoBehaviour, ILoadManger
    {
        private static ILoadManger instance;
  
		void Awake(){
			Debug.Log( "Awake LoadManager.." );
		}
//        private LoadManager() { }
//
//        public static ILoadManger getIntance()
//        {
//            if (instance == null)
//                instance = new LoadManager();
//
//            return instance;
//        }

        public IEnumerator loadUrl(string url, LoadFunishHandler callback = null, string filename = null )
        {
            int version = getVersion(url);
            WWW loader = WWW.LoadFromCacheOrDownload( url,version);
            Debug.Log(string.Format("Load Asset url:{0}, version:{1}", url, version));

            yield return loader;

            if (loader.error != null){
                Debug.Log(String.Format("Load Error:{0}", loader.error));
            }
            else {
                if (callback != null)
                {
                    if (filename != null){
                        callback(loader.assetBundle, filename);
                    }
                    else {
                        callback(loader.assetBundle);
                    }
                }
            }

            loader.assetBundle.Unload(false);  //TIPS：可能会导致资源渲染问题,等待0.5至1秒后再unload,
        }

        private int getVersion(string url)
        {
            int v = 0;
            int index = url.LastIndexOf( "?" );
     
            if( index >= 0 ){
                int startIndex = index + 1;
                string str = url.Substring( startIndex, url.Length-startIndex);
       
                if (str.Length >= 0)
                {
                    string[] paramlist = str.Split('&');
                    foreach (string item in paramlist)
                    {
                        string[] list = item.Split('=');
                        if (list.Length == 2 && list[0] == "v")
                        {
                            v = int.Parse(list[1]);
                            break;
                        }
                    }
                }
            }

            return v;
        }
    }
}