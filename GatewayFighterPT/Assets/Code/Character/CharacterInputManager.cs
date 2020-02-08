using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.CharacterControl
{
    public class CharacterInputManager : MonoBehaviour
    {
        public delegate void dashEvent(float direction);
        public event dashEvent DashEvent;
        public delegate void downHeldEvent();
        public event downHeldEvent DownHeldEvent;
        public delegate void neutralEvent();
        public event neutralEvent NeutralEvent;

        public CharacterState character;

        Vector2 inputVector;
        public List<Vector2> inputs = new List<Vector2>();
        bool overloadCheck = false;

        public float doubleTapWindow = 0.2f;
        public float heldTimeWindow = 0.1f;
        public float timeBetweenInputs = 0f;
        public float timer = 0f;
        public float heldTimer = 0f;

        // Start is called before the first frame update
        void Start()
        {
            character = this.GetComponent<CharacterState>();
        }

        // Update is called once per frame
        void Update()
        {
            PopulateInputList();
            HoldTimer();
            InputTimer();
        }

        //doubletap Checking ---------------------------------------------------------
        void InputTimer()
        {
            timer = Mathf.Clamp(timer + Time.fixedDeltaTime, 0, 1f);
        }

        void HoldTimer()
        {
            if (overloadCheck == true)
            {
                heldTimer = Mathf.Clamp(heldTimer + Time.fixedDeltaTime, 0, 1f);

                if (heldTimer > heldTimeWindow && Input.GetAxis(character.myAxisY) == -1)//revise later to make it more flexible
                    CommandListener(inputs);
            }
            else
                heldTimer = 0;
        }

        void SetTimeBetweenInputs(float f)
        {
            timeBetweenInputs = f;

            if (inputs.Count > 1)
            {
                if (inputs[inputs.Count - 1] != inputs[inputs.Count - 2])
                    timer = 0f;
                else if (timeBetweenInputs > doubleTapWindow)
                    timer = 0f;
            }
            else
                timer = 0f;
        }
        //doubletap Checking ---------------------------------------------------------

        void PopulateInputList()
        {
            inputVector = new Vector2(Mathf.Round(Input.GetAxis(character.myAxisX)), Mathf.Round(Input.GetAxis(character.myAxisY)));

            if (inputVector != Vector2.zero)
            {
                if (overloadCheck == false)
                {
                    inputs.Add(inputVector);
                    SetTimeBetweenInputs(timer);
                    ListCeiling(inputs);
                    CommandListener(inputs);
                    overloadCheck = true;
                }
                else
                {
                    if (inputs.Count < 10)
                    {
                        if (inputVector != inputs[inputs.Count - 1])
                        {
                            inputs.Add(inputVector);
                            ListCeiling(inputs);
                            CommandListener(inputs);
                        }
                    }
                    else if (inputs.Count >= 10)
                    {
                        if (inputVector != inputs[9])
                        {
                            inputs.Add(inputVector);
                            ListCeiling(inputs);
                            CommandListener(inputs);
                        }
                    }
                }
            }
            else
            {
                //Debug.Log("NeutralEvent");
                if (NeutralEvent != null)
                {
                    NeutralEvent();
                }
                overloadCheck = false;
            }
        }

        void ListCeiling(List<Vector2> l)
        {
            if (l.Count > 10)
            {
                l.RemoveAt(0);
            }
        }

        void CommandListener(List<Vector2> l)
        {
            //DoubleTap Commands
            if (inputs.Count > 1)
            {
                //DashCheck
                if (inputs[inputs.Count - 1] != Vector2.zero)
                {
                    if ((inputs[inputs.Count - 1].x == inputs[inputs.Count - 2].x) && (timeBetweenInputs < doubleTapWindow))
                    {
                        Debug.Log("DashEvent");
                        if (DashEvent != null)
                            DashEvent(inputs[inputs.Count - 1].x);
                    }
                }
            }
            if (inputs.Count > 0)
            {
                if (inputs[inputs.Count - 1].y == -1 && overloadCheck == true && heldTimer >= heldTimeWindow)
                {
                    Debug.Log("DownHeldEvent");
                    if (DownHeldEvent != null)
                        DownHeldEvent();
                }
            }
        }
    }
}