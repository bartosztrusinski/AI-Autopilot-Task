using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  [Header("References")]
  public Transform player;

  [Header("Movement")]
  public float moveSpeed;

  float horizontalInput;
  float verticalInput;
  Vector3 moveDirection;
  Rigidbody rb;

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
    rb.freezeRotation = true;

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  private void FixedUpdate()
  {
    MovePlayer();
  }

  private void Update()
  {
    horizontalInput = Input.GetAxis("Horizontal");
    verticalInput = Input.GetAxis("Vertical");

    SpeedControl();
  }

  private void MovePlayer()
  {
    moveDirection = player.forward * verticalInput + player.right * horizontalInput;
    rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
  }

  private void SpeedControl()
  {
    Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    if (flatVelocity.magnitude > moveSpeed)
    {
      Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
      rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
    }
  }

}
