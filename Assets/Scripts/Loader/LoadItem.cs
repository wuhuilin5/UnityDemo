using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace UnityDemo
{
    class LoadItem
    {
        #region public
        public void AddListener(Action<WWW> onSuccess, Action<String> onError = null)
        {
            mSuccessListener += onSuccess;

            if (onError != null)
                mErrorListener += onError;

            ++referenceCount;
        }

        public void RemoveListener(Action<WWW> onSuccess, Action<String> onError = null)
        {
            mSuccessListener -= onSuccess;
            if (onError != null)
                mErrorListener -= onError;

            --referenceCount;
        }

        public void OnLoadSuccess()
        {
            if (www != null)
            {
                if (mSuccessListener != null)
                    mSuccessListener(www);
            }
        }

        public void OnLoadError()
        {
            if (www != null)
            {
                if (mErrorListener != null)
                    mErrorListener(www.error);
            }
        }

        public void Dispose()
        {
            if (www != null)
            {
                if (www.assetBundle != null)
                    www.assetBundle.Unload(false);

                www.Dispose();
            }
            www = null;
            mSuccessListener = null;
            mErrorListener = null;
        }

        #endregion

        public string url;
        public WWW www;
        public int referenceCount { private set; get; }

        private event Action<WWW> mSuccessListener;
        private event Action<string> mErrorListener;
    }
}
