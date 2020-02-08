using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.CharacterControl
{
    public interface ICharacterBase
    {
        void StateStart();
        void StateUpdate();
    }
}