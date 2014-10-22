using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityDemo.interfaces.api;
using UnityDemo.interfaces.manager;
using UnityDemo.manager;
using System.Collections;
using UnityEngine;

namespace UnityDemo.api
{
    public class GlobalApi: MonoBehaviour, IGlobalApi
    {
		private static IGlobalApi _instance;
		private UILabel mLog;

		public static IGlobalApi getInstance()
		{
			if( _instance == null )
			{
				GameObject obj = new GameObject();
				GameObject.DontDestroyOnLoad( obj );
				obj.name = "_GlobalApi";
				_instance = obj.AddComponent<GlobalApi>();
			}	
			
			return _instance;
		}
		
        public void init()
        {
			loadFileManager = createApi<LoadFileManager>();
			loadManager = createApi<LoadManager>();
		}
		
		private T createApi<T>() where T : Component{
			T api = this.GetComponentInChildren<T>();
			if( api == null ){
				GameObject obj = new GameObject(typeof(T).Name );
				api = obj.AddComponent<T>();
				api.transform.parent = this.gameObject.transform;
			}
			
			return api;
		}
		
        public ILoadFileManager loadFileManager { set; get; }
        public ILoadManger loadManager { set; get; }

		public UILabel LogLable
		{
			set {
				mLog = value;
			}
		}
		
		public void Log( string msg )
		{
			if(mLog)
				mLog.text = msg;
		}
    }
}
