using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityDemo.Utils;
using System.Reflection;
using System.Xml;
using UnityDemo.Managers;

namespace UnityDemo
{
    public class GameStart : GameMonoBehaviour
    {
        private static GameStart mInstance;
        public Action<bool> onLevelLoaded;

        private string mAssetPath = "Resources/Creature/Taidao/Taidao.prefab.u";
#if UNITY_EDITOR
        private string mSavePath = "e:\\temp\\";
#else
        private string mSavePath = "/mnt/sdcard/UnityDemo/";
#endif
        private string mFilePath;
        private string mState = "none";
        private string mError = "";

        public override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            base.Awake();
            mInstance = this;

            //Global.loader = gameObject.AddMissingComponent<WWWLoader>();
            //gameObject.AddMissingComponent<Test1>();

            string a = new string(new char[] { 'a', 'b', 'c' });
        }

        public static GameStart Instance
        {
            get{ return mInstance; }
        }

        // Use this for initialization
        public override void Start()
        {
            //ScriptManager.Instance.LoadLibs(() =>
            //    {
            //        var relativePath = "Resources/UI/Test.prefab";
            //        var filePath = AppConfig.ASSET_FILE_HEAD + Application.streamingAssetsPath + "/" + relativePath + ".u";
            //        Global.loader.Load(filePath, (www) =>
            //        {
            //            var assetbundle = www.assetBundle;
            //            assetbundle.LoadAll();
            //            GameObject gameobject = GameObject.Instantiate(assetbundle.mainAsset) as GameObject;
            //            Global.loader.Destroy(www.url);

            //            Global.loader.Load(ScriptManager.Instance.GetScriptXmlPath(relativePath), (loader) =>
            //                {
            //                    ScriptManager.Instance.BindScripts(gameobject, loader.bytes);
            //                    Global.loader.Destroy(loader.url);
            //                });
            //        });
            //    });
        }

        
        public void StartGame()
        {
            GameHook.OnInit();
        }

        // Update is called once per frame
        public override void Update()
        {

        }

        public override void OnLevelWasLoaded(int level)
        {
            if (onLevelLoaded != null)
                onLevelLoaded(true);
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 400, 100), "state:" + mState);

            GUI.Label(new Rect(0, 50, 400, 100), "filePath:" + mFilePath);

            GUI.Label(new Rect(0, 100, 400, 100), "error:" + mError); 
        }

}
}
