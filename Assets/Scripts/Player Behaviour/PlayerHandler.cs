using Cinemachine;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour
{
	//General movement and action variables
	public enum PlayerState { None, Knocked, Slash, Stab, Charge, Parry, Engross, Monopolize }
	public PlayerState playerState;
	[SerializeField] Animator _animator;
	[SerializeField] float _baseSpeed = 7f;
	[SerializeField] float _focusModifier = 2;
	[SerializeField] float rotationSpeed;
	[SerializeField] AfterImage aim;
	[SerializeField] GameObject playerAsset;
	readonly float _gravity = -9.81f;

	CharacterController _controller;
	Transform target;
	private Quaternion rotation;
	private Vector3 direction;
	private Vector3 xMoveDirection;
	private Vector3 zMoveDirection;
	private Vector3 moveVelocity;
	private bool locked = true;
	private float baseSpeed;
	private float focusModifierVert;
	private float focusModifierHor;
	private float vertSpeed;
	private float horSpeed;
	private bool resetRotation;

	private Vector3 jumpVec;
	private Vector3 gravPull;

	[HideInInspector] public float stamina = 100f;
	[HideInInspector] public float stamRegen;
	[HideInInspector] public bool canMove = true;

	//UI element declarations
	[SerializeField] TMP_Text state;
	[SerializeField] TMP_Text UIStaminaText;
	[SerializeField] Slider UIStaminaSlider;

	[SerializeField] GameObject lockedCamera;
	[SerializeField] GameObject unlockedCamera;

	
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
		if (Input.GetKeyDown(KeyCode.Z)) locked = !locked;
		if (!locked)
		{
			transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f));
		}
		else if(resetRotation)
		{
			lockedCamera.SetActive(true);
			unlockedCamera.SetActive(false);
			resetRotation = false;
			transform.rotation = rotation;
			playerAsset.transform.localRotation = Quaternion.Euler(Vector3.zero);
			
		}
		if (!locked && !resetRotation)
		{
			unlockedCamera.transform.rotation = lockedCamera.transform.rotation;
			resetRotation = true;
			unlockedCamera.SetActive(true);
			lockedCamera.SetActive(false);
		}
		if (locked)
		{
			direction = (target.position - transform.position).normalized;
			rotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed);
		}

		

		if (!canMove) baseSpeed = 0;

		if (_controller.isGrounded && canMove)
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
		_controller = GetComponent<CharacterController>();
		unlockedCamera.transform.rotation = lockedCamera.transform.rotation;
		target = FindObjectOfType<EnemyStagger>().transform;
		UIStaminaText.text = "Stamina value: " + Mathf.CeilToInt(stamina);
		//Lock the camera on scene start.
		Cursor.lockState = CursorLockMode.Locked;
		stamRegen = 1f;
		stamina = 100f;
		UIStaminaSlider.value = stamina / 100f;
		baseSpeed = _baseSpeed;
	}
	void Update()
	{
		state.text = "State: " + Convert.ToString(playerState);
		stamina = Mathf.Clamp(stamina, 0, 100);
		if (playerState == PlayerState.None && stamina < 100) stamRegen += Time.deltaTime;
		if (playerState == PlayerState.None && stamina < 100 && stamRegen >= 3f) stamina += Time.deltaTime * stamRegen * 7.5f;

		AnimatorSet();
		Gravity();
		if(canMove) Movement();
		
		UIStaminaText.text = "Stamina value: " + Mathf.CeilToInt(stamina);
		UIStaminaSlider.value = stamina / 100f;
	}

	public bool StartAction(int stamCost, PlayerState state, bool stopMove = false)
	{
		if(stamina > 1 && (playerState == state || playerState == PlayerState.None)){
			canMove = !stopMove;
			playerState = state;
			stamRegen = 0f;
			stamina -= stamCost;
			return true;
		}
		return false;
	}
	public void StopAction()
	{
		playerState = PlayerState.None;
		canMove = true;
	}
}
