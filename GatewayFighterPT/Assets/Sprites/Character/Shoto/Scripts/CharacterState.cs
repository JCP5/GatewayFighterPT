using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Assets.Code.MiscManagers;

namespace Assets.Code.Shoto
{
    public class CharacterState : MonoBehaviour
    {
        public IShotoBase activeState;
        public InputDetector id;
        public string myAxisY;
        public string myAxisX;
        public string myAxisAttack;

        public int playerNumber;
        public float moveSpeed = 10;
        public float jumpStrength = 10;
        public float dashStrength = 100;
        public Rigidbody2D rb;

        public float adjustDoubleBuffer = 0.3f;
        public float doubleBuffer;
        public bool hzSwitch = false;
        public bool startBuffer = false;
        public int xAxisCounter = 0;
        public float first;
        public float second;

        public bool grounded = true;

        public Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            id = FindObjectOfType<InputDetector>();
            doubleBuffer = adjustDoubleBuffer;
            anim = GetComponent<Animator>();
            playerNumber = PlayerNumber(gameObject.tag);

            if (GetComponent<Rigidbody2D>() != null)
                rb = GetComponent<Rigidbody2D>();
            else
                Debug.LogError("RigidBody is missing");

            activeState = new Free(this);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log(new Vector2(Input.GetAxis(myAxisX), Input.GetAxis(myAxisY)));
            activeState.StateUpdate();
            //Debug.Log(activeState);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((activeState is Jump || activeState is Attack || activeState is Dash) && collision.gameObject.tag == "Ground" && grounded == false)
            {
                grounded = true;
                activeState = new Free(this);
            }
        }

        public void DashCheck()
        {
            if (xAxisCounter >= 2 && first != 0 && first == second)
            {
                xAxisCounter = 0;
                activeState = new Dash(this, first);
            }

            if (startBuffer == true)
            {
                if (doubleBuffer > 0)
                    doubleBuffer -= 1 * Time.fixedDeltaTime;
                else
                {
                    startBuffer = false;
                    doubleBuffer = adjustDoubleBuffer;
                    xAxisCounter = 0;
                    first = 0;
                    second = 0;
                }
            }

            if (Mathf.Abs(Input.GetAxisRaw(myAxisX)) > 0.5f)
            {
                if (hzSwitch == false)
                {
                    startBuffer = true;
                    hzSwitch = true;
                    xAxisCounter++;

                    if (first == 0 || first != Mathf.Round(Input.GetAxisRaw(myAxisX)))
                    {
                        first = Mathf.Round(Input.GetAxisRaw(myAxisX));
                        doubleBuffer = adjustDoubleBuffer;
                    }
                    else if (Mathf.Abs(first) == 1)
                    {
                        second = Mathf.Round(Input.GetAxisRaw(myAxisX));
                    }
                }
            }
            if (Mathf.Abs(Input.GetAxisRaw(myAxisX)) == 0)
            {
                hzSwitch = false;
            }
        }

        public void FlipX()
        {
            if (Input.GetAxis(myAxisX) > 0)
                GetComponent<SpriteRenderer>().flipX = false;
            else if (Input.GetAxis(myAxisX) < 0)
                GetComponent<SpriteRenderer>().flipX = true;
        }

        public void AttackCheck()
        {
            if (Input.GetAxisRaw(myAxisAttack) == 1)
            {
                activeState = new Attack(this, new Vector2 (Mathf.Round(Input.GetAxisRaw(myAxisX)), Mathf.Round(Input.GetAxisRaw(myAxisY))));
            }
        }

        public void AnimationFinish()
        {
            if (grounded == true)
                activeState = new Free(this);
            else if (grounded == false)
                activeState = new Jump(this, Vector2.zero);
        }

        //unused so far
        public void ForceAttack()
        {
            Debug.Log("Hello");
            if(Mathf.Abs(Input.GetAxis(myAxisX)) != 1)
            {
                anim.SetInteger("AnimState", 6);
                AnimationFinish();
            }
            else
            {
                anim.SetInteger("AnimState", 5);
                AnimationFinish();
            }
        }

        //use playerNumber to get index of corresponding controller type in InputDetector
        //so if the player number is 1 then get the joystick at InputDetector.joysticks[1];
        public int PlayerNumber(string s)
        {
            int number = 0;

            if (s == "P1")
                number = 1;
            else if (s == "P2")
                number = 2;
            else
                Debug.LogError("Incorrect Tag");

            myAxisX = gameObject.tag + "_Horizontal" + id.joysticks[number - 1];
            myAxisY = gameObject.tag + "_Vertical" + id.joysticks[number - 1];
            myAxisAttack = gameObject.tag + "_Fire1" + id.joysticks[number - 1];

            return number;
        }
    }
}
