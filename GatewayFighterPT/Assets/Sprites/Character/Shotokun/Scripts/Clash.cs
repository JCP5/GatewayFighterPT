using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Shoto
{
    public class Clash : IShotoBase
    {
        ShotokunManager manager;
        float frameDifference = 0;
        Text text;

        public Clash(ShotokunManager managerRef, float frameData, Text t, Transform opponentRef)
        {
            manager = managerRef;
            frameDifference = frameData;
            text = t;

            Vector2 opponentDir = opponentRef.position - manager.transform.position;
            Vector2 forceVector = new Vector2(opponentDir.x / Mathf.Abs(opponentDir.x), 0) - new Vector2(0, opponentDir.y);
            
            //Reset velocity in order to prevent inheritance
            manager.rb.velocity = Vector2.zero;
            manager.rb.gravityScale = 5f;
            managerRef.anim.Play("8_Guard");

            /*if (manager.transform.rotation == Quaternion.Euler(Vector3.zero))
                manager.rb.AddForce(-Vector2.right * 5f, ForceMode2D.Impulse);
            else
                manager.rb.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);*/

            manager.FaceOpponent(manager.transform.position, opponentRef.transform.position);
            AnimateText(t);
            manager.rb.AddForce(-forceVector * 5f, ForceMode2D.Impulse);
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            if(frameDifference < 0)
            {
                //if you are minus then repeat the first frame of the guard animation that many times
                frameDifference += (1f / 60f);
                manager.anim.Play("8_Guard", -1, 0);
            }
        }

        void AnimateText(Text t)
        {
            if (frameDifference > 0)
                t.color = Color.cyan;
            else
                t.color = Color.red;

            if (frameDifference == 0)
                t.color = Color.white;

            //Animate the text for frame data
            t.text = (Mathf.Round(frameDifference * 60)).ToString();
            t.GetComponent<Animator>().Play("FadeOut", -1, 0);
            t.GetComponentInParent<Canvas>().GetComponent<RectTransform>().localRotation = manager.transform.rotation;
            Debug.Log(manager.name + " " + frameDifference * 60);
        }
    }
}