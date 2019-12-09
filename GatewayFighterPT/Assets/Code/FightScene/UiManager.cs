using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Code.FightScene
{
    public class UiManager : MonoBehaviour
    {
        public Canvas mainCanvas;
        public Image[] elements;

        FightManager fm;

        private void Awake()
        {
            //Find reference to MainCanvas in scene
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.name == "MainCanvas")
                    mainCanvas = c;
            }

            elements = mainCanvas.GetComponentsInChildren<Image>();
        }

        private void Start()
        {
            fm = FindObjectOfType<FightManager>();
            foreach (Image img in elements)
            {
                if (img.name == "WinScreen" || img.name == "Pause")
                    img.gameObject.SetActive(false);
            }
        }

       public void Resume()
        {
            fm.activeState = new Fight(fm);
            Time.timeScale = 1f;
            fm.paused = false;
            SearchElements("Pause").gameObject.SetActive(false);
        }

        public void Rematch()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void CloseGame()
        {
            Application.Quit();
        }

        public Image SearchElements(string s)
        {
            foreach (Image img in elements)
            {
                if (img.name == s)
                    return img;
            }
            return null;
        }
    }
}
