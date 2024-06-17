using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePools : MonoBehaviour
{
	public static ProjectilePools ObjectPoolInstance;

	[Serializable]
	private struct ProjectilesToPool
	{
		public string projName;
		public GameObject projPrefab;
		public int projAmount;
		[HideInInspector]
		public List<GameObject> pooledBullets;
	}

	[SerializeField]
	private List<ProjectilesToPool> projectilesToPool;

	private void Awake()//Pre-generate every gameobject on the list before the game starts
	{
		if (ObjectPoolInstance == null) ObjectPoolInstance = this;
		for (int i = 0; i < projectilesToPool.Count; i++)
		{
			for (int j = 0; j < projectilesToPool[i].projAmount; j++)
			{
				GameObject obj = Instantiate(projectilesToPool[i].projPrefab);
				obj.SetActive(false);
				projectilesToPool[i].pooledBullets.Add(obj);
				obj.GetComponent<ProjectileHandler>().SetIndex(i);
				if(TryGetComponent<OnParry>(out OnParry onParry)){
					onParry.SetIndex(i);
				}
			}
		}
	}
	public GameObject GetPooledObject(GameObject objectToPool) //method made to access any gameobject from said lists
	{
		int poolIndex = -1;
		for(int i = 0; i < projectilesToPool.Count; i++)
		{
			if(objectToPool == projectilesToPool[i].projPrefab)
			poolIndex = i;
		}

		if (poolIndex == -1)
		{
			Debug.Log("Invalid Projectile Index"); //this only happens if something truly fucks up, if you get this error, take a 10 min break
			return null;
		}
		if (projectilesToPool[poolIndex].pooledBullets.Count != 0) //check if the pool has any projectiles
		{
			GameObject sentProj = projectilesToPool[poolIndex].pooledBullets[projectilesToPool[poolIndex].pooledBullets.Count - 1]; //set the projectile to send
			projectilesToPool[poolIndex].pooledBullets.RemoveAt(projectilesToPool[poolIndex].pooledBullets.Count - 1); //remove the projectile from the pooled objects array
			return sentProj; //return it
		}
		Debug.Log("Object pool " + projectilesToPool[poolIndex].projName + " was exhausted."); //this happens if you have 0 available gameobjects in the pool
		return null;
	}
	public void ReturnPooledObject(GameObject objectToReturn, int projType) //return a requested gameobject into the pool that it was withdrawn from
	{
		objectToReturn.SetActive(false);
		projectilesToPool[projType].pooledBullets.Add(objectToReturn);
	}
}
