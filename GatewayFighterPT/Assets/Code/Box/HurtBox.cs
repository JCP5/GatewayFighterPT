using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Shoto;
using Assets.Code.FightScene;
using Assets.Code.Effects;

namespace Assets.Code.Box
{
    public class HurtBox : MonoBehaviour
    {
        Image fade;
        FightManager fm;
        CharacterState manager;

        private void Start()
        {
            manager = transform.parent.GetComponent<CharacterState>();
            fm = FindObjectOfType<FightManager>();
        }

        public void Hit()
        {
            manager.activeState = new PostRound(manager, false);
            Instantiate(manager.vfx["Hit"], transform.position, Quaternion.identity);
            Debug.Log("Fuck");
        }
    }
}