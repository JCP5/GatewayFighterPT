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
        float dashDuration = 0.166f;

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
            if (manager.grounded == true)
            {
                if (dashDuration > 0)
                {
                    dashDuration -= Time.fixedDeltaTime;
                    manager.rb.velocity = new Vector2(manager.moveSpeed * strength * direction * Time.fixedDeltaTime, 0);

                    if (manager.anim.GetInteger("AnimState") != 3)
                        manager.anim.SetInteger("AnimState", 3);

                    if (Input.GetAxis("Vertical") == 1)
                    {
                        manager.rb.velocity = new Vector2(manager.moveSpeed * strength * direction * Time.fixedDeltaTime, Input.GetAxis("Vertical") * manager.jumpStrength * Time.fixedDeltaTime);
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
                    manager.anim.SetInteger("AnimState", 9);
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