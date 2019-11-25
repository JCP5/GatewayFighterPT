using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Free : IShotoBase
    {
        CharacterState manager;

        public Free(CharacterState managerRef)
        {
            manager = managerRef;
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            manager.AttackCheck();
            manager.DashCheck();
            FreeMovement();
        }

        void FreeMovement()
        {
            if (Input.GetAxis("Vertical") == 1)
            {
                manager.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * manager.moveSpeed * Time.fixedDeltaTime, Input.GetAxis("Vertical") * manager.jumpStrength * Time.fixedDeltaTime);
                manager.activeState = new Jump(manager, manager.rb.velocity);
            }
            else
            {
                manager.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * manager.moveSpeed * Time.fixedDeltaTime, 0);

                if (Mathf.Abs(Input.GetAxis("Horizontal")) == 1)
                {
                    if (manager.anim.GetInteger("AnimState") != 1)
                        manager.anim.SetInteger("AnimState", 1);
                    manager.FlipX();
                }
                else if (Mathf.Abs(Input.GetAxis("Horizontal")) == 0)
                {
                    if (manager.anim.GetInteger("AnimState") != 0)
                        manager.anim.SetInteger("AnimState", 0);
                }
            }
        }
    }
}