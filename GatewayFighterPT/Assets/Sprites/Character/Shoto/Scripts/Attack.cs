using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Attack : IShotoBase
    {
        CharacterState manager;
        bool attacking;
        public float waitForStartUp = 3 / 60;

        public Attack(CharacterState managerRef)
        {
            manager = managerRef;
            attacking = false;
            manager.anim.Play("4_Hold", 0, 0);
            //manager.anim.SetInteger("AnimState", 4);
            Debug.Log(manager.anim.GetInteger("AnimState"));
        }

        // Start is called before the first frame update
        public void StateStart()
        {

        }

        // Update is called once per frame
        public void StateUpdate()
        {
            manager.DashCheck();
            waitForStartUp -= Time.fixedDeltaTime;

            if (waitForStartUp <= 0)
            {
                if (Mathf.Abs(Input.GetAxis("Fire1")) != 1 && Mathf.Abs(Input.GetAxis("Horizontal")) != 1 && attacking == false)
                {
                    attacking = true;
                    manager.anim.SetInteger("AnimState", 6);
                }
                else if (Input.GetAxis("Fire1") != 1 && Input.GetAxis("Horizontal") != 0 && attacking == false)
                {
                    attacking = true;
                    manager.anim.SetInteger("AnimState", 5);
                }
            }
        }
    }
}