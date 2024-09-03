using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    public float speed;
    public Rigidbody _rb;

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            data.dir.Normalize();
            _rb.AddForce(data.dir * speed * Runner.DeltaTime);
        }
    }
}
