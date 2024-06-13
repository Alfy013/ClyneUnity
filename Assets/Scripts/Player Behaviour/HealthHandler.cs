using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] int maxHP = 250;
    [SerializeField] Animator playeranim;
    [SerializeField] ParticleSystem hit;
    [SerializeField] GameObject deathCam;
    [SerializeField] UIBarInterpolator UIBP;
    private bool defaulted = true;
    private float HP = 250;


    
    PlayerHandler moveSystem;
    private int hitsTaken = 0;

    private void Start(){
        moveSystem = FindObjectOfType<PlayerHandler>();
        UIBP._maxValue = maxHP;
    }

    private void Update()
    {
        UIBP.value = HP;
        HP = Mathf.Clamp(HP, 0, maxHP);
        if (HP <= 0)
        {
            if(defaulted){
                defaulted = false;
                moveSystem.StopAction();
                moveSystem.StartAction(0, PlayerHandler.PlayerState.Knocked, true);
                deathCam.SetActive(true);
                EnemyStagger.StaggerInstance.staggered = true;
            }
            playeranim.SetBool("Knocked", true);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitsTaken = 0;
            HP = maxHP;
            moveSystem.stamina = 100f;
        }
    }
	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("EnemySwordHitbox"))
        {
            HP -= 50;
            hitsTaken++;
        }
	}
    public void TakeHit(int damage, float stunTime = 0)
    {
        HP -= damage;
        hitsTaken++;
        //moveSystem.stamRegen = 0f;
        hit.Play();
    }
    public void Unknocked(){
        HP = maxHP;
        moveSystem.StopAction();
        playeranim.SetBool("Knocked", false);
        deathCam.SetActive(false);
        defaulted = true;
        moveSystem.stamina = 100f;
    }
}
