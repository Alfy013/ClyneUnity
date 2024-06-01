using System;
using Unity.VisualScripting;
using UnityEngine;

public class Slashing : MonoBehaviour
{
    [Serializable] private enum AttackType{
        Slashes, Stabs
    }

    [SerializeField] AttackType type;
    [SerializeField] GameObject[] slashProjectiles;
    [SerializeField] Transform[] slashTransforms;
    [SerializeField] GameObject[] stabProjectiles;
    [SerializeField] Transform[] stabTransforms;
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
    public int slashingIndex = 0;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V)){
            if(type == AttackType.Slashes) type = AttackType.Stabs;
            else if(type == AttackType.Stabs) type = AttackType.Slashes;
        }

        if(type == AttackType.Slashes) Slashes();
        else if(type == AttackType.Stabs) Stabs();
    }
    private void Stabs(){
        animator.SetInteger("SlashingIndex", 0);
        if(Input.GetAxisRaw("Slash") == 1 && moveSystem.StartAction(0, PlayerHandler.PlayerState.Stab)){
            animator.SetBool("Stab", true);
        } else{
            animator.SetBool("Stab", false);
            moveSystem.StopAction();
        }
    }
    private void Slashes(){
        animator.SetBool("Stab", false);
        animator.SetInteger("SlashingIndex", slashingIndex);
        if(Math.Ceiling(Input.GetAxisRaw("Slash")) == 1 && slashCooldown <= 0f){
            slashCooldown = _slashCooldown;
            switch(slashingIndex){
                case 0:
                    if(moveSystem.StartAction(3, PlayerHandler.PlayerState.Slash))
                        slashingIndex = 1;
                    else slashingIndex = 0;
                    break;
                case 1:
                    if(moveSystem.StartAction(3, PlayerHandler.PlayerState.Slash))
                        slashingIndex = 2;
                    else slashingIndex = 0;
                    break;
                case 2:
                    if(moveSystem.StartAction(3, PlayerHandler.PlayerState.Slash))
                        slashingIndex = 3;
                    else slashingIndex = 0;
                    break;
                case 3:
                    if(moveSystem.StartAction(6, PlayerHandler.PlayerState.Slash))
                        slashingIndex = 4;
                    else slashingIndex = 0;
                    break;
                case 4:
                    if(moveSystem.StartAction(15, PlayerHandler.PlayerState.Slash)){
                        slashingIndex = 5;
                    }
                    break;
            }
        }
        if(slashingIndex == 5 && Math.Ceiling(Input.GetAxis("JumpToSword")) == 1 && latestProjectile.GetComponent<ThrownSword>() != null){
            if(!isDashing) {
                sword = FindObjectOfType<ThrownSword>().gameObject.transform;
                jumpDuration = _jumpDuration;
                startingPos = transform.position;
                isDashing = true;
                afterImage.activate = true;
                moveSystem.StopAction();
                moveSystem.StartAction(0, PlayerHandler.PlayerState.Charge);
            }
            transform.position = Vector3.Lerp(startingPos, new Vector3(sword.position.x, transform.position.y, sword.position.z), 1 - (jumpDuration / _jumpDuration));
        }
        if(jumpDuration > 0f) jumpDuration -= Time.deltaTime;
        if(slashCooldown > 0f) slashCooldown -= Time.deltaTime;
        if(Math.Ceiling(Input.GetAxis("Slash")) == 0 && moveSystem.playerState == PlayerHandler.PlayerState.Slash) {
            slashingIndex = 0;
            moveSystem.StopAction();
        }
    }
    public void PlaySlash()
    {
        if(slashingIndex == 0) return;
		latestProjectile = Instantiate(slashProjectiles[slashingIndex - 1], slashTransforms[slashingIndex - 1].position, slashTransforms[slashingIndex - 1].rotation);
		Destroy(latestProjectile, 3);
	}
    public void PlayStab(){
        moveSystem.StartAction(1, PlayerHandler.PlayerState.Stab);
        int roll = UnityEngine.Random.Range(0, stabProjectiles.Length - 1);
        latestProjectile = Instantiate(stabProjectiles[roll], stabTransforms[roll].transform.position, stabTransforms[roll].transform.rotation);
        Destroy(latestProjectile, 1);
    }
    public void ReturnSword(){
        slashCooldown = 1f;
        slashingIndex = 0;
        moveSystem.StopAction();
        isDashing = false;
        afterImage.activate = false;
    }
}
