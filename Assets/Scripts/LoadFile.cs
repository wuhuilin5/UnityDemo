using System;
using UnityDemo.interfaces;

namespace UnityDemo.loadfile
{
	public class LoadFile: ILoadFile
	{
		private string path;
		private int version;
        private string md5;

		public LoadFile ( string path, int version, string md5 )
		{
			this.Path = path;
			this.Version = version;
            this.Md5 = md5;
		}
		
		public string Path {
			get {
				return this.path;
			}
			set {
				path = value;
			}
		}

		public int Version {
			get {
				return this.version;
			}
			set {
				version = value;
			}
		}

        public string Md5
        {
            get
            {
                return this.md5;
            }
            set
            {
                md5 = value;
            }
        }
	}
}