using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Box;

namespace Assets.Code.Shoto
{
    public class Attack : IShotoBase
    {
        ShotokunManager manager;
        bool attacking;
        public Vector2 aimVector;
        public float waitForStartUp = 4f / 60f;
        public float slashTracking = 12f / 60f;
        float diModifier = 1;

        public Attack(ShotokunManager managerRef, Vector2 v2)
        {
            manager = managerRef;
            attacking = false;
            aimVector = v2;
            manager.frameCounter = 0;

            manager.FlipX();
        }

        // Start is called before the first frame update
        public void StateStart()
        {

        }

        // Update is called once per frame
        public void StateUpdate()
        {
            waitForStartUp -= Time.fixedDeltaTime;
            Debug.Log(attacking);

            Track();

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
                {
                    manager.rb.velocity = new Vector2(aimVector.x * 350f/*old moveSpeed*/ * 2f/*old dashStrength*/ * Time.fixedDeltaTime, aimVector.y * 350f * 2f * Time.fixedDeltaTime)
                        + DiCalculation(diModifier);
                }
            }
            else if (slashTracking < 0)//Stop tracking once the move is active + remove dashStrength to weaken or stop velocity
            {
                if (manager.grounded == true) //Animation finish event resets state to idle
                    VelocityDecay(75f);
                    //manager.rb.velocity = new Vector2(aimVector.x * 350f / 10f * Time.fixedDeltaTime, aimVector.y * 350f / 10f * Time.fixedDeltaTime);
                else
                {
                    //Reset aerial gravity and velocity
                    manager.rb.gravityScale = 5;
                    manager.rb.AddForce(Vector2.zero);//new Vector2(aimVector.x, aimVector.y));//Can't use jump state because that would cancel the animation
                    manager.airAttack = true;
                }
            }
        }

        Vector2 DiCalculation(float modifier)
        {
            return new Vector2(diPreventDoubleOnDiagonalX(aimVector.x) * manager.diStrength * modifier, 
                diPreventDoubleOnDiagonalY(aimVector.y) * manager.diStrength * modifier);
        }

        float diPreventDoubleOnDiagonalX(float vector)
        {
            if (Mathf.Ceil(Input.GetAxis(manager.myAxisX)) == vector)
                return 0;
            else
                return Mathf.Ceil(Input.GetAxis(manager.myAxisX));
        }

        float diPreventDoubleOnDiagonalY(float vector)
        {
            if (Mathf.Ceil(Input.GetAxis(manager.myAxisY)) == vector)
                return 0;
            else
                return Mathf.Ceil(Input.GetAxis(manager.myAxisY));
        }

        void WhichAttack()
        {
            if (waitForStartUp < 0)
            {
                if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) == 1 && attacking == false)//Hold check
                {
                    if (Mathf.Abs(manager.rb.velocity.x) <= 20f)
                        AttackDash();

                    manager.anim.Play("4_Hold");
                    aimVector = new Vector2(Mathf.Round(Input.GetAxisRaw(manager.myAxisX)), Mathf.Round(Input.GetAxisRaw(manager.myAxisY)));

                    VelocityDecay(50f);
                }
                else if ((Input.GetAxis(manager.myAxisAttack) != 1 && Mathf.Abs(Input.GetAxis(manager.myAxisX)) > 0.5f || (Mathf.Abs(Input.GetAxis(manager.myAxisX)) > 0.5f && Mathf.Abs(Input.GetAxis(manager.myAxisY)) > 0.5f)) && attacking == false)//slash check
                {
                    //was Fire1 released and the Input Axis X is > 0.5f?
                    diModifier = 1;
                    attacking = true;
                    manager.anim.Play("5_Slash");
                }
                else if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) != 1 && Mathf.Abs(Input.GetAxis(manager.myAxisX)) < 0.1f && Mathf.Abs(Input.GetAxis(manager.myAxisY)) < 0.1f && attacking == false)//parry check
                {
                    //Was Fire1 released and both Input Axes are < 0.1f?
                    aimVector = Vector2.zero;
                    attacking = true;
                    manager.anim.Play("6_Parry");

                    VelocityDecay(50f);
                }
                else if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) != 1 && Mathf.Abs(Input.GetAxis(manager.myAxisX)) < 0.1f && Input.GetAxis(manager.myAxisY) > 0.5f && attacking == false)//rising slash check
                {
                    //Was Fire1 released and X is < 0.1f and Y > 0.5f?
                    diModifier = 0.5f;

                    if(CheckDirection(manager.transform) == "right")
                        aimVector = new Vector2(0.1f, 1);
                    else
                        aimVector = new Vector2(-0.1f, 1);

                    attacking = true;
                    manager.anim.Play("RisingSlash");
                }
                else if (Mathf.Abs(Input.GetAxis(manager.myAxisAttack)) != 1 && Mathf.Abs(Input.GetAxis(manager.myAxisX)) < 0.1f && Input.GetAxis(manager.myAxisY) < 0.5f && attacking == false && manager.grounded == false)//Helm Breaker check
                {
                    diModifier = 0.5f;
                    manager.helmbreaker = true;

                    aimVector = -Vector2.up; 
                    attacking = true;
                    manager.anim.Play("HelmBreaker");
                }
            }
        }

        string CheckDirection(Transform t)
        {
            if (t.rotation == Quaternion.Euler(Vector3.zero))
                return "right";
            else
                return "left";
        }

        void VelocityDecay(float modifier)
        {
            if (manager.grounded == true)
            {
                if (Mathf.Abs(manager.rb.velocity.x) <= 1)
                {
                    manager.rb.velocity = Vector2.zero;
                }
                //Decay velocity until Abs(manager.rb.velocity.x) <= 1 then set to Vector2.zero
                else if (manager.rb.velocity.x > 1)
                {
                    manager.rb.velocity = new Vector2(manager.rb.velocity.x - Time.fixedDeltaTime * modifier, manager.rb.velocity.y);
                }
                else if (manager.rb.velocity.x < 1)
                {
                    manager.rb.velocity = new Vector2(manager.rb.velocity.x + Time.fixedDeltaTime * modifier, manager.rb.velocity.y);
                }
            }
            else if (manager.grounded == false)
            {
                //Reset aerial gravity
                manager.rb.gravityScale = 5;
            }
        }

        void AttackDash()
        {
            if (manager.airAction == false)
            {
                if (manager.xAxisCounter >= 2 && manager.first != 0 && manager.first == manager.second)
                {
                    manager.xAxisCounter = 0;
                    manager.rb.AddForce(new Vector2(20f * manager.first, 0), ForceMode2D.Impulse);
                    manager.anim.Play("3_Dash");
                }

                if (manager.startBuffer == true)
                {
                    if (manager.doubleBuffer > 0)
                        manager.doubleBuffer -= 1 * Time.fixedDeltaTime;
                    else
                    {
                        manager.startBuffer = false;
                        manager.doubleBuffer = manager.adjustDoubleBuffer;
                        manager.xAxisCounter = 0;
                        manager.first = 0;
                        manager.second = 0;
                    }
                }

                if (Mathf.Abs(Input.GetAxisRaw(manager.myAxisX)) > 0.5f)
                {
                    if (manager.hzSwitch == false)
                    {
                        manager.startBuffer = true;
                        manager.hzSwitch = true;
                        manager.xAxisCounter++;

                        if (manager.first == 0 || manager.first != Mathf.Round(Input.GetAxisRaw(manager.myAxisX)))
                        {
                            manager.first = Mathf.Round(Input.GetAxisRaw(manager.myAxisX));
                            manager.doubleBuffer = manager.adjustDoubleBuffer;
                        }
                        else if (Mathf.Abs(manager.first) == 1)
                        {
                            manager.second = Mathf.Round(Input.GetAxisRaw(manager.myAxisX));
                        }
                    }
                }
                if (Mathf.Abs(Input.GetAxisRaw(manager.myAxisX)) < 0.1f)
                {
                    manager.hzSwitch = false;
                }
            }
        }
    }
}