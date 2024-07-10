using Cinemachine;
using System;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class MovementHandler : AbilityHandler.Ability
{
	//General movement and action variables
	[SerializeField] Animator _animator;
	[SerializeField] float _baseSpeed = 7f;
	[SerializeField] float _focusModifier = 2;
	[SerializeField] float _runModifier = 4;
	[SerializeField] float runTurnFraction = 0.5f;
	[SerializeField] ParticleSystem swordTrail;
	[SerializeField] ParticleSystem runParticles;
	[SerializeField] AfterImage aim;
	[SerializeField] GameObject playerChar;
	readonly float _gravity = -9.81f;

	CharacterController _controller;

	private Vector3 xMoveDirection;
	private Vector3 zMoveDirection;
	private Vector3 moveVelocity;
	private float baseSpeed;
	private float runModifierVert;
	private float runModifierHor;
	private float focusModifierVert;
	private float focusModifierHor;
	private float vertSpeed;
	private float horSpeed;
	private bool running;

	private Vector3 jumpVec;
	private Vector3 gravPull;
    internal override void AbilitySetup(){
		runModifierVert = _runModifier * Input.GetAxis("Vertical") * baseSpeed;
		runModifierHor = _runModifier * Input.GetAxis("Horizontal") * baseSpeed;
		swordTrail.Stop();
		if(runParticles.isStopped)
			runParticles.Play();
		running = true;

	}
	internal override void AbilityEffect(){

	}
	internal override void AbilityReset(){
		runModifierHor = runModifierVert = 0;
		swordTrail.Play();
		if(runParticles.isPlaying)
			runParticles.Stop();
		running = false;
	}
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
		beingUsed = (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0) && Input.GetAxisRaw("Run") > 0;

		if(running){
			Vector3 inputDir = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
			playerChar.transform.forward = Vector3.Slerp(playerChar.transform.forward, inputDir.normalized, Mathf.Pow(runTurnFraction, 100 * Time.deltaTime));
		} else 
			playerChar.transform.forward = Vector3.Slerp(playerChar.transform.forward, transform.forward, Mathf.Pow(runTurnFraction, 100 * Time.deltaTime));

		if (_controller.isGrounded)
			baseSpeed = _baseSpeed;
		if(Input.GetAxisRaw("Focus") > 0 && Mathf.Abs(runModifierHor) + Mathf.Abs(runModifierVert) == 0){
			focusModifierVert = _focusModifier * Input.GetAxis("Focus") * Input.GetAxis("Vertical") * baseSpeed;
			focusModifierHor = _focusModifier * Input.GetAxis("Focus") * Input.GetAxis("Horizontal") * baseSpeed;
		} else focusModifierVert = focusModifierHor = 0;
		
		vertSpeed = Input.GetAxis("Vertical") * baseSpeed + runModifierVert - focusModifierVert;
		horSpeed = Input.GetAxis("Horizontal") * baseSpeed + runModifierHor - focusModifierHor;
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
