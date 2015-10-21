using UnityEngine;
using System.Collections;

using UnityEditor;
using System.IO;
using System.Xml;
using UnityDemo.Utils;


using UnityDemo.Managers;
using UnityDemo;

public class VersionFileExporter : Editor
{
    [MenuItem("Custom Editor/Create assetbundle version file")]
	static void BuildAssetBundleVersion()
    {
		string filePath = Application.dataPath + "/Resources/files.xml";
		if (File.Exists(filePath))
		{
			LoadFileManager.Instance.initversionInfo(filePath);
			File.Delete(filePath);
        }

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("fs");

        try{
            string assetsPath = Application.streamingAssetsPath;
			appendDirectory(assetsPath, xmlDoc, root);

        }finally{
        }
      
        xmlDoc.AppendChild(root);
		xmlDoc.Save(filePath);
		
		Debug.Log("Export Successed! " + filePath);
    }

    private static string dir = "";

    private static void appendDirectory( string filePath, XmlDocument xmlDoc, XmlElement root )
    {
        DirectoryInfo folderInfo = new DirectoryInfo(filePath);
        FileSystemInfo[] fileInfos = folderInfo.GetFileSystemInfos();

        foreach (FileSystemInfo fileInfo in fileInfos)
        {
            appendFile(xmlDoc, root, fileInfo);
        }

        int index = dir.LastIndexOf("/");
        dir = index >= 0 ? dir.Substring(0, index) : "";
    }

    private static void appendFile(XmlDocument xmlDoc, XmlElement root, FileSystemInfo fileInfo)
    {
        string filePath = fileInfo.FullName;

        if (Directory.Exists(filePath))
        {
			DirectoryInfo folderInfo = new DirectoryInfo(filePath);
        	FileSystemInfo[] fileInfos = folderInfo.GetFileSystemInfos();
			
			if( fileInfos.Length > 0 ) 
			{
	            XmlElement rootDir = xmlDoc.CreateElement("d");
	            rootDir.SetAttribute("n", fileInfo.Name);
	            root.AppendChild(rootDir);
	
	            dir += "/"+ fileInfo.Name;
	           	appendDirectory(filePath, xmlDoc, rootDir);
			}
        }
        else
        {
			if(!isMetaFile(fileInfo.FullName)){
	            XmlElement node = xmlDoc.CreateElement("d");
	            node.SetAttribute("u", fileInfo.Name);

	            string md5 = FileUtils.GetFileMD5(fileInfo.FullName);
	            int version = getVersion(fileInfo, md5);

	           // Debug.Log(string.Format("file:{0}, md5:{1}, dir:{2}", fileInfo.Name, md5, dir));

	            node.SetAttribute("v", version.ToString()); 
	            node.SetAttribute("md5", md5);

                var info = new FileInfo(fileInfo.FullName);
                node.SetAttribute("s", info.Length.ToString());

	            root.AppendChild(node);
			}
        }
    }

    private static int getVersion( FileSystemInfo fileInfo, string md5 )
    {
        string path = fileInfo.Name;
        if ( dir.Length > 0){
            int startIdx = 1;
            path = dir.Substring( startIdx, dir.Length-startIdx) + "/" + fileInfo.Name;
        }

        int newV = 0;

        LoadFile file = LoadFileManager.Instance.getFile( path );
        if( file != null ){
            int oldV = file.version;
            string oldMd5 = file.md5;
              
            newV = oldV;

            bool ret = oldMd5.Equals(md5);
            if (!ret ){
                newV = TimeUtils.GetTime();
            }

           //Debug.Log(string.Format("file:{0}, oldV:{1}: newV:{2}", fileInfo.Name, oldV, newV ));
        }else{
            newV = TimeUtils.GetTime();
        }
        return newV;
    }

    static bool isMetaFile(string fileName)
    {
        int startIndex = fileName.LastIndexOf(".");
        int length = fileName.Length - startIndex;

        string ext = fileName.Substring(fileName.LastIndexOf("."), length);
        return ext == ".meta";
    }

}