using UnityEngine;

using System;
using System.Collections;
using System.IO;

public class LoadScene : MonoBehaviour {

	//private string SceneUrl = Application.dataPath + "/Res/UnityDemo.unity3d";
	public static readonly string cubeUrl = "File:///" + Application.dataPath + "/Res/cube.unity3d";
	public static readonly string sceneUrl = "File:///" + Application.dataPath + "/Res/UnityDemo.unity3d";
	
	void Start () {
		Debug.Log( "..Start.." + cubeUrl );
		StartCoroutine( DownloadAssetAndScene() );
	}
	
	IEnumerator DownloadAssetAndScene()
	{
		WWW asset = new WWW( cubeUrl );  //WWW.LoadFromCacheOrDownload( cubeUrl, 1 );
		yield return asset;
				
		AssetBundle bundle = asset.assetBundle;
		GameObject obj = (GameObject)Instantiate( asset.assetBundle.mainAsset );
		obj.SetActive( true );
		
		Debug.Log( "dowload Asset..." + obj.name );
		bundle.Unload(false);
	}
}
