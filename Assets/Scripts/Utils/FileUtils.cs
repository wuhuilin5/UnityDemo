using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

namespace UnityDemo.Utils
{
	public sealed class FileUtils
	{
        public static string AssetRootPath = Path.Combine(Application.dataPath, "/");

        public static string StreamingAssetPath =
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
 "file:///" + Application.streamingAssetsPath + "/";
#elif UNITY_IPHONE
	"file:///" + Application.dataPath +"/Raw/"  // Application/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx/xxx.app/Data/Raw
#elif UNITY_ANDROID
	Application.streamingAssetsPath + "/";  // jar:file:///data/app/xxx.xxx.xxx.apk/!/assets
#else
	Application.streamingAssetsPath + "/"; 
#endif
        //private static System.Func<string, int> _getVersion = Globals.Api.loadFileManager.getVersion;

        public static string GetFileMD5(string filePath)
        {
            string ret = "";
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var hashValue = md5Hasher.ComputeHash(fs);
                ret = BitConverter.ToString(hashValue);
                ret = ret.Replace("-", "");
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            return ret;
        }

		public static bool SaveFile(string filepath, byte[] data )
		{
            if (string.IsNullOrEmpty(filepath))
            {
                DebugInfo.Log("[SaveFile] filePath is null or emptty");
                return false;
            }

            var ret = false;
            try
            {
                var path = filepath.Replace("\\", "/");
                var dir = path.Substring(0, path.LastIndexOf("/") + 1);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(path))
                    File.Delete(path);

                FileStream stream = new FileStream(path, FileMode.Create);
                BinaryWriter write = new BinaryWriter(stream);
                write.Write(data, 0, data.Length);
                write.Flush();

                write.Close();
                stream.Close();
                ret = true; ;
            }
            catch (Exception e)
            {
                DebugInfo.LogError(e.Message);
                ret = false;
            }

            return ret;
		}

        public static bool SaveFile(string filepath, string content)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                DebugInfo.Log("[SaveFile] filePath is null or emptty");
                return false;
            }

            var ret = false;
            try
            {
                if (content == null)
                    content = "";

                var path = filepath.Replace("\\", "/");
                var dir = path.Substring(0, path.LastIndexOf("/") + 1);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (File.Exists(path))
                    File.Delete(path);

                FileStream stream = new FileStream(path, FileMode.Create);
                StreamWriter write = new StreamWriter(stream);
                write.Write(content);
                write.Flush();

                write.Close();
                stream.Close();
                ret = true;
            }
            catch (Exception e)
            {
                DebugInfo.LogError(e.Message);
                ret = false;
            }

            return ret;
        }

		public static long FreeSpece(string path)
        {
#if UNITY_EDITOR
            System.IO.DriveInfo[] allDrives = System.IO.DriveInfo.GetDrives();
            foreach (var d in allDrives)
            {
                if (!path.StartsWith(d.Name))
                {
                    continue;
                }
                return d.AvailableFreeSpace;
            }
#elif UNITY_ANDROID
                using(AndroidJavaObject statFs = new AndroidJavaObject( "android.os.StatFs", path)) {
                    //statFs.getBlockSize() * statFs.getAvailableBlocks()
                    return statFs.Call<int>("getBlockSize") * statFs.Call<int>("getAvailableBlocks");
                }
#elif UNITY_IPHONE
                return _iDiskSpace_FreeSpece(path);
#endif

            return -1;
        }

        public static string getFilePathWithoutExt(string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf("."));
        }

        public static string getAssetBundlePath(string name)
        {
            string tempPath = "Assetbundle/" + name + ".unity3d";

            //tempPath += "?v=" + _getVersion(tempPath);

            return StreamingAssetPath + tempPath;
        }

        public static string getAssetFilePath(string relativePath)
		{
			return string.Concat(StreamingAssetPath, relativePath);
		}

        public static string getXmlPath( string name )
        {
            string tempPath = name + ".xml";
			//tempPath += "?v=" + _getVersion(tempPath);
			
            return StreamingAssetPath + tempPath;
        }

		public static string GetAssetPath(string name)
		{
			return StreamingAssetPath + name;
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
	}
}