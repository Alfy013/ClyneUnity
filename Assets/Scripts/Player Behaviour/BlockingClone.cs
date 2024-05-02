using UnityEngine;

public class BlockingClone : MonoBehaviour
{
	Rigidbody rb;
	[SerializeField] Material mat;
	float countdown;
	float lifeTime;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void OnEnable()
	{
		rb.velocity = transform.forward * -45f;
		countdown = 0.1f;
		lifeTime = 3f;
		mat.SetFloat("_FlickerIntensity", 0f);
		mat.SetFloat("_AlphaAmount", 1f);

	}

	void Update()
    {
		if (countdown > 0f) countdown -= Time.deltaTime;
		else rb.velocity = Vector3.zero;

		if(lifeTime > 0f) lifeTime -= Time.deltaTime;
		else gameObject.SetActive(false);
		if (lifeTime < 1f)
		{
			mat.SetFloat("_AlphaAmount", 0f);
			mat.SetFloat("_FlickerIntensity", 30f);
		}
	}
}
