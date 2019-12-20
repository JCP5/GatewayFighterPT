using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.FightScene;

namespace Assets.Code.Shoto
{
    public class PreRound : IShotoBase
    {
        ShotokunManager manager;

        public PreRound(ShotokunManager managerRef)
        {
            manager = managerRef;
            manager.anim.Play("RoundStart", 0, 0);
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