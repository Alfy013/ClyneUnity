using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    private Transform target;
	[SerializeField] private float _lockOnAfter = 0f;
	[SerializeField] private bool verticality;
	[SerializeField] private float _lockedOnFor;
	private Vector3 targetDirection;
	ProjectileHandler handler;
	private float lockOnAfter;
	private float lockedOnFor;

    private void Start()
	{
		target = FindObjectOfType<PlayerHandler>().transform;
        handler = GetComponent<ProjectileHandler>();
	}

	private void OnEnable()
	{
		lockedOnFor = _lockedOnFor;
		lockOnAfter = _lockOnAfter;
    }

    private void Update()
	{
		targetDirection = target.position - transform.position;
		if (lockOnAfter > 0f)
		{
			lockOnAfter -= Time.deltaTime;
			transform.LookAt(target.position);
		}
		else if (lockedOnFor >= 0f)
		{
			lockedOnFor -= Time.deltaTime;
			if (!verticality) targetDirection.y = 0f;
			handler.SetMoveDirection(targetDirection.normalized);
		}

	}
}

