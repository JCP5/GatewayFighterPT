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
        Dictionary<string, Vector2> inputType = new Dictionary<string, Vector2>();
        Dictionary<string, Vector2[]> commandList = new Dictionary<string, Vector2[]>();
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
            InitInputTypes();
        }

        // Update is called once per frame
        void Update()
        {
            PopulateInputList();
            HoldTimer();
            InputTimer();
        }

        void InitInputTypes()
        {
            inputType.Add("up", Vector2.up);
            inputType.Add("upRight", Vector2.up + Vector2.right);
            inputType.Add("right", Vector2.right);
            inputType.Add("downRight", Vector2.right + Vector2.down);
            inputType.Add("down", Vector2.down);
            inputType.Add("downLeft", Vector2.down + Vector2.left);
            inputType.Add("Left", Vector2.left);
            inputType.Add("upLeft", Vector2.left + Vector2.up);
            InitCommandList();
        }

        void InitCommandList()
        {
            commandList.Add("SideDash", new Vector2[] { inputType["right"], inputType["right"] });
            commandList.Add("IAD", new Vector2[] { inputType["upRight"], inputType["right"] });
            commandList.Add("EmptyDash", new Vector2[] { inputType["down"], inputType["down"] });
            commandList.Add("EmptyIAD", new Vector2[] { inputType["upRight"], inputType["down"] });

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
            SetTimeBetweenInputs(timer);
            //DoubleTap Commands
            if (inputs.Count > 1)
            {
                //Double tap commands
                if (inputs[inputs.Count - 1] != Vector2.zero)
                {
                    Vector2[] temp;
                    temp = new Vector2[2];
                    temp[0] = inputs[inputs.Count - 2];
                    temp[1] = inputs[inputs.Count - 1];

                    if (AbsoluteX(temp[0]) == commandList["SideDash"][0] && AbsoluteX(temp[1]) == commandList["SideDash"][1] && timeBetweenInputs < doubleTapWindow && temp[0] == temp[1])
                    {
                        if (DashEvent != null)
                            DashEvent(temp[1].x);
                    }

                    if (FlipByRight(temp[0]) == commandList["IAD"][0] && FlipByRight(temp[1]) == commandList["IAD"][1] && timeBetweenInputs < doubleTapWindow && overloadCheck == false)
                    {
                        if (DashEvent != null)
                            DashEvent(temp[1].x);
                    }

                    if (FlipByRight(temp[0]) == commandList["EmptyDash"][0] && FlipByRight(temp[1]) == commandList["EmptyDash"][1] && timeBetweenInputs < doubleTapWindow && overloadCheck == false)
                    {
                        if (DashEvent != null)
                            DashEvent(temp[1].x);
                    }

                    /*if((inputs[inputs.Count - 1].y == inputs[inputs.Count - 2].y && inputs[inputs.Count - 1].y == -1) && inputs[inputs.Count - 1].x == 0 && inputs[inputs.Count - 1].y != 0 && timeBetweenInputs < doubleTapWindow)
                    {
                        //Empty Dash
                        Debug.Log("DownDown");
                        Debug.Log("DashEvent");
                        if (DashEvent != null)
                            DashEvent(inputs[inputs.Count - 1].x);
                    }
                    else if ((inputs[inputs.Count - 1].x == inputs[inputs.Count - 2].x) && inputs[inputs.Count - 1].x != 0 && timeBetweenInputs < doubleTapWindow)
                    {
                        Debug.Log("SideSide");
                        Debug.Log("DashEvent");
                        if (DashEvent != null)
                            DashEvent(inputs[inputs.Count - 1].x);
                    }*/
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
            //Debug.Log(inputs[inputs.Count - 1]);
        }

        Vector2 FlipByRight(Vector2 input)
        {
            Vector2 temp;
            temp = new Vector2(transform.right.x * input.x, input.y);//test more later

            return temp;
        }

        Vector2 AbsoluteX(Vector2 v2)
        {
            Vector2 temp;
            temp = new Vector2(Mathf.Abs(v2.x), v2.y);

            return temp;
        }
    }
}