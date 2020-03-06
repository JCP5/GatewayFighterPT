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
            if (transform.parent != null)
                swb = GetComponentInParent<ScrollWithButton>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (swb != null)
                swb.SetVertical(index);
        }

        public void Press()
        {
            Persistent.instance.bgm.Stop();
            Persistent.instance.bgm.clip = ac;
            Persistent.instance.bgm.Play();
        }

        public void Stage(string s)
        {
            Persistent.instance.bgm.Stop();
            Persistent.instance.bgm.clip = defaultClip;
            Persistent.instance.bgm.Play();
            Time.timeScale = 1f;
            SceneManager.LoadScene(s);
        }

        public void HolyOrders()
        {
            int random = Random.Range(1, 3);
            Debug.Log(random);

            if (random == 1)
            {
                Persistent.instance.bgm.Stop();
                Persistent.instance.bgm.clip = ac;
                Persistent.instance.bgm.Play();
            }
            else
            {
                Persistent.instance.bgm.Stop();
                Persistent.instance.bgm.clip = defaultClip;
                Persistent.instance.bgm.Play();
            }
        }
    }
}