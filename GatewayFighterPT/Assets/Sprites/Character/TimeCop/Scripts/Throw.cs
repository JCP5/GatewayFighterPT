using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.TimeCop
{
    public class Throw : MonoBehaviour
    {
        TimeCopManager timeCop;
        Vector2 direction;
        public float moveSpeed = 10f;
        public bool thrown = false;

        // Start is called before the first frame update
        void Start()
        {
            if (transform.parent != null)
                timeCop = GetComponentInParent<TimeCopManager>();
            else
                Debug.LogError("TimeCop not found");

            transform.parent = null;
            direction = Vector2.zero;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.position += new Vector3(direction.x * Time.fixedDeltaTime * moveSpeed, direction.y * Time.fixedDeltaTime * moveSpeed, 0);
        }

        public void SetDirection(Vector2 dir)
        {
            direction = dir;
        }
    }
}