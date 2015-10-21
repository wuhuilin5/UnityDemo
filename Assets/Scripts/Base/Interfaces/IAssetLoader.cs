using System;

namespace UnityDemo.Interfaces
{
    public interface IAssetLoader 
    {
        void LoadAsset(string assetName, OnLoadFinished onLoaded, OnLoadProgress onProgress = null);
        void Release(string assetName);
    }
}
