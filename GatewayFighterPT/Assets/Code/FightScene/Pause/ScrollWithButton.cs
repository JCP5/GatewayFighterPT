using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.ButtonScrollUI
{
    public class ScrollWithButton : MonoBehaviour
    {
        float vPos;
        public Button[] buttons;
        ScrollRect scrollRect;

        private void Start()
        {
            scrollRect = GetComponent<ScrollRect>();
            buttons = GetComponentsInChildren<Button>();

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<ScrollListener>().index = i;
            }
        }

        public void SetVertical(int i)
        {
            float index = (float)i;
            vPos = 1f - index / (buttons.Length - 1);
            scrollRect.verticalNormalizedPosition = vPos;
        }
    }
}