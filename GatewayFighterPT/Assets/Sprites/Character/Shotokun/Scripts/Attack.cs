using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Box;

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
            manager.frameCounter = 0;
        }

        // Start is called before the first frame update
        public void StateStart()
        {

        }

        // Update is called once per frame
        public void StateUpdate()
        {
            waitForStartUp -= Time.fixedDeltaTime;

            if(attacking == true && slashTracking >= 0)//Slash tracking during startup and before active frames
            {
                manager.frameCounter += (1f/60f);
                //Debug.Log(manager.name + " is at " + manager.frameCounter * 60);
                slashTracking -= Time.fixedDeltaTime;
                manager.rb.velocity = new Vector2(aimVector.x * manager.moveSpeed * manager.dashStrength * Time.fixedDeltaTime, aimVector.y * manager.moveSpeed * manager.dashStrength * Time.fixedDeltaTime);
            }
            else if (slashTracking < 0)//Stop tracking once the move is active + remove dashStrength to weaken or stop velocity
            {
                if (manager.grounded == true) //Animation finish event resets state to J
                    manager.rb.velocity = new Vector2(aimVector.x * manager.moveSpeed / 10f * Time.fixedDeltaTime, aimVector.y * manager.moveSpeed / 10f * Time.fixedDeltaTime);
                else
                {
                    //Reset aerial gravity and velocity
                    manager.rb.gravityScale = 5;
                    manager.rb.AddForce(new Vector2(aimVector.x, aimVector.y));//Can't use jump state because that would cancel the animation
                    manager.airAttack = true;
                }
            }

            if (waitForStartUp < 0)
            {
                if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) == 1 && attacking == false)//Hold check
                {
                    manager.DashCheck();
                    manager.anim.Play("4_Hold");
                    aimVector = new Vector2(Mathf.Round(Input.GetAxisRaw(manager.myAxisX)), Mathf.Round(Input.GetAxisRaw(manager.myAxisY)));

                    if (manager.grounded == true)
                    {
                        //Decay velocity until Abs(manager.rb.velocity.x) <= 1 then set to Vector2.zero
                        if (manager.rb.velocity.x > 1)
                        {
                            manager.rb.velocity = new Vector2(manager.rb.velocity.x - Time.fixedDeltaTime * 50f, manager.rb.velocity.y);
                        }
                        else if (manager.rb.velocity.x < 1)
                        {
                            manager.rb.velocity = new Vector2(manager.rb.velocity.x + Time.fixedDeltaTime * 50f, manager.rb.velocity.y);
                        }

                        if (Mathf.Abs(manager.rb.velocity.x) <= 1)
                        {
                            manager.rb.velocity = Vector2.zero;
                        }
                    }
                    else if (manager.grounded == false)
                    {
                        //Reset aerial gravity and velocity
                        manager.rb.gravityScale = 5;
                        manager.rb.AddForce(new Vector2(aimVector.x, aimVector.y));
                    }
                }
                else if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) != 1 && Mathf.Abs(Mathf.Round(Input.GetAxis(manager.myAxisX))) != 1 && Mathf.Abs(Mathf.Round(Input.GetAxis(manager.myAxisY))) != 1 && attacking == false)//parry check
                {
                    //Was Fire1 released and the Input Axis is < 1?
                    attacking = true;
                    manager.anim.Play("6_Parry");
                }
                else if (Input.GetAxis(manager.myAxisAttack) != 1 && (Mathf.Abs(Input.GetAxis(manager.myAxisX)) > 0.5f || Mathf.Abs(Input.GetAxis(manager.myAxisY)) > 0.5f) && attacking == false)//slash check
                {
                    //was Fire1 released and the Input Axis is > 0.5f?
                    attacking = true;
                    manager.anim.Play("5_Slash");
                }
            }
        }
    }
}