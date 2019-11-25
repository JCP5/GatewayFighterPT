using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class CharacterState : MonoBehaviour
    {
        public IShotoBase activeState;

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
            doubleBuffer = adjustDoubleBuffer;
            anim = GetComponent<Animator>();

            if (GetComponent<Rigidbody2D>() != null)
                rb = GetComponent<Rigidbody2D>();
            else
                Debug.LogError("RigidBody is missing");

            activeState = new Free(this);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            activeState.StateUpdate();
            Debug.Log(activeState);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (activeState is Jump && collision.gameObject.tag == "Ground")
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

            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1)
            {
                if (hzSwitch == false)
                {
                    startBuffer = true;
                    hzSwitch = true;
                    xAxisCounter++;

                    if (first == 0 || first != Input.GetAxisRaw("Horizontal"))
                    {
                        first = Input.GetAxisRaw("Horizontal");
                        doubleBuffer = adjustDoubleBuffer;
                    }
                    else if (Mathf.Abs(first) == 1)
                    {
                        second = Input.GetAxisRaw("Horizontal");
                    }
                }
            }
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0)
            {
                hzSwitch = false;
            }
        }

        public void FlipX()
        {
            if (Input.GetAxis("Horizontal") > 0)
                GetComponent<SpriteRenderer>().flipX = false;
            else if (Input.GetAxis("Horizontal") < 0)
                GetComponent<SpriteRenderer>().flipX = true;
        }

        public void AttackCheck()
        {
            if (Input.GetAxisRaw("Fire1") == 1)
            {
                activeState = new Attack(this);
            }
        }

        public void AnimationFinish()
        {
            anim.SetInteger("AnimState", 0);
            activeState = new Free(this);
        }

        public void ForceAttack()
        {
            Debug.Log("Hello");
            if(Mathf.Abs(Input.GetAxis("Horizontal")) != 1)
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
    }
}
