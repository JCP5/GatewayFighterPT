using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.TimeCop
{
    public class Throw : MonoBehaviour
    {
        Vector2 direction;
        Rigidbody2D rb;
        public float distance = 3f;
        public float moveSpeed = 10f;
        public bool thrown = false;

        // Start is called before the first frame update
        void Start()
        {
            transform.parent = null;
            direction = Vector2.zero;
            rb = this.GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {

        }

        public void SetDirection(Vector2 dir)
        {
            direction = dir;
            rb.AddForce(new Vector2(dir.x * moveSpeed, dir.y * moveSpeed), ForceMode2D.Impulse);
        }
    }
}