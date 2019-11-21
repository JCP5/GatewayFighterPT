using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Free : IShotoBase
{
    CharacterState manager;

    public Free(CharacterState managerRef)
    {
        manager = managerRef;
    }

    public void StateStart()
    {
        throw new System.NotImplementedException();
    }

    public void StateUpdate()
    {
        Debug.Log(Input.GetAxis("Horizontal"));
        if (Mathf.Abs(Input.GetAxis("Horizontal")) == 1)
        {
            manager.rb.velocity = new Vector2(Input.GetAxis("Horizontal") * manager.moveSpeed * Time.fixedDeltaTime, 0);

            //manager.rb.AddForce(new Vector2((Input.GetAxis("Horizontal")/ Mathf.Abs(Input.GetAxis("Horizontal"))) * manager.moveSpeed * Time.deltaTime, 0), ForceMode2D.Force);
        }
        else 
            manager.rb.velocity = new Vector2(0, 0);
    }
}
