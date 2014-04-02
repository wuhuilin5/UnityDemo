using System.IO;
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
 "File://" + Application.streamingAssetsPath + "/";
#elif UNITY_IPHONE
	Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
	"jar:file://" + Application.dataPath + "!/Assets/";
#else
	string.Empty;
#endif
        private static System.Func<string, int> _getVersion = Globals.Api.loadFileManager.getVersion;

        public FileUtils ()
        {
        }

        public static string getAssetBundlePath(string name)
        {
            string tempPath = "assetbundle/" + name + ".assetbundle";
            
            tempPath += "?v=" + _getVersion(tempPath);

            return StreamingAssetRootPath + tempPath;
        }

        public static string getXmlPath( string name )
        {
            string tempPath = name + ".xml";
            return StreamingAssetRootPath + tempPath;
        }

        public static string GetMd5Hash(string pathName)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;

            System.IO.FileStream oFileStream = null;

            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();

            try
            {
                oFileStream = new System.IO.FileStream(pathName.Replace("\"", ""), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);

                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream); //����ָ��Stream ����Ĺ�ϣֵ

                oFileStream.Close();

                //�������ַ��ָ���ʮ�����ƶԹ��ɵ�String������ÿһ�Ա�ʾvalue �ж�Ӧ��Ԫ�أ����硰F-2C-4A��

                strHashData = System.BitConverter.ToString(arrbytHashValue);

                //�滻-
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