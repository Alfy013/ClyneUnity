using UnityEngine;

public class ScaleUP : MonoBehaviour
{
    [SerializeField] float _timeToMax;
    [SerializeField] Vector3 _scaleUpMax;
    float timeToMax = 0;
    Vector3 initialScale;
    void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timeToMax > timeToMax){
            transform.localScale = Vector3.Lerp(initialScale, _scaleUpMax, timeToMax/_timeToMax);
            timeToMax += Time.deltaTime;
        }
        
    }
}
