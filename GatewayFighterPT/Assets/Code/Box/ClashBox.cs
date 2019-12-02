using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Shoto;

public class ClashBox : MonoBehaviour
{
    CharacterState manager;

    private void Start()
    {
        manager = this.GetComponentInParent<CharacterState>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ClashBox>() && collision.transform.parent.tag != this.transform.parent.tag)
        {
            manager.activeState = new Clash(manager, CalculateFrames(manager.frameCounter, collision.GetComponentInParent<CharacterState>().frameCounter), manager.t);
        }
    }

    float CalculateFrames(float a, float b)
    {
        return a - b;
    }
}
