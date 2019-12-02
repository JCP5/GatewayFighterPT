using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.FightScene
{
    public class UiManager : MonoBehaviour
    {
        public Canvas mainCanvas;
        public Image[] elements;

        private void Start()
        {
            //Find reference to MainCanvas in scene
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.name == "MainCanvas")
                    mainCanvas = c;
            }

            elements = mainCanvas.GetComponentsInChildren<Image>();
        }
    }
}
