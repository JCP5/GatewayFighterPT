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
            Vector3 contactDir = collision.transform.position - transform.position;
            Vector3 contactPoint = transform.position + contactDir;

            if (collision.GetComponent<HitBox>() && collision.transform.rotation.y != this.transform.rotation.y || collision.GetComponent<ClashBox>() && collision.transform.rotation.y != this.transform.rotation.y)
            {
                Instantiate(manager.vfx["Parry"], contactPoint, Quaternion.Euler(-90f, 0, 0));

                collision.GetComponentInParent<CharacterState>().activeState = new Parried(collision.GetComponentInParent<CharacterState>(), manager.transform);

                //manager.AnimationFinish();//Check if grounded and set appropriate state
            }
        }
    }
}