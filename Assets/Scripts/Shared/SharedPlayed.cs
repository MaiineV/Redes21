using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SharedPlayed : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float _speed;

    private bool _jump;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityForce;
    private Vector3 _velocity;

    [SerializeField] private NetworkMecanimAnimator _animator;

    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [Networked, OnChangedRender(nameof(ChangeColor))] public Color _teamColor { get; set; }

    private bool hasTeam = false;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            SetCameraOwner();
            StartCoroutine(WaitInit());
        }
    }

    private void Update()
    {
        if (HasStateAuthority)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _jump = true;
            if (Input.GetKeyDown(KeyCode.F))
                _teamColor = Color.green;
        }
    }

    public override void FixedUpdateNetwork()
    {
        var dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * _speed * Runner.DeltaTime;

        _animator.Animator.SetFloat("dir", dir.magnitude);


        if (characterController.isGrounded)
            _velocity = Vector3.zero;

        if (dir.magnitude > 0)
        {
            transform.forward = dir;
        }

        _velocity.y += _gravityForce * Runner.DeltaTime;

        if (_jump && characterController.isGrounded)
        {
            _velocity.y += _jumpForce;
        }

        characterController.Move(dir + _velocity * Runner.DeltaTime);

        _jump = false;
    }

    private void SetCameraOwner()
    {
        Camera.main.transform.parent.parent = transform;
        Camera.main.transform.parent.localPosition = Vector3.zero;
        //Puede usar GetComponent y cambiarle settings
    }

    private void ChangeColor()
    {
        _meshRenderer.material.color = _teamColor;
    }

    private IEnumerator WaitInit()
    {
        yield return new WaitForSeconds(2);
        if (Runner.ActivePlayers.Count() % 2 == 0)
        {
            _teamColor = Color.red;
        }
        else
        {
            _teamColor = Color.blue;
        }
    }
}
