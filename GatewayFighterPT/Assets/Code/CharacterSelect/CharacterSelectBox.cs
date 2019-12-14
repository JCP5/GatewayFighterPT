using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.CharacterSelect
{
    public class CharacterSelectBox : MonoBehaviour
    {
        Color defaultColor;
        SpriteRenderer sr;

        Cursor[] cursors;
        public GameObject character;

        private void Start()
        {
            cursors = FindObjectsOfType<Cursor>();
            sr = this.GetComponent<SpriteRenderer>();
            defaultColor = sr.color;

            foreach (Cursor c in cursors)
                c.NewSelect += Unhighlight;
        }

        void Unhighlight()
        {
            bool isSelected = false;

            foreach (Cursor c in cursors)
            {
                if (c.highlighted == this.GetComponent<SpriteRenderer>())
                    isSelected = true;
            }

            if (isSelected == false)
                sr.color = defaultColor;
        }
    }
}