
using System.Xml;
using System.Collections.Generic;

using UnityDemo.interfaces;
using UnityDemo.loadfile;
using UnityDemo.interfaces.manager;
using UnityEngine;

namespace UnityDemo.manager
{
    public class LoadFileManager: ILoadFileManager
	{
        private static ILoadFileManager instance;

		private Dictionary<string, ILoadFile> fileMap = new Dictionary<string, ILoadFile>();
		private string dir = "";

        private LoadFileManager() { }

        public static ILoadFileManager getIntance()
        {
            if (instance == null)
                instance = new LoadFileManager();

            return instance;
        }

		//XmlDocument xmldoc = new XmlDocument();
        //xmldoc.Load(filepath);  
		public void setData( XmlDocument xmlDoc )
		{
            fileMap.Clear();

			XmlNodeList nodelist = xmlDoc.SelectSingleNode("fs").ChildNodes; 
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

            //foreach (KeyValuePair<string, ILoadFile> keyValue in fileMap)
            //{
            //    string msg = string.Format("n:{0},v:{1}\n", keyValue.Value.Path, keyValue.Value.Version);
            //    Debug.Log(msg);
            //}
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

            int index = dir.LastIndexOf("/");   //返回上一个目录
            dir = index >= 0 ? dir.Substring(0, index) : "";
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