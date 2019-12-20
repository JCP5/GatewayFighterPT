using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Shoto
{
    public class ShotokunManager : CharacterState
    {
        public IShotoBase activeState;

        //dash stuff
        public bool airAction = false;
        public float adjustDoubleBuffer = 0.3f;
        public float doubleBuffer;
        public bool hzSwitch = false;
        public bool startBuffer = false;
        public int xAxisCounter = 0;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            doubleBuffer = adjustDoubleBuffer;

            activeState = new Free(this);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (t.color.a > 0)
                t.color -= new Color(0, 0, 0, Time.deltaTime * 1f);

            activeState.StateUpdate();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (rb != null)
                rb.gravityScale = 5f;
            else
            {
                rb = GetComponent<Rigidbody2D>();
                rb.gravityScale = 5f;
            }
            if (collision.gameObject.tag == "Ground" && grounded == false)
            {
                grounded = true;
                airAttack = false;
                airAction = false;

                if ((activeState is Jump || activeState is Dash))
                    activeState = new Free(this);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                grounded = false;
                Instantiate(vfx["Jump"], new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), Quaternion.identity);
            }
        }

        public void DashCheck()
        {
            if (airAction == false)
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
        }

        public void AttackCheck()
        {
            if (Input.GetAxisRaw(myAxisAttack) == 1)
            {
                activeState = new Attack(this, new Vector2(Mathf.Round(Input.GetAxisRaw(myAxisX)), Mathf.Round(Input.GetAxisRaw(myAxisY))));
            }
        }

        public override void Hit()
        {
            activeState = new PostRound(this, false);
            Instantiate(vfx["Hit"], transform.position, Quaternion.identity);
            Debug.Log("Fuck");
        }

        public override void HitBox()
        {
            activeState = new PostRound(this, true);
        }

        public override void Clash(CharacterState opponent, float frames, Vector3 contactpoint)
        {
            activeState = new Clash(this, frames, this.t, opponent.transform);
        }

        public override void Parried(CharacterState self, CharacterState opponent)
        {
            activeState = new Parried(this, opponent.transform);
        }

        public override void Free()
        {
            activeState = new Free(this);
        }

        public override void PreRound()
        {
            activeState = new PreRound(this);
        }

        public void AnimationFinish()
        {
            if (grounded == true)
                activeState = new Free(this);
            else if (grounded == false)
                activeState = new Jump(this, Vector2.zero);

            invulToStrike = false;
        }

        /*public int PlayerNumber(int i)
        {
            int number = i;

            playerNumber = i;

            if (i == 1)
                this.tag = "P1";
            else if (i == 2)
                this.tag = "P2";

            return number;
        }

        public void FaceOpponent(Vector3 self, Vector3 opponent)
        {
            Vector3 direction = opponent - self;

            if (direction.x < 0)
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            else
                transform.rotation = Quaternion.Euler(Vector3.zero);
        }*/
    }
}