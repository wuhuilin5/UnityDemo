using UnityEngine;
using System.Collections;

using UnityEditor;
using System.IO;
using System.Xml;

public class MyEditor : Editor
{
    [MenuItem("Custom Editor/Create AssetBundle Main" )]
	static void CreateAssetBundlesMain()
    {
       	Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);		
		foreach( Object obj in SelectedAsset )
		{
			string sourcePath = AssetDatabase.GetAssetPath( obj );
			string targetPath = Application.dataPath + "/StreamingAssets/" + obj.name + ".assetbundle";
			
			Debug.Log( "sourcePath: " + sourcePath );
			
			//移动平台需要在最后添加一个参数：android: BuildTarget.Android, ios: BuildTarget.iPhone
        	bool ret = BuildPipeline.BuildAssetBundle( obj, null, targetPath, BuildAssetBundleOptions.CollectDependencies );
			
			if( ret ){
				Debug.Log( obj.name + " export successed! " + targetPath );
			}else{
				Debug.Log( obj.name + " export failed");
			}
		}   
		
		AssetDatabase.Refresh();
	}

    [MenuItem("Custom Editor/Create AssetBundle All")]
    static void CreateAssetBundleAll()
    {
		Caching.CleanCache();
		
		string targetPath = Application.dataPath + "/StreamingAssets/all.assetbundle";
		Object[] SelectedAssets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets );
	    
		bool ret = BuildPipeline.BuildAssetBundle( null, SelectedAssets, targetPath, BuildAssetBundleOptions.CollectDependencies );
		if( ret ){
			AssetDatabase.Refresh();		
			Debug.Log( "Create AssetBundle All Successed." );
		}else{
			Debug.Log( "Create AssetBundle All failed." );
		}
    }

    [MenuItem("Custom Editor/Build AssetBundles From Directory of Files" )]
    static void ExportAssetBundles()
    {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        Debug.Log("Selected Folder: " + path);
		
        if (path.Length != 0)
        {
            path = path.Replace("Assets/", "");
            Debug.Log("data path: " + Application.dataPath);

            string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + path);

            foreach (string filename in fileEntries)
            {
                string filepath = filename.Replace("\\", "/");
                int index = filepath.LastIndexOf("/");
                Debug.Log(filepath);

                string localPath = "Assets/" + path;
                if (index > 0)
                    localPath += filepath;

                Object t = AssetDatabase.LoadMainAssetAtPath(localPath);
                if (t != null)
                {
                    Debug.Log(t.name);
                    string bundlePath = "Assets/" + path + "/" + t.name + ".unity3d";

                    BuildPipeline.BuildAssetBundle(t, null, bundlePath, BuildAssetBundleOptions.CompleteAssets);
                }
            }

        }

    }

    [MenuItem("Custom Editor/Export Scenes To XML")]
    static void ExportXML()
    {
        string filepath = Application.dataPath + "/my.xml";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }

        Debug.Log("exprot scene " + filepath);

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("gameObjects");

        foreach (UnityEditor.EditorBuildSettingsScene s in UnityEditor.EditorBuildSettings.scenes)
        {
            if (s.enabled)
            {
                string name = s.path;
                Debug.Log("exprot scene " + name);

                EditorApplication.OpenScene(name);
                XmlElement scenes = xmlDoc.CreateElement("scenes");
                scenes.SetAttribute("name", name);

                foreach( GameObject obj in Object.FindObjectsOfType( typeof(GameObject)))
                {
                    if (obj.transform.parent == null)
                    {
                        XmlElement gameObject = xmlDoc.CreateElement("gameObject");
                        gameObject.SetAttribute("name", obj.name);
                        gameObject.SetAttribute("asset", obj.name + ".prefab");

                        XmlElement transform = xmlDoc.CreateElement("transform");

                        //position
                        XmlElement position = xmlDoc.CreateElement("position");

                        XmlElement pos_x = xmlDoc.CreateElement("x");
                        pos_x.InnerText = obj.transform.position.x + "";

                        XmlElement pos_y = xmlDoc.CreateElement("y");
                        pos_y.InnerText = obj.transform.position.y + "";

                        XmlElement pos_z = xmlDoc.CreateElement("z");
                        pos_z.InnerText = obj.transform.position.z + "";

                        position.AppendChild(pos_x);
                        position.AppendChild(pos_y);
                        position.AppendChild(pos_z);

                        //rotation
                        XmlElement rotation = xmlDoc.CreateElement("rotation");
                        XmlElement rot_x = xmlDoc.CreateElement("x");
                        rot_x.InnerText = obj.transform.rotation.eulerAngles.x + "";

                        XmlElement rot_y = xmlDoc.CreateElement("y");
                        rot_y.InnerText = obj.transform.rotation.eulerAngles.y + "";

                        XmlElement rot_z = xmlDoc.CreateElement("z");
                        rot_z.InnerText = obj.transform.rotation.eulerAngles.z + "";

                        rotation.AppendChild(rot_x);
                        rotation.AppendChild(rot_y);
                        rotation.AppendChild(rot_z);

                        //scale;
                        XmlElement scale = xmlDoc.CreateElement("scale");
                        XmlElement scale_x = xmlDoc.CreateElement("x");
                        scale_x.InnerText = obj.transform.localScale.x + "";

                        XmlElement scale_y = xmlDoc.CreateElement("y");
                        scale_y.InnerText = obj.transform.localScale.y + "";

                        XmlElement scale_z = xmlDoc.CreateElement("z");
                        scale_z.InnerText = obj.transform.localScale.z + "";

                        scale.AppendChild(scale_x);
                        scale.AppendChild(scale_y);
                        scale.AppendChild(scale_z);

                        transform.AppendChild(position);
                        transform.AppendChild(rotation);
                        transform.AppendChild(scale);

                        gameObject.AppendChild(transform);
                        scenes.AppendChild(gameObject);
                    }
                }

                root.AppendChild(scenes);
                xmlDoc.AppendChild(root);
                xmlDoc.Save(filepath);
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("export success");
    }
	
	[MenuItem( "Custom Editor/Export Scene")]
	static void ExportScene()
	{
		Caching.CleanCache();
		
		string targetPath = EditorUtility.SaveFilePanel("Save Resource", "/StreamingAssets/", "New Resource", "unity3d");
		if( targetPath.Length != 0 )
		{
			string[] scenes = {"Assets/UnityDemo.unity"};
			
			BuildPipeline.BuildPlayer( scenes, targetPath, BuildTarget.StandaloneWindows, BuildOptions.BuildAdditionalStreamedScenes );
		}
	}
	
	[MenuItem( "Custom Editor/Export Dictionary to xml" )]
	static void ExportDirToXml()
	{
		
		string filepath = Application.dataPath + "/my.xml";
        if (File.Exists(filepath))
        {
            File.Delete(filepath);
        }

		
	}
}
