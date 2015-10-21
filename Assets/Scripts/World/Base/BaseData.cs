using System.Collections.Generic;

namespace UnityDemo
{
    public class BaseData
    {
        public delegate void OnDataUpdateHandler(int key, object oldValue, object newValue);

        #region 公共方法
        public BaseData()
        {
            mDataDict = new Dictionary<int, object>();
            mListeners = new Dictionary<int, OnDataUpdateHandler>();
        }

        public void Init(int key, object value)
        {
            mDataDict[key] = value;
        }

        public object GetValue(int key)
        {
            if (mDataDict.ContainsKey(key))
                return mDataDict[key];

            return null;
        }

        public void Update(int key, object value)
        {
            if (mDataDict.ContainsKey(key))
            {
                object oldValue = mDataDict[key];
                mDataDict[key] = value;
                NotifyListenter(key, oldValue, value);
            }
            else
            {
                DebugInfo.LogError(string.Format("[BaseData.Update] not init key:{0}", key));
            }
        }

        public void AddListener(int key, OnDataUpdateHandler listener)
        {
            if (mListeners.ContainsKey(key))
                mListeners[key] += listener;
            else
                mListeners[key] = listener;
        }

        public void RemoveListener(int key, OnDataUpdateHandler listener)
        {
            if (!mListeners.ContainsKey(key))
                return;

            mListeners[key] -= listener;
        }
        #endregion

        #region 私有方法
        private void NotifyListenter(int key, object oldValue, object currValue)
        {
            var listeners = mListeners[key];
            if (listeners != null)
                listeners(key, oldValue, currValue);
        }
        #endregion

        #region 变量
        private Dictionary<int, object> mDataDict;
        private Dictionary<int, OnDataUpdateHandler> mListeners;
        #endregion
    }
}
