
using System.Xml;
using System.Collections.Generic;
using System.IO;

using UnityDemo.interfaces;
using UnityDemo.loadfile;
using UnityDemo.interfaces.manager;
using UnityDemo.Utils;

using UnityEngine;

namespace UnityDemo.manager
{
    public class LoadFileManager: MonoBehaviour, ILoadFileManager
	{
        private static ILoadFileManager instance;

		private Dictionary<string, ILoadFile> fileMap = new Dictionary<string, ILoadFile>();
		private string dir = "";
		
		void Awake(){
			init();
		}
		
//        private LoadFileManager() 
//        {
//            init();
//        }

//        public static ILoadFileManager getIntance()
//        {
//            if (instance == null)
//                instance = new LoadFileManager();
//
//            return instance;
//        }

		private void init()
		{

		}

		public void initversionInfo(string filePath)
		{
			if (File.Exists(filePath))
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load( filePath );
                setData(xmlDoc);
            }
        }

		public void initVersionInfo(TextAsset data)
		{
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load (new MemoryStream(data.bytes));
			setData (xmlDoc);
		}

		//XmlDocument xmldoc = new XmlDocument();
        //xmldoc.Load(filepath);  
		public void setData( XmlDocument xmlDoc )
		{
            fileMap.Clear();
			XmlNode root = xmlDoc.SelectSingleNode("fs");
			string v = (root as XmlElement).GetAttribute("v");

			XmlNodeList nodelist = xmlDoc.SelectSingleNode("fs").ChildNodes; 
			foreach( XmlElement node in nodelist )
			{
				dir = "";
				if( node.HasAttribute("u"))
				{
                    addFile(node);
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
                    addFile(node);
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

        private void addFile( XmlElement node )
        {
            string name = node.GetAttribute("u");
            string path = dir.Length > 0 ? dir+"/"+name : name;
         
            int version = int.Parse(node.GetAttribute("v"));
            string md5 = node.GetAttribute("md5");

            ILoadFile file = new LoadFile(path, version, md5);
            fileMap.Add(file.Path, file);
        }

		public ILoadFile getFile( string path )
		{
			if( fileMap.ContainsKey( path ))
				return fileMap[path];
			return null;
		}
		
		public int getVersion( string path )
		{
			ILoadFile file = getFile( path );
			if( file != null )
				return file.Version;
			
			return 0;
		}

        public string getMd5(string path)
        {
            ILoadFile file = getFile(path);
            if (file != null)
                return file.Md5;

            return "0";
        }

		private void parseElement(XmlNode xmlNode, string fileName)
		{
			XmlElement element = xmlNode as XmlElement;
			while (element != null)
			{
				if (element.HasAttribute("u"))
				{
					parseElement(element, string.Concat(fileName, element.GetAttribute("u"), "/"));
				}else
				{
					string name = string.Concat(fileName, element.GetAttribute("u"));
					int version = int.Parse(element.GetAttribute("v"));
					int size = int.Parse(element.GetAttribute("s"));
				 }
				element = element.NextSibling as XmlElement;
			}
		}
	}
}