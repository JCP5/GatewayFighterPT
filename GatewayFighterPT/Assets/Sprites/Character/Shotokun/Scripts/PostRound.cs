using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class PostRound : IShotoBase
    {
        CharacterState manager;

        public PostRound(CharacterState managerRef, bool victory)
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