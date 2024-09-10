using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    public override void FixedUpdateNetwork()
    {
        transform.position += transform.forward * 2 * Runner.DeltaTime;
    }
}
