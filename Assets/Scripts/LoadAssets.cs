using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadAssets : MonoBehaviour {

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
	
	public static readonly string AssetSuffix = ".assetbundle";
	public static Dictionary<string, string[]> AssetMap;
	
	void Start () {
		initAssetMap();
		loadAssets();
	}

	private void initAssetMap()
	{
		AssetMap = new Dictionary<string, string[]>();
	
		AssetMap.Add( "cube_1", null);
		AssetMap.Add( "cube_2", null);
		//AssetMap.Add( "all", new string[]{"cube_1", "cube_2"});
	}
	
	private void loadAssets()
	{
		foreach( var item in AssetMap ){
			string filename = item.Key + AssetSuffix;
			StartCoroutine(DownloadAssets( filename ));
		}
	}
	
	IEnumerator DownloadAssets( string filename )
	{
		string url = rootPath + filename;
		WWW asset = WWW.LoadFromCacheOrDownload( url, 2 ); 
		yield return asset;
	
		AssetBundle bundle = asset.assetBundle;
		if( asset.error != null ){
			Debug.Log( "Error: " + asset.error );
		}else{
			Debug.Log( "dowload Asset" + filename + " successed. " + url );
			
			int index = filename.LastIndexOf( "." );
			string assetName = filename.Substring( 0, index );
			
			GameObject obj;
			string[] list = AssetMap[assetName];
			if( list == null || list.Length == 0 ){
				obj = (GameObject)Instantiate( bundle.mainAsset );
				obj.SetActive( true );
			}else{
				foreach( string name in list )
				{
					obj = (GameObject)Instantiate( bundle.Load( name ));
					obj.SetActive( true );
				}
			}
		}
		
		bundle.Unload(false);
	}
}
