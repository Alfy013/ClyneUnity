using System;
using Unity.VisualScripting;
using UnityEngine;

public class Slashing : MonoBehaviour
{
    [SerializeField] GameObject[] projectileToUse;
    [SerializeField] Transform[] transformToUse;
    [SerializeField] PlayerHandler moveSystem; 
    [SerializeField] Animator animator;
    [SerializeField] float _slashCooldown;
    [SerializeField] float _jumpDuration;
    [SerializeField] AfterImage afterImage;
    Transform sword;
    GameObject latestProjectile;
    float jumpDuration;
    Vector3 startingPos;
    private bool isDashing;
    float slashCooldown;
    int slashingIndex = 0;
    
    void Update()
    {
        
		animator.SetInteger("SlashingIndex", slashingIndex);
        if(Math.Ceiling(Input.GetAxisRaw("Slash")) == 1 && slashCooldown <= 0f){
            moveSystem.actionPlaying = true;
            slashCooldown = _slashCooldown;
            switch(slashingIndex){
                case 0:
                    if(moveSystem.StartAction(3))
                        slashingIndex = 1;
                    else slashingIndex = 0;
                    break;
                case 1:
                    if(moveSystem.StartAction(3))
                        slashingIndex = 2;
                    else slashingIndex = 0;
                    break;
                case 2:
                    if(moveSystem.StartAction(3))
                        slashingIndex = 3;
                    else slashingIndex = 0;
                    break;
                case 3:
                    if(moveSystem.StartAction(6))
                        slashingIndex = 4;
                    else slashingIndex = 0;
                    break;
                case 4:
                    if(moveSystem.StartAction(15)){
                        slashingIndex = 5;
                    }
                    break;
            }
        }
        if(slashingIndex == 5 && Math.Ceiling(Input.GetAxis("JumpToSword")) == 1){
            if(!isDashing) {
                sword = FindObjectOfType<ThrownSword>().gameObject.transform;
                jumpDuration = _jumpDuration;
                startingPos = transform.position;
                isDashing = true;
                afterImage.activate = true;
            }
            transform.position = Vector3.Lerp(startingPos, new Vector3(sword.position.x, transform.position.y, sword.position.z), 1 - (jumpDuration / _jumpDuration));
        }
        if(jumpDuration > 0f) jumpDuration -= Time.deltaTime;
        if(slashCooldown > 0f) slashCooldown -= Time.deltaTime;
        if(Math.Ceiling(Input.GetAxis("Slash")) == 0 && slashingIndex != 5) {
            moveSystem.actionPlaying = false;
            slashingIndex = 0;
        }
    }
    public void PlaySlash()
    {
        if(slashingIndex == 0) return;
		latestProjectile = Instantiate(projectileToUse[slashingIndex - 1], transformToUse[slashingIndex - 1].position, transformToUse[slashingIndex - 1].rotation);
		Destroy(latestProjectile, 3);
	}
    public void ReturnSword(){
        slashingIndex = 0;
        moveSystem.actionPlaying = false;
        isDashing = false;
        afterImage.activate = false;
    }
}
