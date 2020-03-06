using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.CharacterControl;

namespace Assets.Code.Shoto
{
    public class ShotokunManager : CharacterState
    {
        public ICharacterBase activeState;
        public float diStrength = 1f;

        //dash stuff
        /*public float adjustDoubleBuffer = 0.3f;
        public float doubleBuffer;
        public bool hzSwitch = false;
        public bool startBuffer = false;
        public int xAxisCounter = 0;*/
        public float airDashModifier = 1f;
        public bool dashCancel = true;

        public bool helmbreaker = false;
        public bool risingSlash = false;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            inputManager.DashEvent += DashCheck;
            //doubleBuffer = adjustDoubleBuffer;

            activeState = new Free(this);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            base.FixedUpdate();

            activeState.StateUpdate();
            //Debug.Log(activeState);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Vector3 contactDirection = Vector3.zero;
            if (rb != null)
                contactDirection = collision.contacts[0].point - rb.position;

            if (rb != null)
                rb.gravityScale = 5f;
            else
            {
                rb = GetComponent<Rigidbody2D>();
                rb.gravityScale = 5f;
            }

            if (collision.gameObject.tag == "Ground" && contactDirection.y < -0.8f)
            {
                LandingHandler();
            }
            else if (collision.gameObject.tag == "Platform" && contactDirection.y < -0.8f)
            {
                LandingHandler();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Platform")
                grounded = false;
            else
                DetectGround();
        }

        void LandingHandler()
        {
            DetectGround();
            airAttack = false;
            airAction = false;

            if (risingSlash == true || helmbreaker == true)
            {
                if (!(activeState is PostRound || activeState is Parried || activeState is Clash || activeState is PreRound))
                    activeState = new Recovery(this);
            }
            else if(risingSlash == false && helmbreaker == false)
            {
                if ((activeState is Jump || activeState is Dash))
                    activeState = new Free(this);
            }
        }

        //use animation event to mark cancel time
        public void DashCancelable()
        {
            dashCancel = true;
        }

        public void DashCancelableNot()
        {
            dashCancel = false;
        }

        public void DashCheck(float v)
        {
            if (!(activeState is Free || activeState is Jump || activeState is Attack))
            {
                return;
            }
            else if (airAction == false && dashCancel == true)
                activeState = new Dash(this, v);
        }

        public void ResetActions()
        {
            grounded = true;
            airAction = false;
            airAttack = false;
            risingSlash = false;
            helmbreaker = false;
            gameObject.layer = 8;
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
            rb.gravityScale = gravityScale;
            if (grounded == true)
                activeState = new Free(this);
            else if (grounded == false)
                activeState = new Jump(this, Vector2.zero);

            invulToStrike = false;
            dashCancel = true;
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