using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Parried : IShotoBase
    {
        CharacterState manager;

        public Parried(CharacterState managerRef)
        {
            manager = managerRef;

            manager.rb.velocity = Vector2.zero;

            manager.anim.Play("10_Parried");

            //Addforce backwards depending on rotation
            //0 is facing right, 180 is facing left
            if (manager.transform.rotation == Quaternion.Euler(Vector3.zero))
                manager.rb.AddForce(-Vector2.right * 2f, ForceMode2D.Impulse);
            else
                manager.rb.AddForce(Vector2.right * 2f, ForceMode2D.Impulse);
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            
        }
    }
}