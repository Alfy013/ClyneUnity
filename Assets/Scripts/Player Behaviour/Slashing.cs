using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Slashing : MonoBehaviour
{
    [SerializeField] Transform swordpos;
    [SerializeField] PlayerHandler moveSystem; 
    [SerializeField] Animator animator;
    [SerializeField] GameObject swordProjectile;
    float slashTime;
    bool canSlash = true;
    bool isSlashing = false;

    void Update()
    {
		animator.SetBool("Slashing", isSlashing);
		if (Math.Ceiling(Input.GetAxisRaw("Slash")) == 1 && canSlash && moveSystem.stamina >= 10f && HealthHandler.deathStunTime <= 0f)
        {
            moveSystem.StartAction(10);
            isSlashing = true;
            canSlash = false;
            slashTime = 0.83f;
            
        }
        if(slashTime > 0f)
        {
            slashTime -= Time.deltaTime;
        }
        if(isSlashing && slashTime <= 0f)
        {
            isSlashing = false;
            canSlash = true;
        }
    }
    public void PlaySlash()
    {
		GameObject proj = Instantiate(swordProjectile, swordpos.position, swordpos.rotation);
		Destroy(proj, 3);
	}
}
