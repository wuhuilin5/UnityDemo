using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UnityDemo
{
	public class ResUpdate : MonoBehaviour 
	{
		public static readonly string VERSION_FILE = "version.txt";
		public static readonly string LOCAL_RES_URL = "file:://" + Application.dataPath + "/Res/";
		public static readonly string SERVER_RES_URL = "file::///E:/Res/";
		public static readonly string LOCAL_RES_PATH = Application.dataPath + "/Res/";
		
		private Dictionary<string, string> LocalResVersion;
		private Dictionary<string, string> ServerResVersion;
		
		private List<string> NeedDownFiles;
		private bool NeedUpdateLocalVersionFile = false;
		
		public delegate void HandleFinishDownload( WWW www );
		
		void Start () 
		{
			init();
			loadVersionFile();
		}
		
		private void init()
		{
			LocalResVersion = new Dictionary<string, string>();
			ServerResVersion = new Dictionary<string, string>();
			NeedDownFiles = new List<string>();	
		}
		
		private void loadVersionFile()
		{
			string localUrl = LOCAL_RES_URL + VERSION_FILE;
			StartCoroutine(Download( localUrl, delegate(WWW localVersion )	{	
				//保存本地的version
				ParseVersionFile(localVersion.text, LocalResVersion );	
				
				string serverUrl = SERVER_RES_URL + VERSION_FILE;
				StartCoroutine( Download( serverUrl, delegate(WWW serverVersion) {
					//保存服务器的version
					ParseVersionFile( serverVersion.text, ServerResVersion );
					
					//比较版本，计算出需要重新加载的资源
					CompareVersion();
					
					//加载需要更新的资源
					DownLoadRes();
				}));
			}));
		}
			
		private void DownLoadRes()
		{
			if( NeedDownFiles.Count == 0 ){
				UpdateLocalVersionFile();
				return;
			}
		
			string file = NeedDownFiles[0];
			NeedDownFiles.RemoveAt(0);
		
			string url = SERVER_RES_URL + file;
			StartCoroutine( Download( url, delegate(WWW www) {
				ReplaceLocalRes( file, www.bytes );
				DownLoadRes();						
			}));
		}
		
		private void ReplaceLocalRes( string fileName, byte[] data )
		{
			string filepath = LOCAL_RES_PATH + fileName;
			FileStream stream = new FileStream( filepath, FileMode.Create );
			
			stream.Write( data, 0, data.Length );
			stream.Flush();
			stream.Close();
		}
		
		private IEnumerator show()
		{
			WWW asset = new WWW(LOCAL_RES_URL + "cube.assetbundle" );
			yield return asset;
			
			AssetBundle bundel = asset.assetBundle;
			Instantiate( bundel.Load( "cube" ));
			bundel.Unload(false);
		}
		
		private void UpdateLocalVersionFile()
		{
			if( NeedUpdateLocalVersionFile )
			{
				StringBuilder versins = new StringBuilder();
				
				foreach( var item in ServerResVersion )
				{
					versins.Append(item.Key).Append(",").Append(item.Value).Append("\n");
				}
				
				string filePath = LOCAL_RES_PATH + VERSION_FILE;
				FileStream stream = new FileStream( filePath, FileMode.Create );
				
				byte[] data = Encoding.UTF8.GetBytes(versins.ToString());
				
				stream.Write( data, 0, data.Length );
				stream.Flush();
				stream.Close();
			}
			
			//显示
			StartCoroutine(show());
		}
		
		private void CompareVersion()
		{
			foreach( var version in ServerResVersion )
			{
				string filename = version.Key;
				string serverMd5 = version.Value;
				
				if( !LocalResVersion.ContainsKey(filename)){
					NeedDownFiles.Add(filename);
				}else{
					string localMd5;
					LocalResVersion.TryGetValue( filename, out localMd5 );
					if( !serverMd5.Equals(localMd5))
					{
						NeedDownFiles.Add( filename );
					}
				}
			}
			
			NeedUpdateLocalVersionFile = NeedDownFiles.Count > 0;
		}
		
		private void ParseVersionFile( string content, Dictionary<string, string> dict )
		{
			if( content == null || content.Length == 0 )
			{
				return;
			}
			
			string[] items = content.Split('\n');
			foreach( string item in items )
			{
				string[] info = item.Split(',');
				if( info != null && info.Length == 2 )
				{
					dict.Add( info[0], info[1] );   //key:filename, value: md5
				}
			}
		}
		
		private IEnumerator Download( string url, HandleFinishDownload callback )
		{
			WWW www = new WWW(url);
			yield return www;
			
			if( callback != null ){
				callback(www);
			}
			
			www.Dispose();
		}
	}
}
