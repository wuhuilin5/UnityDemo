using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityDemo.Interfaces
{
    public interface IPathManager
    {
        string GetScenePath(string sceneName);
        string GetCreatureModelPrefab(string name);
        string GetEquipModelPrefab(string name);
    }
}
