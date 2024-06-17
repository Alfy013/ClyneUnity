using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class OnParry : MonoBehaviour
{
    
    [SerializeField] GameObject projectileToTurnInto;
    [SerializeField] int burstAmount;
    float angleStep;
    float angle;
    int projTypeIndex;
    public void SetIndex(int index)
	{
		projTypeIndex = index;
	}
    void OnTriggerEnter(Collider col){
        if (col.CompareTag("Shield"))
		{
            transform.rotation = Quaternion.identity;
            angleStep = 360 / (burstAmount - 1);

            for(int i = 0; i < burstAmount; i++){
                GameObject bul;
                bul = Instantiate(projectileToTurnInto, transform.position, transform.rotation);
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
                angle += angleStep;
            }
            if (ProjectilePools.ObjectPoolInstance != null)
			    ProjectilePools.ObjectPoolInstance.ReturnPooledObject(gameObject, projTypeIndex);
		    else print("destruction returned null");
		}
    }
}
