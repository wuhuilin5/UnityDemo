using UnityEngine;
using System.Collections;

using UnityEditor;
using System.IO;
using System.Xml;
using UnityDemo.Utils;

using UnityDemo.interfaces.manager;
using UnityDemo.manager;
using UnityDemo.interfaces;
using UnityDemo;

public class MyEditor : Editor
{
   private static ILoadFileManager loadFileMgr = Globals.Api.loadFileManager;

	private static readonly string AssetBundleDir = Application.streamingAssetsPath + "/AssetBundle/";
	private static readonly string AssetBundleExt = ".unity3d";

	[MenuItem("Custom Editor/BuildAssetBundle - Windows")]
	static void BuildAssetBundle_windows()
	{
		BuildAllPrefabToAssetBundles(BuildTarget.StandaloneWindows);
	}

	[MenuItem("Custom Editor/BuildAssetBundle - Android")]
	static void BuildAssetBundle_Android()
	{
		BuildAllPrefabToAssetBundles(BuildTarget.Android);
	}

	[MenuItem("Custom Editor/BuildAssetBundle - iPhone")]
	static void BuildAssetBundle_iPhone()
	{
		BuildAllPrefabToAssetBundles(BuildTarget.iPhone);
	}
	
	static void BuildAllPrefabToAssetBundles(BuildTarget buildTarget)
	{
		BuildPipeline.PushAssetDependencies();

		BuildFiles(GetFiles("Assets/Resources/Prefabs/Atlas"), true, buildTarget);  // Altas
		BuildFiles(GetFiles("Assets/Resources/Prefabs"), false, buildTarget);
	
		BuildPipeline.PopAssetDependencies();

		AssetDatabase.Refresh();
	}

	static string[] GetFiles(string dirName)
	{
		return Directory.GetFiles(dirName, "*", SearchOption.TopDirectoryOnly);
	}

	static void BuildFiles(string[] files, bool isDependencies, BuildTarget buildTarget )
	{
		foreach( string filePath in files)
		{
			if(!isDependencies)
				BuildPipeline.PushAssetDependencies();

			Object obj = LoadMainAssetAtPath(filePath);
			if (obj == null) 
				continue;
			
			string targetPath = AssetBundleDir + obj.name + AssetBundleExt;
			BuildAssetBundler( obj, null, targetPath, buildTarget);

			if(!isDependencies)
				BuildPipeline.PopAssetDependencies();
		}
	}

	static bool BuildAssetBundler(Object mainAsset, Object[] assets, string pathName, BuildTarget buildTarget )
	{
		var options = BuildAssetBundleOptions.CollectDependencies |
			BuildAssetBundleOptions.CompleteAssets |
				BuildAssetBundleOptions.DeterministicAssetBundle;

		bool ret = BuildPipeline.BuildAssetBundle(mainAsset, assets, pathName, options, buildTarget);
		if (ret){
			Debug.Log(string.Format("export successed! {0}", pathName));
		}
		else{
			Debug.Log(string.Format("export failed! {0}", pathName));
		}

		return ret;
	}

	static Object LoadMainAssetAtPath(string filePath)
	{
		if (isMetaFile(filePath))
			return null;

		string newpath = filePath.Replace("\\", "/");
		return AssetDatabase.LoadMainAssetAtPath(newpath);
	}

	static bool isMetaFile(string fileName)
	{
		int startIndex = fileName.LastIndexOf(".");
		int length = fileName.Length - startIndex;

		string ext = fileName.Substring(fileName.LastIndexOf("."), length);
		return ext == ".meta";
	}

//    [MenuItem("Custom Editor/Create AssetBundle All")]
//    static void CreateAssetBundleAll()
//    {
//        string targetPath = Application.streamingAssetsPath + "/assetbundle/all.assetbundle";
//        Object[] SelectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
//
//        bool ret = BuildPipeline.BuildAssetBundle(null, SelectedAssets, targetPath, BuildAssetBundleOptions.CollectDependencies);
//        if (ret)
//        {
//            AssetDatabase.Refresh();
//            Debug.Log("Create AssetBundle All Successed.");
//        }
//        else
//        {
//            Debug.Log("Create AssetBundle All failed.");
//        }
//
//        ExportDirToXml();
//    }
//
//    //[MenuItem("Custom Editor/Build AssetBundles From Directory of Files")]
//    //static void ExportAssetBundles()
//    //{
//    //    string path = AssetDatabase.GetAssetPath(Selection.activeObject);
//    //    Debug.Log("Selected Folder: " + path);
//
//    //    if (path.Length != 0)
//    //    {
//    //        path = path.Replace("Assets/", "");
//    //        Debug.Log("data path: " + Application.dataPath);
//
//    //        string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);
//
//    //        foreach (string filename in fileEntries)
//    //        {
//    //            string filepath = filename.Replace("\\", "/");
//    //            int index = filepath.LastIndexOf("/");
//    //            Debug.Log(filepath);
//
//    //            string localPath = "Assets/" + path;
//    //            if (index > 0)
//    //                localPath += filepath;
//
//    //            Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
//    //            if (t != null)
//    //            {
//    //                Debug.Log(t.name);
//    //                string bundlePath = "Assets/" + path + "/" + t.name + ".unity3d";
//
//    //                BuildPipeline.BuildAssetBundle(t, null, bundlePath, BuildAssetBundleOptions.CompleteAssets);
//    //            }
//    //        }
//
//    //    }
//    //}
//
//    [MenuItem("Custom Editor/Export Scene To XML")]
//    static void ExportSceneToXML()
//    {
//        string filepath = Application.streamingAssetsPath + "/UnityDemo.xml";
//        if (File.Exists(filepath))
//        {
//            File.Delete(filepath);
//        }
//
//        Debug.Log("exprot scene " + filepath);
//
//        XmlDocument xmlDoc = new XmlDocument();
//        XmlElement root = xmlDoc.CreateElement("gameObjects");
//
//        foreach (UnityEditor.EditorBuildSettingsScene s in UnityEditor.EditorBuildSettings.scenes)
//        {
//            if (s.enabled)
//            {
//                string name = s.path;
//                Debug.Log("exprot scene " + name);
//
//                EditorApplication.OpenScene(name);
//                XmlElement scenes = xmlDoc.CreateElement("scenes");
//                scenes.SetAttribute("name", name);
//
//                foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
//                {
//                    if (obj.transform.parent == null)
//                    {
//                        XmlElement gameObject = xmlDoc.CreateElement("gameObject");
//                        gameObject.SetAttribute("name", obj.name);
//                        gameObject.SetAttribute("asset", obj.name + ".assetbundle");
//
//                        XmlElement transform = xmlDoc.CreateElement("transform");
//
//                        //position
//                        XmlElement position = xmlDoc.CreateElement("position");
//                        position.SetAttribute("x", obj.transform.position.x + "");
//                        position.SetAttribute("y", obj.transform.position.y + "");
//                        position.SetAttribute("z", obj.transform.position.z + "");
//
//                        //rotation
//                        XmlElement rotation = xmlDoc.CreateElement("rotation");
//                        rotation.SetAttribute( "x",  obj.transform.rotation.eulerAngles.x + "");
//                        rotation.SetAttribute( "y",  obj.transform.rotation.eulerAngles.y + "");
//                        rotation.SetAttribute( "z",  obj.transform.rotation.eulerAngles.z + "");
//
//                        //scale;
//                        XmlElement scale = xmlDoc.CreateElement("scale");
//                        scale.SetAttribute( "x", obj.transform.localScale.x + "");
//                        scale.SetAttribute( "y", obj.transform.localScale.y + "");
//                        scale.SetAttribute( "z", obj.transform.localScale.z + "");
//
//                        transform.AppendChild(position);
//                        transform.AppendChild(rotation);
//                        transform.AppendChild(scale);
//
//                        gameObject.AppendChild(transform);
//                        scenes.AppendChild(gameObject);
//                    }
//                }
//
//                root.AppendChild(scenes);
//                xmlDoc.AppendChild(root);
//                xmlDoc.Save(filepath);
//            }
//        }
//
//        AssetDatabase.Refresh();
//        Debug.Log("export success");
//    }
//
//    //[MenuItem("Custom Editor/Export Scene")]
//    //static void ExportScene()
//    //{
//    //    Caching.CleanCache();
//
//    //    string targetPath = EditorUtility.SaveFilePanel("Save Resource", "/StreamingAssets/", "New Resource", "unity3d");
//    //    if (targetPath.Length != 0)
//    //    {
//    //        string[] scenes = { "Assets/UnityDemo.unity" };
//
//    //        BuildPipeline.BuildPlayer(scenes, targetPath, BuildTarget.StandaloneWindows, BuildOptions.BuildAdditionalStreamedScenes);
//    //    }
//    //}
//
    [MenuItem("Custom Editor/Create assetbundle version file")]
    static void ExportDirToXml()
    {
        string filepath = Application.streamingAssetsPath + "/files.xml";
       
        if (File.Exists(filepath))
        {
            XmlDocument oldXml = new XmlDocument();
            oldXml.Load(filepath);
            loadFileMgr.setData(oldXml);

            File.Delete(filepath);
        }

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("fs");

        try{
            string filePath = Application.streamingAssetsPath;
            appendDirectory(filePath, xmlDoc, root);

        }finally{
        }
      
        xmlDoc.AppendChild(root);
        xmlDoc.Save(filepath);

        Debug.Log("Export Successed! " + filepath);
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

	            string md5 = FileUtils.GetMd5Hash(fileInfo.FullName);
	            int version = getVersion(fileInfo, md5);

	           // Debug.Log(string.Format("file:{0}, md5:{1}, dir:{2}", fileInfo.Name, md5, dir));

	            node.SetAttribute("v", version.ToString()); 
	            node.SetAttribute("md5", md5);

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

        ILoadFile file = loadFileMgr.getFile( path );
        if( file != null ){
            int oldV = file.Version;
            string oldMd5 = file.Md5;
              
            newV = oldV;

            bool ret = oldMd5.Equals(md5);
            if (!ret ){
                newV += 1;
            }

           Debug.Log(string.Format("file:{0}, oldV:{1}: newV:{2}", fileInfo.Name, oldV, newV ));
        }else{
            newV = 1;
        }
        return newV;
    }
}