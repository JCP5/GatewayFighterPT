using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Code.Shoto;
using Assets.Code.MiscManagers;

namespace Assets.Code.FightScene
{
    public class FightManager : MonoBehaviour
    {
        public IFightBase activeState;

        public AudioClip defaultBgm;

        public GameObject[] characters;
        public UiManager uiManager;
        public EventSystem es;

        public bool paused = false;

        public float fightTimer = 180f;
        public float minutes;
        public float seconds;

        int rounds = 0;
        public int player1Wins = 0;
        public int player2Wins = 0;

        public string p1Start = "";
        public string p2Start = "";

        public List<Transform> spawnLocations = new List<Transform>();
        public CharacterState[] sceneCharacters;
        public InputDetector inputManager;

        // Start is called before the first frame update
        void Start()
        {
            if (FindObjectOfType<EventSystem>() != null)
                es = FindObjectOfType<EventSystem>();
            else
                Debug.LogError("EventSystem not found");

            inputManager = FindObjectOfType<InputDetector>();
            List<Vector3> used = new List<Vector3>();

            DetectControllerStart();
            IntializeSpawnLocations();

            for (int i = 0; i < characters.Length; i++)
            {
                //characters[] is a GameObject array that houses what characters will be spawned in the scene. 
                //Will be set by a different class after character selection screen
                //If changing colours, remember to reset the prefab to white after you spawn
                characters[i].GetComponent<CharacterState>().PlayerNumber(i + 1);
                characters[i].GetComponent<CharacterState>().enabled = false;

                //temporary
                if (i == 0)
                    characters[i].GetComponent<SpriteRenderer>().color = Color.red;
                else
                    characters[i].GetComponent<SpriteRenderer>().color = Color.cyan;

                SpawnCharacterTemp(characters[i], spawnLocations[i]);
                //SpawnCharacters(characters[i], used);

                characters[i].GetComponent<SpriteRenderer>().color = Color.white;
            }

            uiManager = FindObjectOfType<UiManager>();
            sceneCharacters = Object.FindObjectsOfType<CharacterState>();

            minutes = fightTimer / 60f;
            seconds = 0f;

            activeState = new RoundStart(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (activeState == null)
                activeState = new RoundStart(this);
            activeState.StateUpdate();
        }

        public void PauseGame(FightManager fm)
        {
            if ((Input.GetAxis("P1_Pause" + inputManager.joysticks[0]) == 1 || Input.GetAxis("P2_Pause" + inputManager.joysticks[1]) == 1) && paused == false)
            {
                activeState = new Pause(fm, inputManager);
            }
        }

        //Temp. Detect which controller is being used and maps the start button accordingly
        //Possible basis for input management rework
        void DetectControllerStart()
        {
            if (inputManager.joysticks != null)
            {
                if (inputManager.joysticks[0] == "_360")
                {
                    p1Start = "joystick 1 button 7";
                }
                else
                {
                    p1Start = "joystick 1 button 9";
                }

                if (inputManager.joysticks[1] == "_360")
                {
                    p2Start = "joystick 2 button 7";
                }
                else
                {
                    p2Start = "joystick 2 button 9";
                }
            }
        }

        int CheckUsedV3(List<Vector3> used, int temp)
        {
            int i = temp;
            foreach (Vector3 v in used)
            {
                if (spawnLocations[i].position == v)
                {
                    i = RandomNumber();
                    CheckUsedV3(used, i);
                }
            }
            return i;
        }

        void IntializeSpawnLocations()
        {
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.parent != null)
                {
                    spawnLocations.Add(t);
                }
            }
        }

        int RandomNumber()
        {
            return Random.Range(0, spawnLocations.Count - 1);
        }


        void SpawnCharacterTemp(GameObject go, Transform t)
        {
            Instantiate(go, t.position, Quaternion.identity);
        }

        /*void SpawnCharacters(GameObject go, List<Vector3> used)
        {
            int temp = RandomNumber();

            temp = CheckUsedV3(used, temp);

            Instantiate(go, spawnLocations[temp].position, Quaternion.identity);
            used.Add(spawnLocations[temp].position);
        }*/

        public void UpdateWins(int i)
        {
            switch (i)
            {
                case (1):
                    player1Wins++;
                    for (int j = 0; j < player1Wins; j++)
                    {
                        Image[] img = uiManager.transform.Find("Player1").GetComponentInChildren<GridLayoutGroup>().GetComponentsInChildren<Image>();
                        img[j].color = Color.red;
                    }
                    break;

                case (2):
                    player2Wins++;
                    for (int j = 0; j < player2Wins; j++)
                    {
                        Image[] img = uiManager.transform.Find("Player2").GetComponentInChildren<GridLayoutGroup>().GetComponentsInChildren<Image>();
                        img[j].color = Color.red;
                    }
                    break;
            }

            if (player2Wins == 5 && player1Wins == 5)
            {
                activeState = new PostFight(this, 0);
            }
            else if (player1Wins == 5 || player2Wins == 5)
            {
                activeState = new PostFight(this, i);
            }
            else if (player1Wins < 5 || player2Wins < 5)
            {
                EndRound();
            }
        }

        //-----------------------------------Round Handling--------------------------------
        public void AddRound()
        {
            rounds++;

            foreach (Image img in uiManager.elements)
            {
                if (img.name == "FadeBlack")
                {
                    img.GetComponentInChildren<Text>().text = "Round " + rounds;
                }
            }
        }

        public void DisableCharacters()
        {
            foreach (CharacterState cs in sceneCharacters)
            {
                cs.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        public void EnableCharacters()
        {
            foreach (CharacterState cs in sceneCharacters)
            {
                cs.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                cs.Free();
            }
        }

        public void TextToFight()
        {
            foreach (Image img in uiManager.elements)
            {
                if (img.name == "FadeBlack")
                {
                    img.GetComponentInChildren<Text>().text = "Fight!";
                }
            }

            activeState = new Fight(this);
        }

        public void ResetCharacters()
        {
            foreach (CharacterState cs in sceneCharacters)
            {
                cs.ResetGravityScale();
                cs.grounded = true;
                cs.GetComponent<SpriteRenderer>().enabled = true;
                cs.PreRound();
                cs.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                cs.transform.position = spawnLocations[cs.playerNumber - 1].position;
            }
        }

        public void StartRound()
        {
            activeState = new RoundStart(this);
        }

        public void EndRound()
        {
            activeState = new RoundEnd(this);
        }
    }
}