using System;
using System.Collections;

using UnityDemo.Utils;
using UnityDemo.Interfaces;

using UnityEngine;

namespace UnityDemo.Manager
{
    public delegate void LoadFunishHandler( UnityEngine.Object asset );

    public class AssetManager : Singleton<AssetManager>, IAssetManager
    {
        private static IAssetManager instance;
  		
		void Awake(){
			Debug.Log( "Awake AssetManager.." );
		}
//        private AssetManager() { }
//
//        public static ILoadManger getIntance()
//        {
//            if (instance == null)
//                instance = new AssetManager();
//
//            return instance;
//        }


        public void LoadAsset(string path, OnLoadFinished callback)
        {
//			System.Action<AssetBundle> handler = (asset) => {
//				if( callback != null ){
//					Globals.Api.Log("load callbck~");
//					callback(asset);
//				}	
//			};
			
			//StartCoroutine(load(path, callback));
        }
		
		private IEnumerator load(string path, LoadFunishHandler callback )
		{
			int version = getVersion(path);
			WWW loader = WWW.LoadFromCacheOrDownload(path,1006);  //android下无法使用
			//WWW loader = new WWW(path);
			Debug.Log(string.Format("Load Asset url:{0}, version:{1}", path, version));

            yield return loader;

            if (loader.error != null){
                Debug.Log(String.Format("Load Error:{0}", loader.error));
            }
            else {
				AssetBundle asset = loader.assetBundle;
				if( callback != null){
					callback(asset.mainAsset);
				}
            }

			//yield return new WaitForSeconds(0.5f);
            loader.assetBundle.Unload(false);  
		}
		
		private int getVersion(string path)
        {
           int v = 0; 
//			int index = path.LastIndexOf("?");
//     
//            if( index >= 0 ){
//                int startIndex = index + 1;
//				string str = path.Substring(startIndex, path.Length-startIndex);
//       
//                if (str.Length >= 0)
//                {
//                    string[] paramlist = str.Split('&');
//                    foreach (string item in paramlist)
//                    {
//                        string[] list = item.Split('=');
//                        if (list.Length == 2 && list[0] == "v")
//                        {
//                            v = int.Parse(list[1]);
//                            break;
//                        }
//                    }
//                }
//            }
//
			string newPath = path.Replace(FileUtils.StreamingAssetPath, "");
			v = LoadFileManager.Instance.getVersion(newPath);

            return v;
		
        }
    }
}