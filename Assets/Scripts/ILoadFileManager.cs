using System.Xml;

namespace UnityDemo.interfaces.manager
{
    public interface ILoadFileManager
    {
        void setData(XmlDocument xmlDoc);
        ILoadFile getFile(string path);
        string getVersion(string path);
    }
}