

namespace UnityDemo.Managers
{
    public class PathManager : Singleton<PathManager>
    {
        public PathManager()
        {
        }

        public string GetScenePath(string sceneName)
        {
            return string.Concat("Scenes/", sceneName, ".unity");
        }

        public string GetCreatureModelPrefab(string name)
        {
            return string.Concat("Creature/", name, ".prefab");
        }

        public string GetEquipModelPrefab(string name)
        {
            return string.Concat("Equip/", name, ".prefab");
        }
    }
}
