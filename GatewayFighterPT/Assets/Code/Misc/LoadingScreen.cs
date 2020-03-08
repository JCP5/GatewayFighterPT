using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen loadingScreen;
    public AudioClip ambience;

    [SerializeField]
    GameObject loadingScreenGO;

    [SerializeField]
    Slider slider;
    // Start is called before the first frame update

    private void Awake()
    {
        loadingScreen = this;
        slider = GetComponentInChildren<Slider>();
        loadingScreenGO.SetActive(false);
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (loadingScreen == null)
            loadingScreen = this;
        else if (loadingScreen != this)
            Destroy(this.gameObject);
    }

    public void LoadLevel(string sceneName, AudioClip ac)
    {
        loadingScreenGO.SetActive(true);

        Persistent.instance.bgm.Stop();
        Persistent.instance.bgm.clip = ambience;
        Persistent.instance.bgm.Play();

        StartCoroutine(LoadAsync(sceneName, ac));
    }

    IEnumerator LoadAsync(string sceneName, AudioClip ac)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            Debug.Log(progress);

            yield return null;
        }

        Persistent.instance.bgm.Stop();
        Persistent.instance.bgm.clip = ac;
        Persistent.instance.bgm.Play();

        loadingScreenGO.SetActive(false);

        yield return null;
    }
}
