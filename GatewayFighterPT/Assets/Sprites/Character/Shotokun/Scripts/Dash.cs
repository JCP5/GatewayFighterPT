using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.CharacterControl;

namespace Assets.Code.Shoto
{
    public class Dash : ICharacterBase
    {
        ShotokunManager manager;
        float direction;
        float strength;
        float dashDuration = 10f / 60f;

        public Dash(ShotokunManager managerRef, float v)
        {
            strength = managerRef.dashStrength;
            manager = managerRef;
            direction = v;
            //manager.doubleBuffer = manager.adjustDoubleBuffer;

            manager.rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            if (manager.grounded == false)
            {
                if (manager.transform.rotation == Quaternion.Euler(Vector3.zero))
                    manager.vfx["AirDash"].GetComponent<ParticleSystemRenderer>().flip = new Vector3(1, 0, 0);
                else
                    manager.vfx["AirDash"].GetComponent<ParticleSystemRenderer>().flip = new Vector3(0, 0, 0);

                Object.Instantiate(manager.vfx["AirDash"], manager.transform.position, manager.transform.rotation);
                manager.airAction = true;
            }

            //managerRef.rb.velocity = Vector2.zero;
        }

        public void StateStart()
        {

        }

        public void StateUpdate()
        {
            //manager.DashCheck();
            manager.AttackCheck();

            if (manager.grounded == true)
            {
                if (dashDuration > 0)
                {
                    dashDuration -= Time.fixedDeltaTime;
                    manager.rb.velocity = new Vector2(manager.moveSpeed * strength * direction * Time.fixedDeltaTime, CheckSlopeY(manager.CalculateGroundAngle().y / manager.moveSpeed) * strength * Time.fixedDeltaTime);

                    manager.anim.Play("3_Dash");

                    if (Input.GetAxis(manager.myAxisY) > 0.5f)
                    {
                        Object.Instantiate(manager.vfx["Jump"], new Vector3(manager.transform.position.x, manager.transform.position.y - 1.5f, manager.transform.position.z), Quaternion.identity);
                        //manager.rb.velocity = new Vector2(manager.moveSpeed * strength * direction * Time.fixedDeltaTime, Mathf.Round(Input.GetAxis(manager.myAxisY)) * manager.jumpStrength * Time.fixedDeltaTime);
                        manager.activeState = new Jump(manager, new Vector2(Mathf.Round(Mathf.Round(Input.GetAxis(manager.myAxisX)) * manager.dashStrength), Mathf.Round(Input.GetAxis(manager.myAxisY))));
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
                    manager.rb.velocity = new Vector2(manager.moveSpeed * strength / manager.airDashModifier * direction * Time.fixedDeltaTime, 0);

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

        float CheckSlopeY(float f)
        {
            if (f > 0)
                return 0;
            else
                return f;
        }
    }
}