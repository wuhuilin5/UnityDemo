using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;

using UnityDemo.Utils;
using UnityDemo.manager;
using UnityDemo.interfaces.manager;
using UnityDemo;

public class LoadXmlScene : MonoBehaviour {

    private ILoadManger loadMgr;
	
	void Start () {
        loadMgr = Globals.Api.loadManager;
		
		//loadSceneXml();
        LoadScene();
	}
	
	private void loadSceneXml()
	{
		ArrayList list = new ArrayList();
		list.Add( "Terrain" );
		list.Add( "Main Camera" );
		
		foreach( string name in list ){
		string filepath = FileUtils.getAssetBundlePath( name );
		loadMgr.loadUrl( filepath, delegate( AssetBundle asset ){
		  	GameObject obj = (GameObject)Instantiate(asset.mainAsset);
			obj.SetActive(true);
			//obj.transform.position = new Vector3( 0, 0, 0 );
		  //obj.transform.position = Vector3(0,0,0);
		});
		}
	}
	
    private void LoadScene()
    {
       	//string filepath = Application.streamingAssetsPath + "/UnityDemo.xml";
		string filepath = FileUtils.getXmlPath( "UnityDemo" );
        Debug.Log("filepath " + filepath);
       	
        if (File.Exists(filepath)) 
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(filepath);

            XmlNodeList nodelist = xmldoc.SelectSingleNode("gameObjects").ChildNodes;
            foreach (XmlElement scene in nodelist)
            {
                Debug.Log("scene  " + scene.Name );

                foreach (XmlElement gameobject in scene.ChildNodes)
                {
                    string name = gameobject.GetAttribute("name");
                   // string asset = "Prefabs/" + gameobject.GetAttribute("name");

                    Vector3 pos = Vector3.zero;
                    Vector3 rot = Vector3.zero;
                    Vector3 scale = Vector3.zero;

                    foreach (XmlElement transform in gameobject.ChildNodes)
                    {
                        foreach (XmlElement node in transform.ChildNodes)
                        {
                            if (node.Name == "position")
                            {
                                pos = GetVector3FromXmlElement(node);
                            }
                            else if (node.Name == "rotation")
                            {
                                rot = GetVector3FromXmlElement(node);
                            }
                            else if (node.Name == "scale")
                            {
                                scale = GetVector3FromXmlElement(node);
                            }
                        }
                    }
                
                    ArrayList param = new ArrayList();
                    param.Add(name);
                    param.Add(pos);
                    param.Add(rot);
                    param.Add(scale);

                    LoadGameObject(param);
                }
            }

            Debug.Log("Load over!");
        }
    }

    private Vector3 GetVector3FromXmlElement(XmlElement transform)
    {
        Vector3 vec = Vector3.zero;
        vec.x = float.Parse(transform.GetAttribute("x"));
        vec.y = float.Parse(transform.GetAttribute("y"));
        vec.z = float.Parse(transform.GetAttribute("z"));

        return vec;
    }

    private void LoadGameObject( ArrayList param )
    {
        string name = param[0].ToString();
        string url = FileUtils.getAssetBundlePath(name);
	
        loadMgr.loadUrl(url, delegate( AssetBundle asset)
		{ 	
			Vector3 pos = (Vector3)param[1];
            Vector3 rot = (Vector3)param[2];
            Vector3 scale = (Vector3)param[3];

            GameObject obj = (GameObject)Instantiate(asset.mainAsset, pos, Quaternion.Euler(rot));
            obj.transform.localScale = scale;
		});
   }
}