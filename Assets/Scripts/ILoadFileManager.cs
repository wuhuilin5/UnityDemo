using System.Xml;

using UnityEngine;

namespace UnityDemo.interfaces.manager
{
    public interface ILoadFileManager
    {
		void initVersionInfo(TextAsset data);
		void initversionInfo(string filePath);

        void setData(XmlDocument xmlDoc);
        ILoadFile getFile(string path);
        int getVersion(string path);
        string getMd5(string path);
    }
}