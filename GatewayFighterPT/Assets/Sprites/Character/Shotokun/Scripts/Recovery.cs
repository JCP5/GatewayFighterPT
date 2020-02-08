using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.CharacterControl;

namespace Assets.Code.Shoto
{
    public class Recovery : ICharacterBase
    {
        ShotokunManager manager;

        public Recovery(ShotokunManager managerRef)
        {
            manager = managerRef;
            manager.rb.velocity = Vector2.zero;
            manager.rb.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;

            if (manager.risingSlash == true)
            {
                manager.risingSlash = false;
                manager.anim.Play("RisingSlashRecovery"); //CharacterState.AnimationFinish() event will reset state to Free or Jump
            }
            else if (manager.helmbreaker == true)
            {
                manager.helmbreaker = false;
                manager.anim.Play("HelmBreakerFinish");
            }

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