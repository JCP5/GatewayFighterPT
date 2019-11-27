using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.MiscManagers
{
    public class InputDetector : MonoBehaviour
    {
        public string[] joysticks;

        // Start is called before the first frame update
        void Start()
        {
            joysticks = Input.GetJoystickNames();

            for (int i = 0; i < joysticks.Length; i++)
            {
                if (joysticks[i].IndexOf("PS4", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    joysticks[i] = "_PS4";
                }
                else if (joysticks[i].IndexOf("360", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    joysticks[i] = "_360";
                }
            }
        }
    }
}