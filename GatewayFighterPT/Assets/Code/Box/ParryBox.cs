using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Shoto;

namespace Assets.Code.Box
{
    public class ParryBox : MonoBehaviour
    {
        CharacterState manager;

        private void Start()
        {
            manager = this.GetComponentInParent<CharacterState>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<HitBox>() || collision.GetComponent<ClashBox>())
            {
                Instantiate(manager.vfx["Parry"], transform.position, Quaternion.identity);

                collision.GetComponentInParent<CharacterState>().activeState = new Parried(collision.GetComponentInParent<CharacterState>());

                manager.AnimationFinish();//Check if grounded and set appropriate state
            }
        }
    }
}