using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.CharacterControl;

namespace Assets.Code.Shoto
{
    public class Jump : ICharacterBase
    {
        ShotokunManager manager;
        Vector2 jumpVector;
        float waitForStartup = 3f / 60f;
        float xVelocity;
        float originY = 0f;
        float peak = 0f;

        public Jump(ShotokunManager managerRef, Vector2 v)
        {
            manager = managerRef;
            manager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            originY = manager.transform.position.y;
            jumpVector = new Vector2(v.x * manager.moveSpeed, v.y * manager.jumpStrength);
            //Debug.Log(v);

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
            manager.FlipX();
            manager.LayerByVelocity();

            if (manager.airAttack == false)
                manager.AttackCheck();

            if (waitForStartup > 0 && manager.grounded == true)
            {
                waitForStartup -= Time.fixedDeltaTime;
            }
            else if (waitForStartup <= 0 && manager.grounded == true)
            {
                manager.rb.AddForce(jumpVector, ForceMode2D.Force);
                //manager.gameObject.layer = 10;
                manager.anim.Play("2_Jump", -1, 0);
                manager.grounded = false;
            }
            else if(manager.grounded == false && manager.rb.velocity.y < 0f && manager.passThrough == false)
                manager.DetectGround();
        }
    }
}