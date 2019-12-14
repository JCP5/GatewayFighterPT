using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Parried : IShotoBase
    {
        CharacterState manager;
        Transform opponent;

        public Parried(CharacterState managerRef, Transform opponentRef)
        {
            manager = managerRef;
            opponent = opponentRef;

            Vector2 opponentVector = opponent.position - manager.transform.position;
            Vector2 forceVector = new Vector2(opponentVector.x / Mathf.Abs(opponentVector.x), 0) - new Vector2(0, opponentVector.y);

            manager.rb.velocity = Vector2.zero;

            manager.FaceOpponent(manager.transform.position, opponent.transform.position);
            manager.anim.Play("10_Parried");

            //Addforce backwards depending on rotation
            //0 is facing right, 180 is facing left
            /*if (manager.transform.rotation == Quaternion.Euler(Vector3.zero))
                manager.rb.AddForce(-Vector2.right * 2f, ForceMode2D.Impulse);
            else
                manager.rb.AddForce(Vector2.right * 2f, ForceMode2D.Impulse);*/

            manager.rb.AddForce(-forceVector * 2f, ForceMode2D.Impulse);
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