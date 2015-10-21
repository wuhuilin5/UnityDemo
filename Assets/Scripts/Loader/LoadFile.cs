using System;


namespace UnityDemo
{
	public class LoadFile
	{
        public LoadFile(string path, int version, string md5)
        {
            this.path = path;
            this.version = version;
            this.md5 = md5;
        }

        public string path { get; set; }
        public int version { get; set; }
        public string md5 { get; set; }

	}
}