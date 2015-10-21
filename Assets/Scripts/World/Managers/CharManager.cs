

namespace UnityDemo.Managers
{
    public class CharManager : Singleton<CharManager>
    {
        #region public 
        public CharManager()
        {
             mMainCharData = new BaseData();
        }

        public BaseData mainCharData
        {
            get { return mMainCharData; }
        }

        public void InitMainChar(object attr)
        {
            //mainCharData.Init(AttrKeyDefine.ATTR_ID, attr.objectid);
            //foreach (var kv in attr.attrs)
            //{
            //    mainCharData.Init(kv.key, kv.value);
            //}
        }

        public object GetAttrValue(int key)
        {
            return mMainCharData.GetValue(key);
        }

        public void AddListener(int key, UnityDemo.BaseData.OnDataUpdateHandler listener)
        {
            mainCharData.AddListener(key, listener);
        }

        public void RemoveListener(int key, UnityDemo.BaseData.OnDataUpdateHandler listener)
        {
            mainCharData.RemoveListener(key, listener);
        }
        #endregion

        private BaseData mMainCharData;
    }
}
