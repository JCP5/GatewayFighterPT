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

        public PostFight(FightManager managerRef, int winner)
        {
            manager = managerRef;

            foreach (CharacterState cs in manager.sceneCharacters)
            {
                cs.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                cs.enabled = false;
            }
            WinScreen(winner);
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            
        }

        void WinScreen(int winner)
        {
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