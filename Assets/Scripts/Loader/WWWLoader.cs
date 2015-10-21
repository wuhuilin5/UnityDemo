using System;
using System.Collections.Generic;

using UnityEngine;

using System.Collections;

namespace UnityDemo
{
    public class WWWLoader : GameMonoBehaviour
    {
        #region public methods
        public override void Awake()
        {
            base.Awake();
            mWWWDict = new Dictionary<string, WWW>();
        }

        public void Load(string url, Action<WWW> onSuccess, Action<String> onError = null)
        {
            Debug.Log("Load:" + url);
            StartCoroutine(LoadWWWAsset(url, onSuccess, onError));
        }

        public void Post(String url, byte[] data, Action<WWW> onSuccess, Action<String> onError = null)
        {
            StartCoroutine(PostData(url, data, onSuccess, onError));
        }

        public void Destroy(string url)
        {
            WWW www;
            if (mWWWDict.TryGetValue(url, out www))
            {
                //if (www.assetBundle != null)
                //    www.assetBundle.Unload(false);

                www.Dispose();
                mWWWDict.Remove(url);
            }
        }
        #endregion

        #region private methods
        private IEnumerator LoadWWWAsset(string url, Action<WWW> onSuccess, Action<String> onError)
        {
            var www = new WWW(url);
            mWWWDict[url] = www;
            yield return www;

            DoCallback(url, onSuccess, onError);
        }

        private IEnumerator PostData(string url, byte[] data, Action<WWW> onSuccess, Action<String> onError)
        {
            var www = new WWW(url, data);
            mWWWDict[url] = www;
            yield return www;

            DoCallback(url, onSuccess, onError);
        }

        private void DoCallback(string url, Action<WWW> onSuccess, Action<String> onError)
        {
             WWW www;
             if (mWWWDict.TryGetValue(url, out www))
             {
                 if (www.error != null)
                 {
                     Debug.Log("error:" + www.error);
                     if (onError != null)
                         onError(www.error);
                     Destroy(www.url);
                 }
                 else
                 {
                     if (onSuccess != null)
                         onSuccess(www);
                 }
             }
        }
        #endregion

        #region private variable
        private Dictionary<string, WWW> mWWWDict;
        #endregion
    }
}