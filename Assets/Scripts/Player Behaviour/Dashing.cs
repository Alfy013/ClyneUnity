using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour
{
	private float directionY;
	private float directionX;
	private bool dashStarted;
    private bool isDashing;
	private float dashCooldown;
    [SerializeField] private float dashSpeed = 90f;
    [SerializeField] private float dashCD = 0.1f;

    [SerializeField] Transform groundDebrisPos;
	[SerializeField] GameObject groundDebris;
    [SerializeField] Animator animator;
	[SerializeField] PlayerHandler movement;
	[SerializeField] CharacterController controller;

    public void StartDash()
	{
		isDashing = true;
		ShakeHandler.Instance.ShakeCamera(5f, 0.6f);
        movement.stamina -= 5f;
    }
    public void StopDash()
	{
        dashStarted = false;
        isDashing = false;
        movement.actionPlaying = false;
        movement.stamRegen = 0f;
        dashCooldown = dashCD;
		directionY = 0f;
		directionX = 0f;
    }

    void Update()
	{
		animator.SetBool("isDashing", dashStarted);
		animator.SetFloat("directionY", directionY);
        animator.SetFloat("directionX", directionX);

        if (dashCooldown > 0f) dashCooldown -= Time.deltaTime;

        if(!movement.actionPlaying && dashCooldown <= 0f && movement.stamina >= 5f)
        {
            if (Input.GetAxis("Dash") > 0f && Input.GetAxisRaw("Horizontal") != 0f && Input.GetAxisRaw("Vertical") == 0f)
            {
                dashStarted = true;
                movement.actionPlaying = true;
                directionX = dashSpeed * Input.GetAxisRaw("Horizontal");
            }
            if (Input.GetAxis("Dash") > 0f && Input.GetAxisRaw("Vertical") != 0f && Input.GetAxisRaw("Horizontal") == 0f)
            {
                dashStarted = true;
                movement.actionPlaying = true;
                directionY = dashSpeed * Input.GetAxisRaw("Vertical");
            }
        }

        if (isDashing)
		{
            controller.Move(transform.forward * Time.deltaTime * directionY);
            controller.Move(transform.right * Time.deltaTime * directionX);
            groundDebris.transform.SetPositionAndRotation(groundDebrisPos.transform.position, groundDebrisPos.transform.rotation);
		}
    }

}
