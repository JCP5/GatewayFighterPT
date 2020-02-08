using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.CharacterControl;

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

            if (collision.GetComponent<HitBox>() && CheckInFront(collision.transform.parent)|| collision.GetComponent<ClashBox>() && CheckInFront(collision.transform.parent))
            {
                Instantiate(manager.vfx["Parry"], contactPoint, Quaternion.Euler(-90f, 0, 0));
                Debug.Log(transform.InverseTransformPoint(collision.transform.parent.position));

                collision.GetComponentInParent<CharacterState>().Parried(collision.GetComponentInParent<CharacterState>(), manager);//.activeState = new Parried(collision.GetComponentInParent<CharacterState>(), manager.transform);

                //manager.AnimationFinish();//Check if grounded and set appropriate state
            }
        }

        private bool CheckInFront(Transform t)
        {
            if (transform.InverseTransformPoint(t.position).x > -0.2f)
                return true;
            else
                return false;
        }
    }
}