using System;
using UnityDemo.interfaces;

namespace UnityDemo.loadfile
{
	public class LoadFile: ILoadFile
	{
		private string path;
		private string version;
		
		public LoadFile ( string path, string version )
		{
			this.Path = path;
			this.Version = version;
		}
		
		public string Path {
			get {
				return this.path;
			}
			set {
				path = value;
			}
		}

		public string Version {
			get {
				return this.version;
			}
			set {
				version = value;
			}
		}
	}
}

