using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.MiscManagers
{
    public class InputDetector : MonoBehaviour
    {
        public string[] joysticks;//returns what type of controller is plugged in to which slot.

        // Start is called before the first frame update
        void Awake()
        {
            joysticks = Input.GetJoystickNames();

            for (int i = 0; i < joysticks.Length; i++)
            {
                Debug.Log(joysticks[i]);

                if (joysticks[i].IndexOf("360", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    joysticks[i] = "_360";
                }
                else
                {
                    joysticks[i] = "_PS4";
                }
            }
        }
    }
}