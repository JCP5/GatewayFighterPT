using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.MiscManagers;
using Assets.Code.FightScene;
using Assets.Code.Effects;

namespace Assets.Code.CharacterControl
{
    public class CharacterState : MonoBehaviour
    {
        public bool testing;

        public InputDetector id;
        public Text t;
        public Dictionary<string, GameObject> vfx;
        public CharacterInputManager inputManager;

        public string myAxisY;
        public string myAxisX;
        public string myAxisAttack;
        public float frameCounter = 0; //Frame Counter starts counting when an attack starts (see Attack state)

        public int playerNumber;
        public float moveSpeed = 10;
        public float jumpStrength = 10;
        public float dashStrength = 100;
        public float first;
        public float second;
        public Rigidbody2D rb;
        public float gravityScale = 5;

        public bool airAction = false;
        public bool airAttack = false;
        public bool invulToStrike = false;

        public bool grounded = true;
        public bool passThrough = false;


        public Animator anim;

        private void Awake()
        {
            if (GetComponent<Rigidbody2D>() != null)
                rb = GetComponent<Rigidbody2D>();
            else
                Debug.LogError("RigidBody is missing");
        }

        // Start is called before the first frame update
        public void Start()
        {
            vfx = GetComponent<VFXHolder>().vfxPrefabs;
            id = FindObjectOfType<InputDetector>();//unComment
            anim = this.GetComponent<Animator>();
            t = GetComponentInChildren<Text>();
            inputManager = this.GetComponent<CharacterInputManager>();
            inputManager.DownHeldEvent += PassThroughPlatform;
            inputManager.NeutralEvent += NeutralHandler;

            Debug.Log(playerNumber);

            if (testing == true)
                UsePS3ControllerForTesting();
            else
            {
                if (id.joysticks[playerNumber - 1] != null)//unComment
                {
                    myAxisX = gameObject.tag + "_Horizontal" + id.joysticks[playerNumber - 1];
                    myAxisY = gameObject.tag + "_Vertical" + id.joysticks[playerNumber - 1];
                    myAxisAttack = gameObject.tag + "_Fire1" + id.joysticks[playerNumber - 1];
                    this.GetComponent<CharacterInputManager>().enabled = true;
                }
                else
                    Debug.LogError("No controller detected");
            }
            //activeState = new Free(this);
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            if (t.color.a > 0)
                t.color -= new Color(0, 0, 0, Time.deltaTime * 1f);
            //activeState.StateUpdate();
        }

        //Testing Only
        void UsePS3ControllerForTesting()
        {
            Debug.Log("Currently in testing mode. Deactivate later");
            myAxisX = gameObject.tag + "_Horizontal" + "_360";/*id.joysticks[playerNumber - 1];*/
            myAxisY = gameObject.tag + "_Vertical" + "_360";/*id.joysticks[playerNumber - 1];*/
            myAxisAttack = gameObject.tag + "_Fire1" + "_360";/*id.joysticks[playerNumber - 1];*/
            this.GetComponent<CharacterInputManager>().enabled = true;
        }

        public virtual void HitTaken()
        {

        }

        public virtual void HitLanded()
        {

        }

        public virtual void Clash(CharacterState opponent, float frames, Vector3 contactpoint)
        {

        }

        public virtual void Parried(CharacterState self, CharacterState opponent)
        {

        }

        public virtual void Free()
        {

        }

        public virtual void PreRound()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            rb.gravityScale = gravityScale;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            DetectGround();
        }

        public void LayerByVelocity()
        {
            if (passThrough == false)
            {
                if (rb.velocity.y > 0.2f)
                {
                    int mask = LayerMask.NameToLayer("PlatformPass");
                    gameObject.layer = mask;
                }
                else if (rb.velocity.y <= 0.2f)
                {
                    int mask = LayerMask.NameToLayer("Character");
                    gameObject.layer = mask;
                }
            }
            else
            {
                int mask = LayerMask.NameToLayer("PlatformPass");
                gameObject.layer = mask;
            }
        }

        public void ResetGravityScale()
        {
            if (rb != null)
                rb.gravityScale = gravityScale;
        }

        public Vector2 InputAxes()
        {
            return new Vector2(Input.GetAxis(myAxisX), Input.GetAxis(myAxisY));
        }

        public void FlipX()
        {
            if (Input.GetAxis(myAxisX) > 0)
                transform.rotation = Quaternion.Euler(Vector3.zero);
            else if (Input.GetAxis(myAxisX) < 0)
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        public void DetectGround()
        {
            int layerMask = ~(1 << 8);
            RaycastHit2D hit;
            hit = Physics2D.Raycast(this.transform.position, -Vector2.up, 1f, layerMask);

            if (hit.collider == null)
            {
                grounded = false;
            }
            else
            {
                grounded = true;
            }
        }

        public Vector3 CalculateGroundAngle()
        {
            int layerMask = ~(LayerMask.GetMask("Character"));
            RaycastHit2D hit;
            hit = Physics2D.Raycast(this.transform.position, -Vector2.up, 2f, layerMask);

            //Debug.Log(new Vector3(Mathf.Abs(hit.normal.x) * transform.right.x, hit.normal.y * -hit.normal.x * transform.right.x, 0));

            //return (new Vector3(Mathf.Abs(hit.normal.x) * transform.right.x, hit.normal.y * -hit.normal.x * transform.right.x, 0));
            return hit.normal;
        }

        public float CalculateSlope(Vector2 vector)
        {
            if (vector.x != 0)
                return vector.y / -vector.x;
            else
                return 0;
        }

        public void PassThroughPlatform()
        {
            passThrough = true;
        }

        public void NeutralHandler()
        {
            passThrough = false;
        }

        public void ResetValues()
        {
            gameObject.layer = LayerMask.NameToLayer("Character");
            grounded = true;
            rb.velocity = Vector2.zero;
        }

        /*public void AttackCheck() //***** Convert to virtual override
        {
            if (Input.GetAxisRaw(myAxisAttack) == 1)
            {
                activeState = new Attack(this, new Vector2 (Mathf.Round(Input.GetAxisRaw(myAxisX)), Mathf.Round(Input.GetAxisRaw(myAxisY))));
            }
        }*/

        /*public void AnimationFinish()
        {
            if (grounded == true)
                activeState = new Free(this);
            else if (grounded == false)
                activeState = new Jump(this, Vector2.zero);

            invulToStrike = false;
        }*/

        public void ParryActive()
        {
            //invulToStrike = !invulToStrike;
        }

        //unused so far
        /*public void ForceAttack()
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
        }*/

        //playerNumber will be set by FightManager on Fight Start***
        //use playerNumber to get index of corresponding controller type in InputDetector
        //so if the player number is 1 then get the joystick at InputDetector.joysticks[1];
        public int PlayerNumber(int i)
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
        }
    }
}

