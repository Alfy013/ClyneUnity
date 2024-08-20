using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
	[SerializeField] GameObject afterImageGhost;
	[SerializeField] Material afterImageMAT;
	[SerializeField] GameObject[] _objectsToBeAfterImaged;
	[SerializeField] int _afterImageCount;
	GameObject[] afterImageSMObjects;
	[SerializeField] bool useSkinnedMesh = true;
	[SerializeField] int frequency = 0;
	public bool activate;
	private MeshRenderer afterImageMeshRenderer;
	private Mesh meshToUse;
	private int currentAfterImageObject;
	int frequencyDecrement;
	AfterImageObject[] afterImageObjects;
	SkinnedMeshRenderer characterMesh;
	class AfterImageObject
	{
		internal int objectID;
		internal GameObject afterImageGO;
	}
	private void Start()
	{
		currentAfterImageObject = 0;
		afterImageObjects = new AfterImageObject[_afterImageCount * _objectsToBeAfterImaged.Length];
		afterImageSMObjects = new GameObject[_afterImageCount];
		if (useSkinnedMesh)
			characterMesh = GetComponent<SkinnedMeshRenderer>();
		for(int j = 0; j < _objectsToBeAfterImaged.Length; j++) {
			for(int i = 0; i < _afterImageCount; i++)
			{
				GameObject afterImageObject = Instantiate(afterImageGhost); //create afterimage
				afterImageMeshRenderer = afterImageObject.GetComponent<MeshRenderer>(); //get the meshrenderer for materials
				meshToUse = _objectsToBeAfterImaged[j].GetComponent<MeshFilter>().mesh; //grab the mesh
				afterImageObject.GetComponent<MeshFilter>().mesh = meshToUse; //set the mesh
				Material[] AfterImageMATs = new Material[meshToUse.subMeshCount];

				for (int k = 0; k < AfterImageMATs.Length; k++)
					AfterImageMATs[k] = afterImageMAT; //set the materials

				afterImageMeshRenderer.materials = AfterImageMATs; //apply the materials
				afterImageObjects[currentAfterImageObject] = new AfterImageObject();
				afterImageObjects[currentAfterImageObject].afterImageGO = afterImageObject;
				afterImageObjects[currentAfterImageObject].objectID = j;
				currentAfterImageObject++;
			}
		}
		for(int i = 0; i < _afterImageCount; i++)
		{
			GameObject afterImageSMObject = Instantiate(afterImageGhost);
			afterImageSMObject.SetActive(false);
			if(useSkinnedMesh)
				characterMesh.BakeMesh(afterImageSMObject.GetComponent<MeshFilter>().mesh);
			Material[] AfterImageCharMATs = new Material[afterImageSMObject.GetComponent<MeshFilter>().mesh.subMeshCount];

			for (int j = 0; j < AfterImageCharMATs.Length; j++)
				AfterImageCharMATs[j] = afterImageMAT;

			afterImageSMObject.GetComponent<MeshRenderer>().materials = AfterImageCharMATs;
			afterImageSMObjects[i] = afterImageSMObject;
		}
	}
	// Update is called once per frame
	void FixedUpdate()
    {
		if (activate)
		{
			if(frequencyDecrement == 0){
				for (int j = 0; j < _objectsToBeAfterImaged.Length; j++)
				{
					for (int i = 0; i < afterImageObjects.Length; i++)
					{
						if (afterImageObjects[i] == null) print("The afterimage object is missing.");
						if (afterImageObjects[i].objectID == j && !afterImageObjects[i].afterImageGO.activeSelf)
						{
							afterImageObjects[i].afterImageGO.transform.SetPositionAndRotation(_objectsToBeAfterImaged[j].transform.position, _objectsToBeAfterImaged[j].transform.rotation);
							afterImageObjects[i].afterImageGO.SetActive(true);
							break;
						}
					}
				}
				for (int i = 0; i < afterImageSMObjects.Length; i++)
				{
					if (!afterImageSMObjects[i].activeInHierarchy && afterImageSMObjects[i] != null)
					{
						if(useSkinnedMesh)
							characterMesh.BakeMesh(afterImageSMObjects[i].GetComponent<MeshFilter>().mesh);
						afterImageSMObjects[i].transform.SetPositionAndRotation(transform.position, transform.rotation);
						afterImageSMObjects[i].SetActive(true);
						break;
					}
				}
				frequencyDecrement = frequency;
			} else frequencyDecrement--;

		}	
	}
}
