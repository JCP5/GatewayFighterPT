using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Shoto;
using Assets.Code.FightScene;
using Assets.Code.CharacterControl;

namespace Assets.Code.Box
{
    public class HitBox : MonoBehaviour
    {
        CharacterState manager;

        private void Start()
        {
            manager = this.GetComponentInParent<CharacterState>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<ClashBox>() && collision.transform.parent.tag != this.transform.parent.tag)
            {
                collision.GetComponent<ClashBox>().ClashEffect(manager, this.GetComponent<Collider2D>());
            }
            else if (collision.GetComponent<HurtBox>() && collision.transform.parent.tag != this.transform.parent.tag)
            {
                //this.GetComponentInParent<CharacterState>().activeState = new PostRound(this.GetComponentInParent<CharacterState>(), true);
                manager.HitLanded();
                collision.GetComponent<HurtBox>().HitTaken();
                FindObjectOfType<FightManager>().UpdateWins(this.transform.parent.GetComponent<CharacterState>().playerNumber);
            }
        }
    }
}