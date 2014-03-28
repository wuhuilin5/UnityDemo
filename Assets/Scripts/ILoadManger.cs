using System;
using System.Collections;
using UnityEngine;

using UnityDemo.manager;

namespace UnityDemo.interfaces.manager
{
    public interface ILoadManger
    {
        IEnumerator loadUrl(string url, LoadFunishHandler callback = null, string filename = null );  
    }

}
