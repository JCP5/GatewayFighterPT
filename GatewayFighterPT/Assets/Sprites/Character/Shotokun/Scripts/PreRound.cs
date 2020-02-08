using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.FightScene;
using Assets.Code.CharacterControl;

namespace Assets.Code.Shoto
{
    public class PreRound : ICharacterBase
    {
        ShotokunManager manager;

        public PreRound(ShotokunManager managerRef)
        {
            manager = managerRef;
            manager.rb.velocity = Vector2.zero;
            manager.anim.Play("RoundStart", 0, 0);
            manager.ResetActions();
        }

        public void StateStart()
        {
            throw new System.NotImplementedException();
        }

        public void StateUpdate()
        {
            manager.FaceOpponent(manager.transform.position, Vector3.zero);
        }
    }
}