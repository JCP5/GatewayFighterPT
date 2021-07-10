using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Shoto;
using Assets.Code.FightScene;
using Assets.Code.Effects;
using Assets.Code.CharacterControl;

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

        public void HitTaken()
        {
            manager.HitTaken();
        }
    }
}