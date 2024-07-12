using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    MeshRenderer _meshRenderer;
	[SerializeField] float _timer = 0.1f;
	float timer;
    private float transparency;
	private float _transparency;
    // Start is called before the first frame update
    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
	}
	private void OnEnable()
	{
		timer = _timer;
		try
		{
			_transparency = _meshRenderer.material.GetFloat("_Transparency");
			transparency = _transparency;
		} catch	
		{
			print(_meshRenderer.material.name + _meshRenderer.material);
		}
	}

	// Update is called once per frame
	void Update()
    {
		Material[] materialToFade = _meshRenderer.materials;
		if (transparency > 0f)
		{
			foreach (Material mat in materialToFade)
			{
				try
				{
					mat.SetFloat("_Transparency", transparency);
				} catch
				{
					Debug.Log(mat.name, mat);
				}
			}
			transparency = Mathf.Lerp(_transparency, 0f, 1 - (timer/_timer));
			timer -= Time.deltaTime;
		}
		else
		{
			gameObject.SetActive(false);
			foreach (Material mat in materialToFade)
			{
				try
				{
					mat.SetFloat("_Transparency", 0.5f);
				}
				catch
				{
					Debug.Log(mat.name, mat);
				}
			}
		}/*
		if(timer > 0f) timer -= Time.deltaTime;
		else gameObject.SetActive(false);*/
    }
}
