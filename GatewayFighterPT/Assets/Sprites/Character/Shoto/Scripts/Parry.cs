using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Shoto
{
    public class Parry : IShotoBase
    {
        CharacterState manager;

        public Parry(CharacterState managerRef)
        {
            manager = managerRef;
            manager.anim.Play("6_Parry"); //CharacterState.AnimationFinish() event will reset state to Free or Jump
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {

        }
    }
}