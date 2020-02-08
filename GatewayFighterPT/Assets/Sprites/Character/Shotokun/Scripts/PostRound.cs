using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.CharacterControl;

namespace Assets.Code.Shoto
{
    public class PostRound : ICharacterBase
    {
        ShotokunManager manager;

        public PostRound(ShotokunManager managerRef, bool victory)
        {
            manager = managerRef;

            if (victory == true)
                manager.anim.Play("RoundEnd", 0, 0);
            else
                manager.anim.Play("Hit", 0, 0);
        }

        public void StateStart()
        {

        }

        public void StateUpdate()
        {

        }
    }
}