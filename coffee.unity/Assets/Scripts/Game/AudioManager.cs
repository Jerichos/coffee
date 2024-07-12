using System;
using UnityEngine;

namespace POLYGONWARE.Coffee.Game
{
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("AudioManager instance is null!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void PlaySfx(AudioClip clip, Vector3 position = default)
    {
        if (!clip)
        {
            Debug.LogError("Trying to play null audio clip!");
            return;
        }
        
        if (_instance == null || _instance._audioSource == null)
        {
            AudioSource.PlayClipAtPoint(clip, position);
            return;
        }
        
        Instance._audioSource.PlayOneShot(clip);
    }
}
}