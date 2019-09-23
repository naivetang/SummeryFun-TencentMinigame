
using UnityEngine;

namespace ETModel
{
    public class SingletonMono<T> : MonoBehaviour, ISingleton<T> where T : SingletonMono<T>
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance != null)
                        DontDestroyOnLoad(_instance.gameObject);
                }

                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).ToString() + "_Singleton");
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }

                //Debug.Assert(_instance == null, typeof(T).ToString() + " must add to gameobject.");
                return _instance;
            }
        }

        public static T GetMonoInstance()
        {
            if (_instance == null)
            {
                if (Application.isPlaying)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this as T;
        }

        public virtual void Init() { }

        public void Free()
        {
            FreeSingleton();
            _instance = null;
            DestroyObject(gameObject);
        }

        protected virtual void FreeSingleton() { }
    }
}
