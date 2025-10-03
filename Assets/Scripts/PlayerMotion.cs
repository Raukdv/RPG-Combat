using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotion : MonoBehaviour
{
    public Transform cam;
    public CinemachineFreeLook cinemachineFreeLook;
    public GameObject targetCam;
    public float speed;
    public float speedRotation = 10;
    public float groundDistanceUp, groundDistance;
    public float jumpPower = 35;
    public float gravity = 9.81f;
    public float gravityPlayer = 1;
    public float rotationSpeedCamX, rotationSpeedCamY;
    public bool onGround, isJump;
    public bool stop;
    public LayerMask groundLayer;
    //public Transform groundCheck;
    Rigidbody rb;
    Animator anim;
    Vector2 _move, m_look;
    Vector3 move;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + (Vector3.up * groundDistanceUp), groundDistance);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //Comprobacion de si esta en el suelo
        onGround = Physics.CheckSphere(transform.position + (Vector3.up * groundDistanceUp), groundDistance, groundLayer);
        if (!onGround)
            rb.AddForce(-gravity * gravityPlayer * Vector3.up, ForceMode.Acceleration);
        if (onGround && isJump)
        {
            isJump = false;
            anim.SetBool("OnAir", false);
            rb.velocity = Vector3.zero;
        }
        else if (!onGround && !isJump)
        {
            anim.SetBool("OnAir", true);
            isJump = true;
            Stopping();
            anim.SetTrigger("Fall");
        }

        //Detencion del movimiento
        if (stop) return;
        //Movimiento del personaje
        if (_move.x != 0 || _move.y != 0)
        {
            move = cam.forward * _move.y + cam.right * _move.x;
            move.Normalize();
            move.y = 0;
            rb.velocity = move * speed;
            Vector3 dir = cam.forward * _move.y + cam.right * _move.x;
            dir.Normalize();
            dir.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            Quaternion playerRoration = Quaternion.Slerp(transform.rotation, targetRotation, speedRotation * Time.fixedDeltaTime);
            transform.rotation = playerRoration;
        }
    }
    //Captura del input de movimiento para animaciones y movimiento
    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
        if (stop) return;
        if (_move.x == 0 && _move.y == 0)
            rb.velocity = Vector3.zero;
        anim.SetBool("Move", (_move.x == 0 && _move.y == 0) ? false : true);
        anim.SetFloat("Moving", (_move.x == 0 && _move.y == 0) ? 0 : 1);
        anim.SetFloat("MoveX", _move.x);
        anim.SetFloat("MoveY", _move.y);

    }
    public void OnJump(InputValue value)
    {
        Stopping();
        isJump = true;
        Vector2 moveDir = _move;
        anim.SetTrigger("Jumping");
        if (moveDir != Vector2.zero)
        {
            Vector3 dir = cam.forward * moveDir.y + cam.right * moveDir.x;
            dir.y = 0;
            dir.Normalize();
            Quaternion targetR = Quaternion.LookRotation(dir);
            transform.rotation = targetR;
            rb.AddForce((transform.forward + Vector3.up) * jumpPower, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
        anim.SetBool("OnAir", true);

    }
    public void Stopping()
    {
        if (onGround)
            rb.velocity = Vector3.zero;
        stop = true;
        anim.SetFloat("MoveX", 0);
        anim.SetFloat("MoveY", 0);
        anim.SetFloat("Moving", 0);
        anim.SetBool("Move", false);

    }
    public void StopEnd()
    {
        anim.SetBool("Move", (_move.x == 0 && _move.y == 0) ? false : true);
        anim.SetFloat("Moving", (_move.x == 0 && _move.y == 0) ? 0 : 1);
        anim.SetFloat("MoveX", _move.x);
        anim.SetFloat("MoveY", _move.y);
        rb.velocity = Vector3.zero;
        stop = false;
    }
    public void OnCam(InputValue value)
    {
        m_look = value.Get<Vector2>();
        cinemachineFreeLook.m_XAxis.Value += m_look.x * rotationSpeedCamX;
        cinemachineFreeLook.m_YAxis.Value += m_look.x * rotationSpeedCamX * Time.fixedDeltaTime;
    }
    public void FallEnd()
    {
        StopEnd();
    }
}