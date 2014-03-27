using System.IO;
using UnityDemo.interfaces;
using UnityDemo.interfaces.utils;
using UnityEngine;

namespace UnityEditor.Utils
{
	public sealed class FileUtils
	{
        public static string AssetRootPath = Path.Combine(Application.dataPath, "/");

        private static string StreamingAssetRootPath =
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
 "File://" + Application.streamingAssetsPath + "/";
#elif UNITY_IPHONE
	Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
	"jar:file://" + Application.dataPath + "!/Assets/";
#else
	string.Empty;
#endif

        public FileUtils ()
        {
        }

        public static string getAssetBundlePath(string name)
        {
            string tempPath = "assetbundle/" + name + ".assetbundle";
            return StreamingAssetRootPath + tempPath;
        }
	}
}