using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
        GetComponent<AudioSource>().ignoreListenerPause = true;
    }
}
