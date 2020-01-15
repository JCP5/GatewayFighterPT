using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Code.Shoto;

namespace Assets.Code.FightScene
{
    public class PostFight : IFightBase
    {
        FightManager manager;
        int gameWinner;
        bool winScreenOn = false;
        float waitTime = 2f;

        public PostFight(FightManager managerRef, int winner)
        {
            manager = managerRef;
            Time.timeScale = 0.5f;

            foreach (CharacterState cs in manager.sceneCharacters)
            {
                cs.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                cs.enabled = false;
            }

            gameWinner = winner;
            WinScreen(winner);
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            if (waitTime > 0 && winScreenOn == false)
                waitTime -= Time.deltaTime;
            else if (waitTime <= 0 && winScreenOn == false)
            {
                waitTime = 2f;
                WinScreen(gameWinner);
            }
        }

        void WinScreen(int winner)
        {
            winScreenOn = true;
            Image winScreen = null;
            EventSystem es = Object.FindObjectOfType<EventSystem>();

            winScreen = manager.uiManager.SearchElements("WinScreen");

            if (winScreen != null)
            {
                if (winner == 0)
                    winScreen.GetComponentInChildren<Text>().text = "Draw!";
                else
                    winScreen.GetComponentInChildren<Text>().text = "Player " + winner + " wins!";

                winScreen.gameObject.SetActive(true);
                es.SetSelectedGameObject(winScreen.GetComponentInChildren<Button>().gameObject);
            }
            else
                Debug.LogError("***WinScreen not found***");
        }
    }
}