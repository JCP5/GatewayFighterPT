using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Shoto;
using Assets.Code.FightScene;

namespace Assets.Code.Box
{
    public class HitBox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<HurtBox>() && collision.transform.parent.tag != this.transform.parent.tag && collision.GetComponentInParent<CharacterState>().invulToStrike == false)
            {
                collision.GetComponent<HurtBox>().Hit();
                FindObjectOfType<FightManager>().UpdateWins(this.transform.parent.tag);
            }
        }
    }
}