using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomSpawn : MonoBehaviour
{
    [SerializeField] float circleRadius;
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] int pointsToGenerate;
    // Start is called before the first frame update
    void OnEnable()
    {
        for(int i = 0; i < pointsToGenerate; i++){
            
            GameObject bul;
			bul = ProjectilePools.ObjectPoolInstance.GetPooledObject(objectToSpawn);
            Vector3 randomPoint = transform.position;
            randomPoint += Random.insideUnitSphere * circleRadius;
            randomPoint.y = transform.position.y;
            bul.transform.position = randomPoint;
            bul.SetActive(true);
        }
    }
}
