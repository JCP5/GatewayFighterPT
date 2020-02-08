using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Code.MiscManagers;
using Assets.Code.CharacterControl;

namespace Assets.Code.FightScene
{
    public class Pause : IFightBase
    {
        FightManager manager;
        IFightBase activeState;
        UiManager uiManager;
        EventSystem es;
        bool startPressed = true;
        InputDetector inputManager;

        public Pause(FightManager managerRef, InputDetector id)
        { 
            manager = managerRef;
            uiManager = manager.uiManager;
            inputManager = id;
            es = Object.FindObjectOfType<EventSystem>();

            uiManager.SearchElements("Pause").gameObject.SetActive(true);
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(uiManager.SearchElements("Pause").GetComponentInChildren<Button>().gameObject);

            Time.timeScale = 0;

            foreach (CharacterState cs in Object.FindObjectsOfType<CharacterState>())
            {
                cs.GetComponent<CharacterState>().enabled = false;
            }
        }

        public void StateStart()
        {

        }

        public void StateUpdate()
        {
            if(Input.GetKeyDown(manager.p1Start) || Input.GetKeyDown(manager.p2Start))
            {
                uiManager.SearchElements("Pause").gameObject.SetActive(false);
                es.SetSelectedGameObject(null);

                Time.timeScale = 1f;
                manager.activeState = new Fight(manager);
            }
        }
    }
}