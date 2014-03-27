using System;
using System.Collections;
using UnityDemo.interfaces.manager;
using UnityEngine;

namespace UnityDemo.manager
{
    public delegate void LoadFunishHandler(WWW www, string filename);

    public class LoadManager : ILoadManger
    {
        private static ILoadManger instance;

        private LoadManager() { }

        public static ILoadManger getIntance()
        {
            if (instance == null)
                instance = new LoadManager();

            return instance;
        }

        public IEnumerator loadUrl(string url, int version, LoadFunishHandler callback = null, string filename = "")
        {
            WWW loader = WWW.LoadFromCacheOrDownload(url, version);

            yield return loader;

            if (loader.error != null){
                Debug.Log(String.Format("Load Error:{0}", loader.error));
            }
            else {
                if (callback != null)
                    callback( loader, filename );
            }
        }
    }
}