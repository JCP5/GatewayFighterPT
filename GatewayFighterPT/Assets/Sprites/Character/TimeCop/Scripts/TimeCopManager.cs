using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.MiscManagers;
using Assets.Code.FightScene;
using Assets.Code.Effects;
using Assets.Code.CharacterControl;

namespace Assets.Code.TimeCop
{
    public class TimeCopManager : CharacterState
    {
        public ITimeCopBase activeState;
        public Throw throw1;

        // Start is called before the first frame update
        void Start()
        {
            PlayerNumber(1); //*****

            //vfx = GetComponent<VFXHolder>().vfxPrefabs; *****
            //id = FindObjectOfType<InputDetector>(); *****
            anim = this.GetComponent<Animator>();
            t = GetComponentInChildren<Text>();

            Debug.Log(playerNumber);
            //if (id.joysticks[playerNumber - 1] != null) *****
            //{
            myAxisX = gameObject.tag + "_Horizontal" + "_360";//*/id.joysticks[playerNumber - 1];
            myAxisY = gameObject.tag + "_Vertical" + "_360";//*/id.joysticks[playerNumber - 1];
            myAxisAttack = gameObject.tag + "_Fire1" + "_360";//*/id.joysticks[playerNumber - 1];
                                                              //}
                                                              //else
                                                              //Debug.LogError("No controller detected");

            if (GetComponent<Rigidbody2D>() != null)
                rb = GetComponent<Rigidbody2D>();
            else
                Debug.LogError("RigidBody is missing");

            activeState = new Free(this);
        }

        private void FixedUpdate()
        {
            activeState.StateUpdate();
            Debug.Log(activeState);
        }

        public void AnimationFinish()
        {
            Debug.Log("AnimationFinish Called");
            if (grounded == true)
                activeState = new Free(this);
            else if (grounded == false) 
                //activeState = new Jump(this, Vector2.zero);

            invulToStrike = false;
        }

        public void AttackCheck()
        {
            if (Input.GetAxisRaw(myAxisAttack) == 1)
            {
                activeState = new Attack(this, new Vector2(Mathf.Round(Input.GetAxisRaw(myAxisX)), Mathf.Round(Input.GetAxisRaw(myAxisY))));
            }
        }
    }
}