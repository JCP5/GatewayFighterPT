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

            foreach(Image img in manager.uiManager.elements)
            {
                if (img.name == "FadeBlack")
                    fade = img;
            }

            fade.GetComponent<Animator>().Play("FadeToBlack", 0, 0);
        }

        public void StateStart()
        {

        }

        public void StateUpdate()
        {

        }
    }
}