using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotationTest : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float startAngle;
    [SerializeField] float endAngle;
    [SerializeField] float angleStep;
    [SerializeField] int amount;
    [SerializeField] bool clockAngles;
    [SerializeField] bool clockRotation;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(clockAngles){
            angleStep = (endAngle - startAngle) / amount;
            angle = startAngle;
            transform.LookAt(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward);
            clockAngles = !clockAngles;
        }
        if(clockRotation){
            angle += angleStep;
            Vector3 bulDir = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
            transform.LookAt(bulDir);
            clockRotation = !clockRotation;
        }        
    }
}
