using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    [Serializable]
    private struct Action{
        public Transform targetPosition;
        public float timeToStart;
        public float timeToArrive;
        public bool verticality;
    }
    [Serializable]
    private struct ActionList{
        public List<Action> actions;
    }
    [SerializeField] List<ActionList> actionLists;
    
    private int AL_Index = 0;
    private int A_Index = 0;
    private float timeToArrive;

    private float timeToStart;
    private float clampedTime;
    private bool calculatedPositions;
    Vector3 selfPos;
    Vector3 targetPos;

    [SerializeField] TMP_Text ActionIndex;
    [SerializeField] TMP_Text ActionListIndex;
    [SerializeField] TMP_Text TimeToStart;
    [SerializeField] TMP_Text TimeToArrive;

    // Start is called before the first frame update
    void Start()
    {
        A_Index = 0;
        AL_Index = 0;
        timeToStart = actionLists[AL_Index].actions[A_Index].timeToStart;
        timeToArrive = actionLists[AL_Index].actions[A_Index].timeToArrive;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToArrive > 0f){
            if(timeToStart <= 0f){
                if(!calculatedPositions){
                    selfPos = new(transform.position.x, actionLists[AL_Index].actions[A_Index].verticality? transform.position.y : 0f, transform.position.z);
                    targetPos = new(actionLists[AL_Index].actions[A_Index].targetPosition.position.x, actionLists[AL_Index].actions[A_Index].verticality? actionLists[AL_Index].actions[A_Index].targetPosition.position.y : 0f, actionLists[AL_Index].actions[A_Index].targetPosition.position.z);
                    calculatedPositions = true; 
                }
                timeToArrive -= Time.deltaTime;
                clampedTime = 1 - (timeToArrive / actionLists[AL_Index].actions[A_Index].timeToArrive);
                transform.position = Vector3.LerpUnclamped(selfPos, targetPos, clampedTime);
            } else timeToStart -= Time.deltaTime;
        } else {
            calculatedPositions = false;
            if(A_Index < actionLists[AL_Index].actions.Count - 1)
                A_Index++;
            else {
                if(AL_Index + 1 <= actionLists.Count - 1){
                    AL_Index++;
                    A_Index = 0;
                } else enabled = false;
                
            }
            timeToStart = actionLists[AL_Index].actions[A_Index].timeToStart;
            timeToArrive = actionLists[AL_Index].actions[A_Index].timeToArrive;
        }
        ActionIndex.text = "AIndex " + Convert.ToString(A_Index);
        ActionListIndex.text = "AIIndex " + Convert.ToString(AL_Index);
        TimeToStart.text = "TimeToStart " + Convert.ToString(timeToStart);
        TimeToArrive.text = "TimeToArrive " + Convert.ToString(timeToArrive);
    }
}