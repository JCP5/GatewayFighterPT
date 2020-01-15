using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Jump : IShotoBase
    {
        ShotokunManager manager;
        Vector2 jumpVector;
        float waitForStartup = 3f / 60f;
        float xVelocity;

        public Jump(ShotokunManager managerRef, Vector2 v)
        {
            manager = managerRef;
            jumpVector = new Vector2(v.x * manager.moveSpeed, v.y * manager.jumpStrength);

            if (manager.grounded == true)
            {
                manager.rb.velocity = Vector2.zero;
                //manager.rb.AddForce(jumpVector, ForceMode2D.Force);
                //manager.anim.Play("2_Jump", -1, 0);
            }
            else
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

            if (waitForStartup > 0 && manager.grounded == true)
            {
                waitForStartup -= Time.fixedDeltaTime;
            }
            else if (waitForStartup <= 0 && manager.grounded == true)
            {
                //The force is applied twice for some reason
                manager.rb.AddForce(jumpVector / 2, ForceMode2D.Force);
                manager.anim.Play("2_Jump", -1, 0);
            }
        }
    }
}