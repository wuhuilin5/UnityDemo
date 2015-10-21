
using UnityDemo.Managers;

namespace UnityDemo
{
    public sealed class GameHook
    {
        public static void OnInit()
        {
            DebugInfo.Log("GameHook OnInit");
        }
    }
}