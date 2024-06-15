using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] int maxHP = 250;
    [SerializeField] Animator playeranim;
    [SerializeField] ParticleSystem hit;
    [SerializeField] GameObject deathCam;
    UIBarInterpolator UIBP;
    [SerializeField] TMP_Text hits;
    [HideInInspector]
    public float HP = 250;


    
    MovementHandler moveHandler;
    AbilityHandler abilityHandler;
    private int hitsTaken = 0;

    private void Awake(){
        moveHandler = GetComponent<MovementHandler>();
        abilityHandler = GetComponent<AbilityHandler>();
        UIBP = GetComponent<UIBarInterpolator>();
        UIBP._maxValue = maxHP;
    }

    private void Update()
    {
        if (HP <= 0)
        {
            deathCam.SetActive(true);
            EnemyStagger.StaggerInstance.staggered = true;
            moveHandler.enabled = false;
            abilityHandler.enabled = false;
            playeranim.SetBool("Knocked", true);
        }
        if(Input.GetKeyDown(KeyCode.L))
            HP -= 50;
        UIBP.value = HP;
        HP = Mathf.Clamp(HP, 0, maxHP);

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
        moveHandler.enabled = true;
        abilityHandler.enabled = true;
        abilityHandler.stamina = 100f;
        playeranim.SetBool("Knocked", false);
        deathCam.SetActive(false);
    }
}
