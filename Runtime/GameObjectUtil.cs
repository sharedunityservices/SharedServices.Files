using UnityEngine;

namespace Services.Files
{
    public static class GameObjectUtil
    {
        public static T CreateDDOLWith<T>(string name = null) where T : Component
        {
            name ??= typeof(T).Name;
            var gameObject = new UnityEngine.GameObject(name);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            return gameObject.AddComponent<T>();
        }
    }
}