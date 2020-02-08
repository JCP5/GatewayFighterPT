using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Effects
{
    public class SFXHolder : MonoBehaviour
    {
        public Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();
        public AudioClip[] sfxList = new AudioClip[0];
        public AudioSource[] Source;

        // Start is called before the first frame update
        void Start()
        {
            foreach (AudioClip ac in sfxList)
            {
                sfx.Add(ac.name, ac);
            }

            Source = GetComponents<AudioSource>();
        }
        
        public void PlaySwordWhiff()
        {
            foreach(AudioSource aso in Source)
            {
                if(aso.isPlaying == false)
                {
                    aso.clip = sfx["SwordWhiff"];
                    aso.Play();
                    break;
                }
            }
        }

        public void PlayParryWhiff()
        {
            foreach (AudioSource aso in Source)
            {
                if (aso.isPlaying == false)
                {
                    aso.clip = sfx["ParryWhiff"];
                    aso.Play();
                    break;
                }
            }
        }

        public void PlayHelmBreakerWhiff()
        {
            foreach (AudioSource aso in Source)
            {
                if (aso.isPlaying == false)
                {
                    aso.clip = sfx["HelmBreakerWhiff"];
                    aso.Play();
                    break;
                }
            }
        }

        public void PlayHelmBreakerThud()
        {
            foreach (AudioSource aso in Source)
            {
                if (aso.isPlaying == false)
                {
                    aso.clip = sfx["HelmBreakerThud"];
                    aso.Play();
                    break;
                }
            }
        }
    }
}