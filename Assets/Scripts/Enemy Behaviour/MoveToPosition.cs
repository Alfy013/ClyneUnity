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
        if(EnemyStagger.StaggerInstance.staggerTimer > 0f)
            Move();
    }

    void Move(){
        if(timeToArrive > 0f){
            if(timeToStart <= 0f){
                if(!calculatedPositions){
                    selfPos = new(transform.position.x, transform.position.y, transform.position.z);
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
            else A_Index = 0;
            timeToStart = actionLists[AL_Index].actions[A_Index].timeToStart;
            timeToArrive = actionLists[AL_Index].actions[A_Index].timeToArrive;
        }
    }
}