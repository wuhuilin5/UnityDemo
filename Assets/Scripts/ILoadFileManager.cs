using System;

namespace UnityDemo.interfaces
{
	public interface ILoadFileManager
	{
		ILoadFile getFile( string path );
		string getVersion( string path );
	}
}

