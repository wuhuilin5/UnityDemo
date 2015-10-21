using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityDemo.Interfaces
{
    public interface ISceneManager
    {
        void ShowScene(string sceneName, Action<bool> onFinish = null, Action<float> onProgress = null);
    }
}
