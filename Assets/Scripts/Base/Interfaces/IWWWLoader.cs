using System;
using UnityEngine;

namespace UnityDemo.Interfaces
{
    public interface IWWWLoader
    {
        void Load(String url, Action<WWW> onLoaded, Action<String> onError = null);
        void Post(String url, byte[] data, Action<WWW> onSuccess, Action<String> onError = null);
        void Destroy(string url);
    }
}
