using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

////namespace UnityDemo
//{
    public sealed class AppConfig
    {
        //是否需要更新
        public static readonly Boolean IsNeedUpdate = 
#if UNITY_STANDALONE_WIN || UINTY_STANDALONE_OSX || UNITY_EDITOR
        true;
#else
        true;
#endif

        public static readonly string ASSET_FILE_HEAD = "file:///";
        public static readonly string RESOURCE_FOLDER = "Resources/";
        public static readonly string ASSET_FILE_EXTENSION = ".u";
    }
//}
