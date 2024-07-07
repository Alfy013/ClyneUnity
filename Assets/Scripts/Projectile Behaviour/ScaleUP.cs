using UnityEngine;

public class ScaleUP : MonoBehaviour
{
    [SerializeField] float[] _timeToMax;
    [SerializeField] Vector3[] _scaleUpTo;
    [SerializeField] bool startOnAwake;
    int index = 0;
    float timeToScale;
    Vector3 initialScale;
    void Awake(){
        if(startOnAwake) timeToScale = 0;
        else timeToScale = 99999;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timeToMax[index] > timeToScale){
            transform.localScale = Vector3.Lerp(initialScale, _scaleUpTo[index], timeToScale/_timeToMax[index]);
            timeToScale += Time.deltaTime;
        }
        
    }
    public void SetIndex(int i){
        index = i;
        initialScale = transform.localScale;
        timeToScale = 0;
    }
}
