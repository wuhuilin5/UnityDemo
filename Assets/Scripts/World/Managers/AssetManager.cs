using System;
using System.Collections.Generic;
using UnityDemo.Interfaces;

namespace UnityDemo.Managers
{
    public class AssetManager : Singleton<AssetManager>
    {
        public AssetManager()
        {
            initAssetLoader();
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称（相对Resources文件的路径)</param>
        /// <param name="onLoaded">加载完成回调</param>
        /// <param name="onProgress">加载进度回调</param>
        public void LoadAsset(string assetName, OnLoadFinished onLoaded, OnLoadProgress onProgress = null)
        {
            mAssetLoader.LoadAsset(assetName,
                (name, obj) =>
                {
                    if (onLoaded != null)
                        onLoaded(name, obj);
                },
                (progress) =>
                {
                    //TODO:显示下载进度条
                    if (onProgress != null)
                        onProgress(progress);
                });
        }

        public void Release(string assetName)
        {
            mAssetLoader.Release(assetName);
        }

        public bool IsUnloadableAsset(string assetName)
        {
            var filter = new List<string>() { ".prefab", ".fbx" };
            foreach (var item in filter)
            {
                if (assetName.EndsWith(item, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        #region 私有函数

        private void initAssetLoader()
        {
            if (AppConfig.IsNeedUpdate){
                mAssetLoader = AssetBundleLoader.Instance;
            }else{
                mAssetLoader = ResourceLoader.Instance;
            }
        }
        #endregion

        private static IAssetManager mInstance;
        private IAssetLoader mAssetLoader;
    }
}