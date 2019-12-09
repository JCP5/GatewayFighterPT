﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Free : IShotoBase
    {
        CharacterState manager;
        bool pressed = true;
        Vector3 myTrans;

        public Free(CharacterState managerRef)
        {
            manager = managerRef;
            manager.invulToStrike = false; //On animation cancel
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            myTrans = manager.transform.position;
            manager.AttackCheck();
            manager.DashCheck();
            FreeMovement();
        }

        void FreeMovement()
        {
            if (Input.GetAxis(manager.myAxisY) > 0.5f) //Check for jump
            {
                //add the x and y axis to create the jump vector
                manager.rb.velocity = new Vector2(Mathf.Round(Input.GetAxis(manager.myAxisX)) * manager.moveSpeed * Time.fixedDeltaTime, Mathf.Round(Input.GetAxis(manager.myAxisY)) * manager.jumpStrength * Time.fixedDeltaTime);
                manager.activeState = new Jump(manager, manager.rb.velocity);
            }
            else //No input on the Y axis
            {
                //Set Velocity to be the input on the X axis and set animations on whether or not it's being used
                manager.rb.velocity = new Vector2(Mathf.Round(Input.GetAxis(manager.myAxisX)) * manager.moveSpeed * Time.fixedDeltaTime, 0);

                if (Mathf.Abs(Input.GetAxis(manager.myAxisX)) != 0)
                {
                    if (pressed == false)
                    {
                        if (manager.transform.rotation == Quaternion.Euler(Vector3.zero))
                            manager.vfx["Walk"].GetComponent<ParticleSystemRenderer>().flip = new Vector3(0, 0, 0);
                        else
                            manager.vfx["Walk"].GetComponent<ParticleSystemRenderer>().flip = new Vector3(1, 0, 0);

                        Object.Instantiate(manager.vfx["Walk"], new Vector3(myTrans.x, myTrans.y - 0.8f, myTrans.z), Quaternion.identity);
                    }

                    pressed = true;

                    manager.anim.Play("1_Run");
                    manager.FlipX();
                }
                else if (Mathf.Abs(Input.GetAxis(manager.myAxisX)) == 0)
                {
                    pressed = false;
                    manager.anim.Play("0_Idle");
                }
            }
        }
    }
}