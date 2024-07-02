using Cinemachine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MovementHandler : MonoBehaviour
{
	//General movement and action variables
	[SerializeField] Animator _animator;
	[SerializeField] float _baseSpeed = 7f;
	[SerializeField] float _focusModifier = 2;
	[SerializeField] AfterImage aim;
	readonly float _gravity = -9.81f;

	CharacterController _controller;

	private Vector3 xMoveDirection;
	private Vector3 zMoveDirection;
	private Vector3 moveVelocity;
	private float baseSpeed;
	private float focusModifierVert;
	private float focusModifierHor;
	private float vertSpeed;
	private float horSpeed;

	private Vector3 jumpVec;
	private Vector3 gravPull;

	private void AnimatorSet()
	{
		_animator.SetFloat("Vertical", vertSpeed);
		_animator.SetFloat("Horizontal", horSpeed);
		_animator.SetInteger("RawVertical", (int)Input.GetAxisRaw("Vertical"));
		_animator.SetInteger("RawHorizontal", (int)Input.GetAxisRaw("Horizontal"));
	}
	private void Gravity()
	{
		// While on the ground, reset the vertical velocity(so it doesn't smash into the ground) and reset the jump timer
		if (_controller.isGrounded)
		{
			gravPull.y = 0f;
		}

		//Apply gravity while the player isn't on the ground and isn't in the middle of a jump
		if (!_controller.isGrounded)
			gravPull.y += _gravity * Time.deltaTime * 5f;
	}
	private void Movement()
	{


		if (_controller.isGrounded)
			baseSpeed = _baseSpeed;

		focusModifierVert = _focusModifier * Input.GetAxis("Running") * Input.GetAxis("Vertical") * baseSpeed;
		focusModifierHor = _focusModifier * Input.GetAxis("Running") * Input.GetAxis("Horizontal") * baseSpeed;
		
		vertSpeed = Input.GetAxis("Vertical") * baseSpeed - focusModifierVert;
		horSpeed = Input.GetAxis("Horizontal") * baseSpeed - focusModifierHor;
		xMoveDirection = transform.rotation * new Vector3(0f, 0f, 1f).normalized * Time.deltaTime * vertSpeed;
		zMoveDirection = transform.rotation * new Vector3(1f, 0f, 0f).normalized * Time.deltaTime * horSpeed;
		moveVelocity = xMoveDirection + zMoveDirection + jumpVec;
		if (!_controller.isGrounded)
			moveVelocity += gravPull;
		if (_controller.isGrounded) moveVelocity.y -= 0.1f;
		_controller.Move(moveVelocity);
	}
	private void Start()
	{
		//Application.targetFrameRate = -1;
		_controller = GetComponent<CharacterController>();
		//Lock the camera on scene start.
		Cursor.lockState = CursorLockMode.Locked;

		baseSpeed = _baseSpeed;
	}
	void Update()
	{
		AnimatorSet();
		Gravity();
		Movement();
	}
}
