using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.FightScene
{
    public class FadeEventHandler : MonoBehaviour
    {
        FightManager fm;
        UiManager um;

        private void Start()
        {
            fm = FindObjectOfType<FightManager>();
            um = FindObjectOfType<UiManager>();
        }

        public void DisableCharacters()
        {
            fm.DisableCharacters();
        }

        public void EnableCharacters()
        {
            fm.EnableCharacters();
        }

        public void RoundPlus()
        {
            fm.AddRound();
        }

        public void TextToFight()
        {
            fm.TextToFight();
        }

        public void ResetCharacters()
        {
            fm.ResetCharacters();
        }

        public void StartRound()
        {
            fm.StartRound();
        }
    }
}
