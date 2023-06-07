using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool isInitialized = false;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (isInitialized)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
        DontDestroyOnLoad(gameObject);

        isInitialized = true;
    }

}
