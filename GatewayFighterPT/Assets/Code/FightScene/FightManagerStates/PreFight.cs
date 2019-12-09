using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.Shoto;

namespace Assets.Code.FightScene
{
    public class PreFight : IFightBase
    {
        FightManager manager;

        public PreFight(FightManager managerRef)
        {
            manager = managerRef;

            foreach (CharacterState cs in Object.FindObjectsOfType<CharacterState>())
            {
                cs.enabled = false;
            }


        }

        public void StateStart()
        {
            
        }

        public void StateUpdate()
        {
            Debug.Log("PreFight says Hello");
            manager.activeState = new Fight(manager);
        }
    }
}