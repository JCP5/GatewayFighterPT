using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.MiscManagers;
using Assets.Code.FightScene;
using Assets.Code.Effects;


public class CharacterState : MonoBehaviour
{
    public InputDetector id;
    public Text t;
    public Dictionary<string, GameObject> vfx;

    public string myAxisY;
    public string myAxisX;
    public string myAxisAttack;
    public float frameCounter = 0;

    public int playerNumber;
    public float moveSpeed = 10;
    public float jumpStrength = 10;
    public float dashStrength = 100;
    public float first;
    public float second;
    public Rigidbody2D rb;

    public bool airAttack = false;
    public bool invulToStrike = false;

    public bool grounded = true;

    public Animator anim;

    // Start is called before the first frame update
    public void Start()
    {
        vfx = GetComponent<VFXHolder>().vfxPrefabs;
        id = FindObjectOfType<InputDetector>();//unComment
        anim = this.GetComponent<Animator>();
        t = GetComponentInChildren<Text>();

        Debug.Log(playerNumber);
        if (id.joysticks[playerNumber - 1] != null)//unComment
        {
            myAxisX = gameObject.tag + "_Horizontal" + /*"_360";*/id.joysticks[playerNumber - 1];
            myAxisY = gameObject.tag + "_Vertical" + /*"_360";*/id.joysticks[playerNumber - 1];
            myAxisAttack = gameObject.tag + "_Fire1" + /*"_360";*/id.joysticks[playerNumber - 1];
        }
        else
            Debug.LogError("No controller detected");

        if (GetComponent<Rigidbody2D>() != null)
            rb = GetComponent<Rigidbody2D>();
        else
            Debug.LogError("RigidBody is missing");

        //activeState = new Free(this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (t.color.a > 0)
            t.color -= new Color(0, 0, 0, Time.deltaTime * 1f);

        //activeState.StateUpdate();
    }

    public virtual void Hit()
    {

    }

    public virtual void HitBox()
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

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        rb.gravityScale = 5f;
        if (collision.gameObject.tag == "Ground" && grounded == false)
        {
            grounded = true;
            airAttack = false;
            airAction = false;

            if ((activeState is Jump || activeState is Dash))
                activeState = new Free(this);
        }
    }*/

    /*private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            Instantiate(vfx["Jump"], new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), Quaternion.identity);
        }
    }*/

    public void FlipX()
    {
        if (Input.GetAxis(myAxisX) > 0)
            transform.rotation = Quaternion.Euler(Vector3.zero);
        else if (Input.GetAxis(myAxisX) < 0)
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
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

