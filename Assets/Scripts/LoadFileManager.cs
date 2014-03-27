
using System.Xml;
using System.Collections.Generic;

using UnityDemo.interfaces;
using UnityDemo.loadfile;
using UnityDemo.interfaces.manager;

namespace UnityDemo.manager
{
    public class LoadFileManager: ILoadFileManager
	{
		private Dictionary<string, ILoadFile> fileMap = new Dictionary<string, ILoadFile>();
		private string dir = "";
		

		//XmlDocument xmldoc = new XmlDocument();
        //xmldoc.Load(filepath);   使用时需要把 xml load好
		
		public LoadFileManager( XmlDocument xmlDoc )
		{
			XmlNodeList nodelist = xmlDoc.SelectSingleNode("fs").ChildNodes;  //dir
			
			foreach( XmlElement node in nodelist )
			{
				dir = "";
				if( node.HasAttribute("u"))
				{
					ILoadFile file = new LoadFile( node.GetAttribute("u"), node.GetAttribute("v"));
					fileMap.Add( file.Path, file );
				}
				else
				{
					dir =  node.GetAttribute("n");
					handleDir( node.ChildNodes );	
				}
			}
			
//			foreach( KeyValuePair<string, ILoadFile> keyValue in fileMap )
//			{
//				string msg = string.Format( "n:{0},v:{1}\n", keyValue.Value.Path, keyValue.Value.Version );
//				Debug.Log( msg );
//				break;
//			}
		}
		
		private void handleDir( XmlNodeList nodelist )
		{
			foreach( XmlElement node in nodelist )
			{
				if( node.HasAttribute("u"))
				{
					ILoadFile file = new LoadFile( dir+"/"+node.GetAttribute("u"), node.GetAttribute("v"));
					fileMap.Add( file.Path, file );
				}
				else
				{
					dir += "/" + node.GetAttribute("n");
					handleDir( node.ChildNodes );	
				}
			}
			
			dir = dir.Substring( 0, dir.LastIndexOf( "/" ));
		}
		
		public ILoadFile getFile( string path )
		{
			if( fileMap.ContainsKey( path ))
				return fileMap[path];
			return null;
		}
		
		public string getVersion( string path )
		{
			ILoadFile file = getFile( path );
			if( file != null )
				return file.Version;
			
			return "0";
		}
	}
}