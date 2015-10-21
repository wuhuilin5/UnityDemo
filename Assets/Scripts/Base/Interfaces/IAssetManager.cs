using System;
using System.Collections;
using UnityEngine;

using UnityDemo.Manager;

namespace UnityDemo.Interfaces
{
    public delegate void OnLoadFinished(string name, UnityEngine.Object obj);
    public delegate void OnLoadProgress(float value);

    public interface IAssetManager
    {
		void LoadAsset(string path, OnLoadFinished callback = null);
	}

}