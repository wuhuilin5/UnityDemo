using System;
using UnityDemo.Interfaces;


namespace UnityDemo
{
    public class AssetBundleLoader : Singleton<AssetBundleLoader>, IAssetLoader
    {
        public AssetBundleLoader()
        {}
 
        public void LoadAsset(string assetName, OnLoadFinished onLoaded, OnLoadProgress onProgress = null)
        {
      
        }

        public void Release(string assetName)
        {

        }

        #region private methods
    
        #endregion
    }
}
