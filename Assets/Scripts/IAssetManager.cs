using System;
using System.Collections;
using UnityEngine;

using UnityDemo.manager;

namespace UnityDemo.interfaces.manager
{
    public interface IAssetManager
    {
		void LoadAsset(string path, LoadFunishHandler callback = null);
	}

}