using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Shoto;
using Assets.Code.Effects;

public class ClashBox : MonoBehaviour
{
    CharacterState manager;
    Transform myTrans;
        
    Vector3 contactDirection;
    Vector3 contactPoint;


    private void Start()
    {
        myTrans = this.transform;
        manager = this.GetComponentInParent<CharacterState>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterState opponent = collision.GetComponentInParent<CharacterState>();

        if (collision.GetComponent<ClashBox>() && collision.transform.parent.tag != this.transform.parent.tag)
        {
            contactDirection = collision.transform.position - myTrans.position;
            contactPoint = myTrans.position + contactDirection;

            Instantiate(manager.vfx["Clash"], contactPoint, Quaternion.identity);
            manager.activeState = new Clash(manager, CalculateFrames(manager.frameCounter, opponent.frameCounter), manager.t, opponent.transform);
        }
    }

    float CalculateFrames(float a, float b)
    {
        return a - b;
    }
}
