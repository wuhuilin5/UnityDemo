using System.IO;
using System.Security.Cryptography;
	
using UnityDemo.interfaces;
using UnityDemo.interfaces.utils;
using UnityDemo.interfaces.manager;
using UnityEngine;

using UnityDemo.manager;

namespace UnityDemo.Utils
{
	public sealed class FileUtils
	{
        public static string AssetRootPath = Path.Combine(Application.dataPath, "/");

        private static string StreamingAssetRootPath =
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
  "file:///" + Application.streamingAssetsPath + "/";   
#elif UNITY_IPHONE
	"file:///" + Application.dataPath +"/Raw/"  // Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data/Raw
#elif UNITY_ANDROID
	Application.streamingAssetsPath + "/";  // jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
#else
	string.Empty;
#endif
//        private static System.Func<string, int> _getVersion = Globals.Api.loadFileManager.getVersion;

        public FileUtils ()
        {
        }

        public static string getAssetBundlePath(string name)
        {
            string tempPath = "Assetbundle/" + name + ".unity3d";
            
            //tempPath += "?v=" + _getVersion(tempPath);

            return StreamingAssetRootPath + tempPath;
        }

        public static string getXmlPath( string name )
        {
            string tempPath = name + ".xml";
			//tempPath += "?v=" + _getVersion(tempPath);
			
            return StreamingAssetRootPath + tempPath;
        }

		public static string GetAssetPath(string name)
		{
			return StreamingAssetRootPath + name;
		}


        public static string GetMd5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            FileStream oFileStream = null;

            MD5CryptoServiceProvider oMD5Hasher = new MD5CryptoServiceProvider();

            try
            {
                oFileStream = new FileStream(pathName.Replace("\"", ""), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream); 
                oFileStream.Close();
                strHashData = System.BitConverter.ToString(arrbytHashValue);

                strHashData = strHashData.Replace("-", "");

                strResult = strHashData;
            }

            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }

            return strResult;
        }

		public static void SaveFile( string filepath, byte[] data )
		{
			if(File.Exists(filepath))
				File.Delete(filepath);

			FileStream stream = new FileStream( filepath, FileMode.Create );
			
			stream.Write( data, 0, data.Length );
			stream.Flush();
			stream.Close();
		}
	}
}