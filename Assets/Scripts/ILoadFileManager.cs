using System.Xml;

namespace UnityDemo.interfaces.manager
{
    public interface ILoadFileManager
    {
        void setData(XmlDocument xmlDoc);
        ILoadFile getFile(string path);
        int getVersion(string path);
        string getMd5(string path);
    }
}