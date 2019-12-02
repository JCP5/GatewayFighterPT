using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Shoto
{
    public class Clash : IShotoBase
    {
        CharacterState manager;
        float frameDifference = 0;

        public Clash(CharacterState managerRef, float frameData, Text t)
        {
            manager = managerRef;
            frameDifference = frameData;

            //Animate the text for frame data
            t.text = (Mathf.Round(frameDifference * 60)).ToString();
            t.GetComponent<Animator>().Play("FadeOut", -1, 0);
            t.GetComponentInParent<Canvas>().GetComponent<RectTransform>().localRotation = manager.transform.rotation;
            Debug.Log(manager.name + " " + frameDifference * 60);

            manager.rb.velocity = Vector2.zero;
            managerRef.anim.Play("8_Guard");

            if (manager.transform.rotation == Quaternion.Euler(Vector3.zero))
                manager.rb.AddForce(-Vector2.right * 5f, ForceMode2D.Impulse);
            else
                manager.rb.AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
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
    }
}