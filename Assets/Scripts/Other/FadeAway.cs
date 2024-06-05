using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    MeshRenderer _meshRenderer;
	float timer = 0.3f;
    private float transparency;
    // Start is called before the first frame update
    void Awake()
    {
        //_meshRenderer = GetComponent<MeshRenderer>();
	}
	private void OnEnable()
	{
		timer = 0.3f;
		/*
		try
		{
			transparency = _meshRenderer.material.GetFloat("_Transparency");
		} catch	
		{
			print(_meshRenderer.material.name + _meshRenderer.material);
		}*/
	}

	// Update is called once per frame
	void Update()
    {
		/*
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
			transparency -= Time.deltaTime;
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
		}*/
		if(timer > 0f) timer -= Time.deltaTime;
		else gameObject.SetActive(false);
    }
}
