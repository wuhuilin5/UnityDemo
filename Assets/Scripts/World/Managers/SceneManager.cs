using System;
using System.Collections;

using UnityEngine;

namespace UnityDemo.Managers
{
    public class SceneManager : Singleton<SceneManager>
    {
        #region public
        public SceneManager()
        {
            mCurSceneName = String.Empty;
        }
        
        public void ShowScene(string sceneName, Action<bool> onFinish = null, Action<float> onProgress = null)
        {
            if (sceneName == mCurSceneName)
            {
                DebugInfo.Log(string.Format("Same Scene:{0}", sceneName));
                if (onFinish != null)
                    onFinish(false);
                return;
            }

            string lastScenePath = PathManager.Instance.GetScenePath(mCurSceneName);
            AssetManager.Instance.Release(lastScenePath);

            mCurSceneName = sceneName;
            AssetManager.Instance.LoadAsset(PathManager.Instance.GetScenePath(mCurSceneName),
                (scenePath, scene) =>
                {
                    onSceneLoaded(onFinish, onProgress);
                },
                (progress) =>
                {
                    //TODO：显示切换场景界面
                    if (onProgress != null)
                        onProgress(progress);
                });
        }
        #endregion

        #region private

        private void onSceneLoaded(Action<bool> onFinish, Action<float> onProgress)
        {
            GameStart.Instance.onLevelLoaded = (bool loaded) =>
                {
                    GameStart.Instance.StartCoroutine(UnloadUnusedAssets((bool isSuccess) =>
                        {
                            GC.Collect();
                            onLevelLoaded(onFinish, onProgress);
                        }));
                };
            Application.LoadLevel(mCurSceneName);
        }

        private void onLevelLoaded(Action<bool> onFinish, Action<float> onProgress)
        {
            GameStart.Instance.onLevelLoaded = null;

            if (onProgress != null)
                onProgress(100);

            if (onFinish != null)
                onFinish(true);
        }

        private IEnumerator UnloadUnusedAssets(Action<bool> callback)
        {
            yield return Resources.UnloadUnusedAssets();

            if (callback != null)
                callback(true);
        }

        private string mCurSceneName;
        #endregion
    }
}
