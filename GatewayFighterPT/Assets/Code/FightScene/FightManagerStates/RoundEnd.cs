using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Shoto;

namespace Assets.Code.FightScene
{
    public class RoundEnd : IFightBase
    {
        FightManager manager;
        Image fade;

        public RoundEnd(FightManager managerRef)
        {
            manager = managerRef;

            manager.fadeEventHandler.GetComponent<Animator>().Play("FadeToBlack", 0, 0);
        }

        public void StateStart()
        {

        }

        public void StateUpdate()
        {

        }
    }
}