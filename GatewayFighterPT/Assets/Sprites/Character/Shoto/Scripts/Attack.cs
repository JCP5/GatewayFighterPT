using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Attack : IShotoBase
    {
        CharacterState manager;
        bool attacking;
        public Vector2 aimVector;
        public float waitForStartUp = 4f/60f;
        public float slashTracking = 12f / 60f;

        public Attack(CharacterState managerRef, Vector2 v2)
        {
            manager = managerRef;
            attacking = false;
            aimVector = v2;
        }

        // Start is called before the first frame update
        public void StateStart()
        {

        }

        // Update is called once per frame
        public void StateUpdate()
        {
            waitForStartUp -= Time.fixedDeltaTime;

            if(attacking == true && slashTracking >= 0)
            {
                slashTracking -= Time.fixedDeltaTime;
                manager.rb.velocity = new Vector2(aimVector.x * manager.moveSpeed * manager.dashStrength * Time.fixedDeltaTime, aimVector.y * manager.moveSpeed * manager.dashStrength * Time.fixedDeltaTime);
            }
            else if (slashTracking < 0)//remove dashStrength to weaken velocity
            {
                if (manager.grounded == true)
                    manager.rb.velocity = new Vector2(aimVector.x * manager.moveSpeed / 10f * Time.fixedDeltaTime, aimVector.y * manager.moveSpeed / 10f * Time.fixedDeltaTime);
                else
                {
                    //Reset aerial gravity and velocity
                    manager.rb.gravityScale = 5;
                    manager.rb.AddForce(new Vector2(aimVector.x, aimVector.y));
                }
            }

            if (waitForStartUp < 0)
            {
                if (Mathf.Abs(Input.GetAxis("Fire1")) == 1 && attacking == false)//Hold check
                {
                    manager.DashCheck();
                    manager.anim.Play("4_Hold");
                    aimVector = new Vector2(Input.GetAxisRaw(manager.myAxisX), Input.GetAxisRaw(manager.myAxisY));

                    if (manager.grounded == false)
                    {
                        //Reset aerial gravity and velocity
                        manager.rb.gravityScale = 5;
                        manager.rb.AddForce(new Vector2(aimVector.x, aimVector.y));
                    }
                }
                else if (Mathf.Abs(Input.GetAxis("Fire1")) != 1 && Mathf.Abs(Input.GetAxis(manager.myAxisX)) != 1 && Mathf.Abs(Input.GetAxis(manager.myAxisY)) != 1 && attacking == false)//parry check
                {
                    attacking = true;
                    manager.anim.Play("6_Parry");
                }
                else if (Input.GetAxis("Fire1") != 1 && (Input.GetAxis(manager.myAxisX) != 0 || Mathf.Abs(Input.GetAxis(manager.myAxisY)) != 0) && attacking == false)//slash check
                {
                    attacking = true;
                    manager.anim.Play("5_Slash");
                }
            }
        }
    }
}