using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.CharacterSelect
{
    public class CharacterSelectManager : MonoBehaviour
    {
        public Cursor[] cursors;

        public GameObject player1Select;
        public GameObject player2Select;

        // Start is called before the first frame update
        void Start()
        {
            cursors = FindObjectsOfType<Cursor>();

            for (int i = 0; i < Input.GetJoystickNames().Length; i++)
            {
                cursors[i].PlayerNumber(i + 1);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}