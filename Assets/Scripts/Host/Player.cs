using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour
{
    public float speed;
    public Rigidbody _rb;
    public LayerMask floorMask;
    public Transform root;

    public NetworkObject bulletPrefab;

    private void Awake()
    {
        if (!HasInputAuthority) return;

        root = Camera.main.transform.parent;
        Camera.main.transform.parent.parent = transform;
    }

    private void Update()
    {
        if (!HasInputAuthority) return;

        Camera.main.transform.localPosition = Vector3.zero;
        root?.Rotate(0, Input.GetAxis("Mouse X"), 0);
    }

    //public void Init()
    //{
    //    if (!HasInputAuthority) return;

    //    root = Camera.main.transform.parent;
    //    Camera.main.transform.parent.parent = transform;
    //}

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            data.dir.Normalize();
            transform.position += data.dir.normalized * speed * Runner.DeltaTime;

            if(data.jumpInput && Physics.Raycast(transform.position, Vector3.down, 1.5f, floorMask))
            {
                //_rb.AddForce(transform.up * 500);
                transform.position += transform.up * 2;
            }

            if (data.shootInput)
            {
                Runner.Spawn(bulletPrefab, transform.position + transform.forward);
            }
        }
    }
}
