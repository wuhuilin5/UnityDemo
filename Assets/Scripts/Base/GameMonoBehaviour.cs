
using UnityEngine;

namespace UnityDemo
{
    public class GameMonoBehaviour : MonoBehaviour
    {
        //Awake -> onEnable -> Start -> Update -> FixedUpdate -> LateUpdate -> OnGUI -> Reset -> OnDisable -> OnDestory

        //Awake is called when the script instance is being loaded.
        public virtual void Awake() { }

        //This function is called when the object becomes enabled and active.
        public virtual void OnEnable() { }

        //Start is called just before any of the Update methods is called the first time.
        public virtual void Start() { }

        //Update is called every frame, if the MonoBehaviour is enabled.
        public virtual void Update() { }

        //FixedUpdate is called every fixed framerate frame, if the MonoBehaviour is enabled
        public virtual void FixedUpdate() { }

        //LateUpdate is called every frame, if the Behaviour is enabled.
        public virtual void LateUpdate() { }

        //OnGUI is called for rendering and handling GUI events.
        public virtual void OnGUI() { }

        //Reset to default values.
        public virtual void Reset() { }

        //This function is called when the behaviour becomes disabled () or inactive.
        public virtual void OnDisable() { }

        //This function is called when the MonoBehaviour will be destroyed.
        public virtual void OnDestroy() { }

        //This function is called after a new level was loaded.
        public virtual void OnLevelWasLoaded(int level) { }
    }
}
