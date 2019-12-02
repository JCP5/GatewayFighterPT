using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Jump : IShotoBase
    {
        CharacterState manager;
        Vector2 jumpVector;

        public Jump(CharacterState managerRef, Vector2 v)
        {
            jumpVector = v;
            manager = managerRef;

            manager.grounded = false;
            manager.rb.AddForce(Vector2.up * manager.jumpStrength * Time.fixedDeltaTime, ForceMode2D.Force);
            manager.anim.Play("2_Jump", -1, 0);
        }

        // Start is called before the first frame update
        public void StateStart()
        {

        }

        // Update is called once per frame
        public void StateUpdate()
        {
            manager.DashCheck();
            manager.FlipX();

            if (manager.airAttack == false)
                manager.AttackCheck();
        }
    }
}