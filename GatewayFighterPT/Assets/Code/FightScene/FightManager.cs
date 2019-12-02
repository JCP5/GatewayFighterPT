using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Code.Shoto;

namespace Assets.Code.FightScene
{
    public class FightManager : MonoBehaviour
    {
        public GameObject[] characters;
        public UiManager uiManager;

        public static float fightTimer = 180f;
        float minutes;
        float seconds;

        int player1Wins = 0;
        int player2Wins = 0;

        public List<Transform> spawnLocations = new List<Transform>();

        // Start is called before the first frame update
        void Start()
        {
            List<Vector3> used = new List<Vector3>();

            IntializeSpawnLocations();

            for (int i = 0; i < characters.Length; i++)
            {
                //If changing colours, remember to reset the prefab to white after you spawn
                characters[i].GetComponent<CharacterState>().PlayerNumber(i + 1);
                SpawnCharacters(characters[i], used);
            }

            uiManager = FindObjectOfType<UiManager>();

            minutes = fightTimer / 60f;
            seconds = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            CountDown();
        }

        void CountDown()
        {
            fightTimer -= Time.deltaTime;
            seconds = Mathf.Floor(fightTimer % 60f);
            minutes = Mathf.Floor(fightTimer / 60f);

            foreach (Image img in uiManager.elements)
            {
                if (img.name == "Timer")
                {
                    if (seconds >= 10)
                        img.GetComponentInChildren<Text>().text = minutes.ToString() + " : " + Mathf.Round(seconds).ToString();
                    else
                        img.GetComponentInChildren<Text>().text = minutes.ToString() + " : " + "0" + Mathf.Round(seconds).ToString();
                }
            }
        }

        void SpawnCharacters(GameObject go, List<Vector3> used)
        {
            int temp = RandomNumber();

            temp = CheckUsedV3(used, temp);

            Instantiate(go, spawnLocations[temp].position, Quaternion.identity);
            used.Add(spawnLocations[temp].position);

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

        int RandomNumber()
        {
            return Random.Range(0, spawnLocations.Count - 1);
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

        public void UpdateWins(string s)
        {
            switch(s)
            {
                case ("P1"):
                    player1Wins++;
                    for (int i = 0; i < player1Wins; i++)
                    {
                        Image[] img = uiManager.transform.Find("Player1").GetComponentInChildren<GridLayoutGroup>().GetComponentsInChildren<Image>();
                        img[i].color = Color.red;
                    }
                    break;

                case ("P2"):
                    player2Wins++;
                    for (int i = 0; i < player1Wins; i++)
                    {
                        Image[] img = uiManager.transform.Find("Player2").GetComponentInChildren<GridLayoutGroup>().GetComponentsInChildren<Image>();
                        img[i].color = Color.red;
                    }
                    break;
            }
        }
    }
}