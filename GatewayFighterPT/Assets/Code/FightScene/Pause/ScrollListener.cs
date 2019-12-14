using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Assets.Code.ButtonScrollUI
{
    public class ScrollListener : MonoBehaviour, ISelectHandler
    {
        ScrollWithButton swb;
        public int index;
        public AudioClip ac;
        public AudioClip defaultClip;

        void Start()
        {
            swb = GetComponentInParent<ScrollWithButton>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            swb.SetVertical(index);
        }

        public void Press()
        {
            FindObjectOfType<AudioSource>().clip = ac;
            FindObjectOfType<AudioSource>().Play();
        }

        public void Stage(string s)
        {
            Persistent.instance.bgm.clip = defaultClip;
            Persistent.instance.bgm.Play();
            Time.timeScale = 1f;
            SceneManager.LoadScene(s);
        }
    }
}