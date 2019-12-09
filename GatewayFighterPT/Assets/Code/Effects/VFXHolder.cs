using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Effects
{
    public class VFXHolder : MonoBehaviour
    {
        public Dictionary<string, GameObject> vfxPrefabs = new Dictionary<string, GameObject>();
        public GameObject[] prefabs = new GameObject[0];

        private void Start()
        {
            foreach (GameObject go in prefabs)
            {
                vfxPrefabs.Add(go.name, go);
            }
        }
    }
}