using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.TimeCop
{
    public class Attack : ITimeCopBase
    {
        TimeCopManager manager;
        bool attacking;
        public Vector2 aimVector;
        public float waitForStartUp = 4f / 60f;
        //tracking time differs based on attack startup
        public float slashTracking = 20f / 60f;

        public Attack(TimeCopManager managerRef, Vector2 v2)
        {
            manager = managerRef;
            aimVector = v2;
            attacking = false;
            manager.frameCounter = 0;
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            waitForStartUp -= Time.fixedDeltaTime;

            WhichAttack();
        }

        void Track()
        {
            if (attacking == true && slashTracking >= 0)//Slash tracking during startup and before active frames
            {
                manager.frameCounter += (1f / 60f);
                //Debug.Log(manager.name + " is at " + manager.frameCounter * 60);
                slashTracking -= Time.fixedDeltaTime;

                if (aimVector != Vector2.zero)
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
        }

        void WhichAttack()
        {
            if (waitForStartUp < 0)
            {
                if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) == 1 && attacking == false)//Hold check
                {
                    manager.anim.Play("Ready");
                    aimVector = new Vector2(Mathf.Round(Input.GetAxisRaw(manager.myAxisX)), Mathf.Round(Input.GetAxisRaw(manager.myAxisY)));

                    VelocityDecay(50f);
                }
                else if (Input.GetAxis(manager.myAxisAttack) != 1 && (Mathf.Abs(Input.GetAxis(manager.myAxisX)) > 0.5f || Mathf.Abs(Input.GetAxis(manager.myAxisY)) > 0.5f) && attacking == false)
                {
                    //was Fire1 released and the Input Axis is > 0.5f?
                    manager.rb.velocity = Vector2.zero;
                    manager.throw1.SetDirection(aimVector);
                    manager.throw1.transform.position = manager.transform.position;
                    attacking = true;
                    manager.throw1.thrown = true;
                    manager.throw1.GetComponent<SpriteRenderer>().enabled = true;
                    manager.anim.Play("Throw");
                }
                else if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) != 1 && Mathf.Abs(Input.GetAxis(manager.myAxisX)) < 0.5f && Mathf.Abs(Input.GetAxis(manager.myAxisY)) < 0.5f && attacking == false)
                {
                    //Was Fire1 released and the Input Axis is < 0.1f?
                    manager.transform.position = manager.throw1.transform.position;
                    attacking = true;

                    Debug.Log("Nothing Here yet");

                    manager.activeState = new Free(manager);
                }
            }

            void VelocityDecay(float modifier)
            {
                if (manager.grounded == true)
                {
                    //Decay velocity until Abs(manager.rb.velocity.x) <= 1 then set to Vector2.zero
                    if (manager.rb.velocity.x > 1)
                    {
                        manager.rb.velocity = new Vector2(manager.rb.velocity.x - Time.fixedDeltaTime * modifier, manager.rb.velocity.y);
                    }
                    else if (manager.rb.velocity.x < 1)
                    {
                        manager.rb.velocity = new Vector2(manager.rb.velocity.x + Time.fixedDeltaTime * modifier, manager.rb.velocity.y);
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
        }
    }
}