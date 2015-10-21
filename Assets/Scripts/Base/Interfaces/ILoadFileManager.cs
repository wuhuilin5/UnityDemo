using System.Xml;

using UnityEngine;

namespace UnityDemo.Interfaces
{
    public interface LoadFileManager
    {
		void initVersionInfo(TextAsset data);
		void initversionInfo(string filePath);

        void setData(XmlDocument xmlDoc);
        LoadFile getFile(string path);
        int getVersion(string path);
        string getMd5(string path);
    }
}