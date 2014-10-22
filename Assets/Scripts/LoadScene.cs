using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {
	
	public static readonly string rootPath = 
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
	"File://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_IPHONE
	Application.dataPath + "/Raw/";
#elif UNITY_ANDROID
	"jar:file://" + Application.dataPath + "!/Assets/";
#else
	string.Empty;
#endif
	
	public static readonly string SceneSuffix = ".assetbundle";
	private bool isLoadScene = false;
	
	private AsyncOperation Sync;
	private string sceneName = "UnityDemo";
	
	void Start () {;
		//Application.LoadLevel( "UnityDemo" );
		Debug.Log( "Start" );
	}
	
	void Awake()
	{
		Debug.Log( "Awake" );
	}
	
	void Update()
	{
		//yield return new WaitForSeconds(2);
	}
	
	void OnGUI()
	{
		if(GUI.Button( new Rect( 0, 0, 80, 26), "loadScene" )){
			if(!isLoadScene)
				loadScene();
		}
	}
	
	private void loadScene()
	{
		isLoadScene = true;
		
		string name = sceneName + SceneSuffix;
		StartCoroutine( DownloadScene( name ));
	}

	IEnumerator DownloadScene( string name )
	{
		Debug.Log( "download scene " + name );
		
		string url = rootPath + name;
		WWW scene = WWW.LoadFromCacheOrDownload( url,1 );
		yield return scene;
		Debug.Log( "download completet" );
		
		//var bundle = scene.assetBundle
		//Application.LoadLevel( "UnityDemo" );
		Sync = Application.LoadLevelAsync( sceneName );
		yield return Sync;
		//bundle = null;
	}
}
