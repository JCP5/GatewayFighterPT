using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public float moveSpeed = 10;
    public Rigidbody2D rb;
    public IShotoBase activeState;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Rigidbody2D>() != null)
            rb = GetComponent<Rigidbody2D>();
        else
            Debug.LogError("RigidBody is missing");

        activeState = new Free(this);
    }

    // Update is called once per frame
    void Update()
    {
        activeState.StateUpdate();
    }
}
