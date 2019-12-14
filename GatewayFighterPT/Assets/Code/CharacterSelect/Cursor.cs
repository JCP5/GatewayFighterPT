using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Code.MiscManagers;

namespace Assets.Code.CharacterSelect
{
    public class Cursor : MonoBehaviour
    {
        public delegate void CursorMove();
        public event CursorMove NewSelect;

        string controllerType;
        int playerNumber;
        static InputDetector inputDetector;
        public float moveSpeed = 10;

        static CharacterSelectManager manager;
        public SpriteRenderer highlighted;
        bool selectionComplete = false;

        // Start is called before the first frame update
        void Start()
        {
            manager = FindObjectOfType<CharacterSelectManager>();
            inputDetector = FindObjectOfType<InputDetector>();
        }

        // Update is called once per frame
        void Update()
        {
            if (selectionComplete == false)
            {
                transform.position += new Vector3(Input.GetAxis(this.tag + "_Horizontal" + controllerType) * moveSpeed * Time.deltaTime,
                    Input.GetAxis(this.tag + "_Vertical" + controllerType) * moveSpeed * Time.deltaTime, 0);

                if (Input.GetAxis(this.tag + "_Fire1" + controllerType) == 1)
                {
                    if (playerNumber == 1)
                        manager.player1Select = highlighted.GetComponent<CharacterSelectBox>().character;
                    else if (playerNumber == 2)
                        manager.player2Select = highlighted.GetComponent<CharacterSelectBox>().character;
                }
            }
        }

        public void PlayerNumber(int i)
        {
            int number = i;

            Debug.Log(inputDetector);
            controllerType = inputDetector.joysticks[i - 1];
            playerNumber = i;

            if (i == 1)
                this.tag = "P1";
            else if (i == 2)
                this.tag = "P2";
        }

        //Add audio later
        private void OnTriggerEnter2D(Collider2D collision)
        {
            highlighted = collision.GetComponent<SpriteRenderer>();

            highlighted.color = this.GetComponent<SpriteRenderer>().color;

            if (NewSelect != null)
                NewSelect();
        }
    }
}