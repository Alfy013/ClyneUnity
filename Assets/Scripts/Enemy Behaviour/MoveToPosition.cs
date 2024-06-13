using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{

    [Serializable]
    private struct Action{
        public GameObject objectToMove; 
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
    
    private int AL_Index = -1;
    private int A_Index = 0;
    private int A_IndexMax = 0;
    private float timeToArrive;

    private float timeToStart;
    private float clampedTime;
    private bool calculatedPositions;
    Vector3 selfPos;
    Vector3 targetPos;
    float timer = -1f;

    // Start is called before the first frame update
    void Start()
    {
        /*A_Index = 0;
        AL_Index = 0;
        timeToStart = actionLists[AL_Index].actions[A_Index].timeToStart;
        timeToArrive = actionLists[AL_Index].actions[A_Index].timeToArrive;*/
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        /*if(EnemyStagger.StaggerInstance.HP > 0f && timer > -1f && timer <= 0f)
            Move();*/
        if(AL_Index != -1)
            Move();

    }

    void Move(){
        if(timeToArrive > 0f){
            if(timeToStart <= 0f){
                if(!calculatedPositions){
                    Vector3 currentPosition = actionLists[AL_Index].actions[A_Index].objectToMove.transform.position;
                    Vector3 targetPosition = actionLists[AL_Index].actions[A_Index].targetPosition.position;
                    selfPos = currentPosition;

                    targetPos.x = targetPosition.x;
                    targetPos.y = actionLists[AL_Index].actions[A_Index].verticality? targetPosition.y : currentPosition.y;
                    targetPos.z = targetPosition.z;
                    calculatedPositions = true; 
                    timeToStart = actionLists[AL_Index].actions[A_Index].timeToStart;
                    timeToArrive = actionLists[AL_Index].actions[A_Index].timeToArrive;
                }
                timeToArrive -= Time.deltaTime;
                clampedTime = 1 - (timeToArrive / actionLists[AL_Index].actions[A_Index].timeToArrive);
                actionLists[AL_Index].actions[A_Index].objectToMove.transform.position = Vector3.LerpUnclamped(selfPos, targetPos, clampedTime);
            } else timeToStart -= Time.deltaTime;
        } else {
            calculatedPositions = false;
            if(A_Index < A_IndexMax)
                A_Index++;
            else{
                AL_Index = -1;
                return;
            }
        }
    }
    public void CallMovement(int phaseNumber, int lastActionIndex = 0, int firstActionIndex = 0){
        calculatedPositions = false;
        AL_Index = phaseNumber;
        A_Index = firstActionIndex;
        A_IndexMax = lastActionIndex;
        timeToStart = actionLists[AL_Index].actions[A_Index].timeToStart;
        timeToArrive = actionLists[AL_Index].actions[A_Index].timeToArrive;
    }
}