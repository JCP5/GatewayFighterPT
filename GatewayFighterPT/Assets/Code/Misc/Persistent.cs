using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Persistent : MonoBehaviour
{
    public static Persistent instance;
    public static AudioClip[] ac;
    public static AudioSource fuck;

    private void Awake()
    {
        fuck = GetComponent<AudioSource>();
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(GetComponent<AudioSource>().clip);

        if (GetComponent<AudioSource>().clip == null)
            GetComponent<AudioSource>().clip = ac[SceneManager.GetActiveScene().buildIndex];
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
