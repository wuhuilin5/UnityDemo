using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Xml;
using UnityDemo.Interfaces;
using UnityDemo.Managers;
using UnityDemo.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityDemo
{
    public class ResourceLoader : Singleton<ResourceLoader>, IAssetLoader
    {
        #region public
        public ResourceLoader()
        {
            mFileDict = new Dictionary<string, string>();
            mResourceDict = new Dictionary<string, ResourceInfo>();
#if UNITY_EDITOR
            mResourcePath = string.Concat(Application.dataPath, "/", AppConfig.RESOURCE_FOLDER);
            GetFileInfo(new DirectoryInfo(mResourcePath));
            BuildPathFile();
#else
            LoadPathFile();
#endif
        }
        public void LoadAsset(string assetName, OnLoadFinished onLoaded, OnLoadProgress onProgress = null)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                DebugInfo.LogError("prefab name is null or empty!");
                return;
            }

            ResourceInfo resourceInfo;
            if (!mResourceDict.TryGetValue(assetName, out resourceInfo))
            {
                if (!mFileDict.ContainsKey(assetName))
                {
                    DebugInfo.LogError(String.Format("prefab not exsit:{0}", assetName));
                    return;
                }

                resourceInfo = new ResourceInfo();
                resourceInfo.relativePath = mFileDict[assetName];
                mResourceDict.Add(assetName, resourceInfo);
            }

            resourceInfo.Retain();
            GameStart.Instance.StartCoroutine(resourceInfo.LoadAsset((obj) =>
                {
                    if (onLoaded != null)
                        onLoaded(assetName, obj);
                }));
        }

        public void Release(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                DebugInfo.LogError("prefab name is null or empty!");
                return;
            }

            ResourceInfo resourceInfo;
            if (mResourceDict.TryGetValue(assetName, out resourceInfo))
            {
                resourceInfo.Release();
                if (resourceInfo.referenceCount == 0)
                    mResourceDict.Remove(assetName);
            }
        }

        #endregion

        #region private
        private void GetFileInfo(DirectoryInfo info)
        {
            var ds = info.GetDirectories();//.Where(t => t.Name.EndsWith(".") == false);
            var fs = info.GetFiles();

            foreach (var item in ds)
            {
                GetFileInfo(item);
            }

            foreach (var file in fs)
            {
                var fileName = file.FullName.Replace("\\", "/");
                if (!IsResource(fileName))
                    continue;

                var key = fileName.Replace(mResourcePath, "");
                if (!mFileDict.ContainsKey(key))
                {
                    mFileDict.Add(key, key);
                }
                else
                {
                    DebugInfo.LogWarning(String.Format("Resource key already exist:{0}", key));
                }
            }
        }

        private bool IsResource(string name)
        {
            var filter = new List<string>() { ".meta", ".xml" };
            foreach (var item in filter)
            {
                if (name.EndsWith(item, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }

        private void BuildPathFile()
        {
            var root = new SecurityElement("root");
            foreach (var item in mFileDict)
            {
                var path = new SecurityElement("file");
                path.AddAttribute("path", item.Value);
                root.AddChild(path);
            }

            string xmlPath = mResourcePath + mResourceInfoFileName;
            FileUtils.SaveFile(xmlPath, root.ToString());
        }

        private void LoadPathFile()
        {
            string xmlPath = FileUtils.getFilePathWithoutExt(mResourceInfoFileName);
            TextAsset info = Resources.Load(xmlPath) as TextAsset;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(new MemoryStream(info.bytes));

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("root").ChildNodes;
            if (nodeList == null)
            {
                DebugInfo.LogError(string.Format("{0} nodeList is null", mResourceInfoFileName));
                return;
            }

            string value;
            foreach (XmlElement item in nodeList)
            {
                value = item.GetAttribute("path");
                mFileDict.Add(value, value);
            }
        }

        private static ResourceLoader mInstance;

        private string mResourcePath;
        private const string mResourceInfoFileName = "ResourceInfo.xml";

        private Dictionary<string, string> mFileDict;
        private Dictionary<string, ResourceInfo> mResourceDict;
        #endregion
    }

    class ResourceInfo
    {
        public IEnumerator LoadAsset(Action<Object> onLoaded)
        {
            if (mObject == null)
            {
                var request = Resources.LoadAsync(FileUtils.getFilePathWithoutExt(relativePath));
                yield return request;
                mObject = request.asset;

                if (mObject == null)
                    DebugInfo.LogWarning(string.Format("Prefab not found at path:{0}", relativePath));
            }
            else
            {
                yield return null;
            }

            if (onLoaded != null)
                onLoaded(mObject);
        }

        public void Retain()
        {
            ++referenceCount;
        }

        public void Release()
        {
            --referenceCount;

            if (referenceCount == 0)
                UnloadAsset();
        }

        public override string ToString()
        {
            return string.Format("{0}:rc-{1}, object-{2}", relativePath, referenceCount, mObject == null ? "null" : mObject.ToString());
        }

        private void UnloadAsset()
        {
            if (mObject != null)
            {
                if (AssetManager.Instance.IsUnloadableAsset(relativePath))
                {
                    Resources.UnloadAsset(mObject);
                    mObject = null;
                }
                else
                {
                    mObject = null;
                    Resources.UnloadUnusedAssets();
                }
            }

            DebugInfo.Log(String.Format("Destroy!!{0}", ToString()));
        }

        public string relativePath;
        public int referenceCount { private set; get; }

        private Object mObject;
    }
}
