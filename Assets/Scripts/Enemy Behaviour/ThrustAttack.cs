using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustAttack : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] GameObject thrustHitBox;
    [SerializeField] private float _cooldown;
    [SerializeField] Transform player;
    [SerializeField] private float speed;
    [SerializeField] private RotateToPlayer rt;
	[SerializeField] AfterImage aim;
    private Vector3 targetPoint;
    private Vector3 nonVerticalPosition;
    private bool dashForward;
    private float cooldown;

	private void OnEnable()
	{
        cooldown = 0f;
        dashForward = false;
	}
	private void OnDisable()
	{
		cooldown = 0f;
		dashForward = false;
		animator.SetBool("Thrust", false);
	}

	// Update is called once per frame
	void Update()
    {
        if(cooldown > 0f)
        {
			cooldown -= Time.deltaTime;
        }
        else
        {
			animator.SetBool("Thrust", true);
		}
		nonVerticalPosition = new Vector3(transform.position.x, 0f, transform.position.z);
		if (dashForward)
		{
			rb.MovePosition(transform.position + (speed * Time.deltaTime * (targetPoint - nonVerticalPosition)));
		}
	}

	public void StartThrust()
    {
        dashForward = true;
		rt.enabled = false;
	}
	public void StopThrust()
    {
        dashForward = false;
		//aim.activate = false;
	}
	public void EndThrust()
    {
		animator.SetBool("Thrust", false);
		cooldown = _cooldown;
        rt.enabled = true;
	}
    public void HitBoxOff()
    {
		thrustHitBox.SetActive(false);
	}
    public void LockOn()
    {
		targetPoint = new Vector3(player.position.x, 0f, player.position.z);
		thrustHitBox.SetActive(true);
		//aim.activate = true;
	}
}
