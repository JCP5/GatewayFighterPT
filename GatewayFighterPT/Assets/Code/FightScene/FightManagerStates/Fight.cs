using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Shoto;

namespace Assets.Code.FightScene
{
    public class Fight : IFightBase
    {
        FightManager manager;

        public Fight(FightManager managerRef)
        {
            manager = managerRef;

            foreach (CharacterState cs in Object.FindObjectsOfType<CharacterState>())
            {
                cs.GetComponent<CharacterState>().enabled = true;
            }
        }

        public void StateStart()
        {

        }

        public void StateUpdate()
        {
            if (Input.GetKeyDown(manager.p1Start) /*|| Input.GetKeyDown(manager.p2Start)*/)
                manager.PauseGame(manager);

            CountDown();
        }

        void CountDown()
        {
            manager.fightTimer -= Time.deltaTime;
            manager.seconds = Mathf.Floor(manager.fightTimer % 60f);
            manager.minutes = Mathf.Floor(manager.fightTimer / 60f);

            if (manager.fightTimer < 1)
            {
                manager.fightTimer = 0;
                CompareWins(manager.player1Wins, manager.player2Wins);
            }

            foreach (Image img in manager.uiManager.elements)
            {
                if (img.name == "Timer")
                {
                    if (manager.seconds >= 10)
                        img.GetComponentInChildren<Text>().text = manager.minutes.ToString() + " : " + Mathf.Round(manager.seconds).ToString();
                    else
                        img.GetComponentInChildren<Text>().text = manager.minutes.ToString() + " : " + "0" + Mathf.Round(manager.seconds).ToString();
                }
            }
        }

        void CompareWins(int p1Wins, int p2Wins)
        {
            if (p1Wins == p2Wins)
                manager.activeState = new PostFight(manager, 0);
            else if (p1Wins > p2Wins)
                manager.activeState = new PostFight(manager, 1);
            else if (p1Wins < p2Wins)
                manager.activeState = new PostFight(manager, 2);
        }
    }
}