using System;
using TMPro;
using UnityEngine;

public class Blocking : MonoBehaviour
{
	[SerializeField] float slowDownValue;
	[SerializeField] float timeToNormalizeCT;
	[SerializeField] float timeToSlowDownCT;
	[SerializeField] GameObject shieldHitBox;
    [SerializeField] TMP_Text UICooldownText;
	[SerializeField] Animator animator;
	public float slowdownCooldown;
	//[SerializeField] GameObject blocker;
	PlayerHandler moveSystem;
	bool reflected = false;
	float timeToNormalize;
	float timeToSlowDown;
	float shieldCooldown = 0f;
	private void Awake()
	{
		moveSystem = FindObjectOfType<PlayerHandler>();
	}

	void Update()
	{
		if (slowdownCooldown > 0f) slowdownCooldown -= Time.unscaledDeltaTime;
		if (timeToSlowDown > 0f)
		{
			Time.timeScale = Mathf.Lerp(slowDownValue, 1f, timeToSlowDown / timeToSlowDownCT);
			timeToSlowDown -= Time.unscaledDeltaTime;
		}
		if (reflected && timeToSlowDown <= 0f)
		{
			reflected = false;
			timeToNormalize = timeToNormalizeCT;
		}
		if (timeToNormalize > 0f)
		{
			Time.timeScale = Mathf.Lerp(1f, slowDownValue, timeToNormalize / timeToNormalizeCT);
			timeToNormalize -= Time.unscaledDeltaTime;
		}
		if (shieldCooldown > 0f)
		{

			shieldCooldown -= Time.deltaTime;
			UICooldownText.text = "Shield Cooldown: " + Convert.ToInt32(shieldCooldown);
			UICooldownText.color = Color.red + Color.blue;
		}
		else
		{
			UICooldownText.text = "Shield Cooldown: READY!";
			UICooldownText.color = Color.yellow;
		}

		if (Math.Ceiling(Input.GetAxisRaw("Block")) == 1 && moveSystem.stamina >= 1 && shieldCooldown <= 0f && moveSystem.StartAction(30, PlayerHandler.PlayerState.Parry))
		{
			animator.SetBool("Blocking", true);
			shieldCooldown = 1f;
			FindObjectOfType<AudioManager>().PlaySound("reflect");
		}
		else animator.SetBool("Blocking", false);

	}
	public void StartBlock()
	{
		shieldHitBox.SetActive(true);
	}
	public void StopBlock()
	{
		shieldHitBox.SetActive(false);
		moveSystem.StopAction();
	}
	public void ReflectSlowDown()
	{
		slowdownCooldown = 1f;
		timeToSlowDown = timeToSlowDownCT;
		reflected = true;
	}
}
