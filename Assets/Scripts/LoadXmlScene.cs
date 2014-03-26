using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;


public class LoadXmlScene : MonoBehaviour {

	// Use this for initialization
	private static readonly string RES_URL = "file:///E:/wuhuilin/Unity/Resources/Prefab/";
	
	void Start () {
        LoadScene();
	}
	
    private void LoadScene()
    {
#if UNITY_EDITOR
        string filepath = Application.dataPath + "/StreamingAssets/UnityDemo.xml";
#elif UNITY_IPHONE
        string filepath = Application.dataPath + "/Raw" + "UnityDemo.xml";
#else
		string filepath = Application.dataPath + "/StreamingAssets/UnityDemo.xml";
#endif
        if (File.Exists(filepath)) 
        {
            Debug.Log("filepath " + filepath);

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
                            else if( node.Name == "scale")
                            {
                                scale = GetVector3FromXmlElement(node);
                            }
                        }
                    }
                
                   // GameObject obj = (GameObject)Instantiate(Resources.Load(asset), pos, Quaternion.Euler(rot));
                    ArrayList param = new ArrayList();
                    param.Add(name);
                    param.Add(pos);
                    param.Add(rot);
                    param.Add(scale);

                    StartCoroutine( "LoadGameObject", param );
                }
            }

            Debug.Log("Load over!");
        }
    }

    private Vector3 GetVector3FromXmlElement(XmlElement transform )
    {
        Vector3 vec = Vector3.zero;

        foreach (XmlElement node in transform.ChildNodes)
        {
            switch (node.Name)
            {
                case "x":
                    vec.x = float.Parse(node.InnerText);
                    break;

                case "y":
                    vec.y = float.Parse(node.InnerText);
                    break;

                case "z":
                    vec.z = float.Parse(node.InnerText);
                    break;
              
            }
        }

        return vec;
    }

    IEnumerator LoadGameObject( ArrayList param )
    {
		string filename = param[0].ToString() + ".unity3d";
        Debug.Log("start load GameObject " + filename);
		string url = RES_URL + filename;
		
        WWW www = WWW.LoadFromCacheOrDownload( url, 1);
        yield return www;

        Vector3 pos = (Vector3)param[1];
        Vector3 rot = (Vector3)param[2];
        Vector3 scale = (Vector3)param[3];

        GameObject gameobject = (GameObject)Instantiate( www.assetBundle.mainAsset, pos, Quaternion.Euler(rot));
        gameobject.transform.localScale = scale;
        www.assetBundle.Unload(false);
        Debug.Log("load " + name + " over!" );
    }
}
 