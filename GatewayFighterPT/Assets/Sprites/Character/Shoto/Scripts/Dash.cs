using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Dash : IShotoBase
    {
        CharacterState manager;
        float direction;
        float strength;
        float dashDuration = 10f / 60f;

        public Dash(CharacterState managerRef, float v)
        {
            strength = managerRef.dashStrength;
            manager = managerRef;
            direction = v;
            manager.doubleBuffer = manager.adjustDoubleBuffer;

            //managerRef.rb.velocity = Vector2.zero;
        }

        public void StateStart()
        {

        }

        public void StateUpdate()
        {
            manager.DashCheck();
            manager.AttackCheck();

            if (manager.grounded == true)
            {
                if (dashDuration > 0)
                {
                    dashDuration -= Time.fixedDeltaTime;
                    manager.rb.velocity = new Vector2(manager.moveSpeed * strength * direction * Time.fixedDeltaTime, 0);

                    manager.anim.Play("3_Dash");

                    if (Input.GetAxis(manager.myAxisY) > 0.5f)
                    {
                        manager.rb.velocity = new Vector2(manager.moveSpeed * strength * direction * Time.fixedDeltaTime, Mathf.Round(Input.GetAxis(manager.myAxisY)) * manager.jumpStrength * Time.fixedDeltaTime);
                        manager.activeState = new Jump(manager, manager.rb.velocity);
                    }
                }
                else
                    manager.activeState = new Free(manager);
            }
            else if (manager.grounded == false)
            {
                if (dashDuration >= 0)
                {
                    dashDuration -= Time.fixedDeltaTime;
                    manager.rb.gravityScale = 0;
                    manager.rb.velocity = new Vector2(manager.moveSpeed * strength / 1.5f * direction * Time.fixedDeltaTime, 0);

                    manager.anim.Play("9_AirDash");
                }
                else if (manager.grounded == true)
                {
                    manager.rb.gravityScale = 5;
                    manager.activeState = new Free(manager);
                }
                else if (manager.grounded == false)
                {
                    manager.rb.gravityScale = 5;
                    manager.activeState = new Jump(manager, Vector2.zero);
                }
            }
        }
    }
}