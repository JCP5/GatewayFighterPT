using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Persistent : MonoBehaviour
{
    public static Persistent instance;
    public AudioSource bgm;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        bgm = GetComponent<AudioSource>();
    }
}
