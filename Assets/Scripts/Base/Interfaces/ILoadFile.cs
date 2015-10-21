
namespace UnityDemo.Interfaces
{
    public interface LoadFile
    {
        string path { get; set; }
        int version { get; set; }
        string md5 { get; set; }
    }
}