using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] int maxHP = 250;
    [SerializeField] int overHealMax;
    [SerializeField] float overHealDrain;
    [SerializeField] Animator playeranim;
    [SerializeField] ParticleSystem hit;
    [SerializeField] GameObject deathCam;
    UIBarInterpolator UIBPNormal;
    [SerializeField] UIBarInterpolator UIBPOverheal;
    [SerializeField] TMP_Text hits;
    [HideInInspector]
    public float HP = 250;
    float overHeal;
    float hitCooldown;


    
    MovementHandler moveHandler;
    AbilityHandler abilityHandler;
    private int hitsTaken = 0;

    private void Awake(){
        moveHandler = GetComponent<MovementHandler>();
        abilityHandler = GetComponent<AbilityHandler>();
        UIBPNormal = GetComponent<UIBarInterpolator>();
        UIBPNormal._virtualMaxValue = maxHP;
        UIBPNormal._actualMaxValue = maxHP + overHealMax;
        UIBPOverheal._virtualMaxValue = overHealMax;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U)) Time.timeScale = 0.2f;
        if (HP <= 0.5)
        {
            deathCam.SetActive(true);
            EnemyStagger.StaggerInstance.staggered = true;
            moveHandler.enabled = false;
            abilityHandler.enabled = false;
            playeranim.SetBool("Knocked", true);
        }
        if(Input.GetKeyDown(KeyCode.L))
            HP -= 50;
        HP = Mathf.Clamp(HP, 0, maxHP + overHealMax);
        overHeal = Mathf.Clamp(overHeal, 0, overHealMax);
        overHeal = HP - maxHP;
        if(overHeal > 0f && HP - (Time.deltaTime * overHealDrain) > maxHP) HP -= Time.deltaTime * overHealDrain;
        UIBPOverheal.value = overHeal;
        UIBPNormal.value = HP;
        if(hitCooldown > 0f) hitCooldown -= Time.deltaTime;
        hits.text = "Hits taken: " + hitsTaken;

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitsTaken = 0;
            HP = maxHP;
            abilityHandler.stamina = 100f;
        }
    }
	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("EnemySwordHitbox") && hitCooldown <= 0f)
        {
            HP -= 50;
            ShakeHandler.Instance.ShakeCamera(10, 0.2f);
            hitsTaken++;
            playeranim.ResetTrigger("Flinch");
            playeranim.SetTrigger("Flinch");
            hitCooldown = 0.5f; //WARNING, ABRUPTLY EXITING ANIMATIONS CASUSES A RUCKUS, FIX LATER
        }
	}
    public void TakeHit(int damage, float stunTime = 0)
    {
        if(enabled){
            HP -= damage;
            hitsTaken++;
            //moveSystem.stamRegen = 0f;
            hit.Play();
        }

    }
    public void Unknocked(){
        HP = maxHP;
        moveHandler.enabled = true;
        abilityHandler.enabled = true;
        abilityHandler.stamina = 100f;
        playeranim.SetBool("Knocked", false);
        deathCam.SetActive(false);
    }
}
