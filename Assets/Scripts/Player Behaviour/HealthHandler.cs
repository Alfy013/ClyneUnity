using UnityEngine;
using TMPro;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] int maxHP = 250;
    [SerializeField] float _deathStunTime;
    [SerializeField] Animator playeranim;
	[SerializeField] TMP_Text UIHPText;
	[SerializeField] TMP_Text UIHitText;
    [SerializeField] GameObject deathCam;
    private bool defaulted;
    private float stunTimer;

    [HideInInspector]
    public static float deathStunTime;
    private float regeneratingHP;
    private int HP = 250;

    
    PlayerHandler moveSystem;
    private int hitsTaken = 0;

    private void Start(){
        moveSystem = FindObjectOfType<PlayerHandler>();
    }

    private void Update()
    {
        if(HP <= 0){
            playeranim.SetBool("Knocked", true);
            deathStunTime = _deathStunTime;
        } else playeranim.SetBool("Knocked", false);
        UIHPText.text = "Health: " + HP;
		UIHitText.text = "Hits taken: " + hitsTaken;
        if (stunTimer > 0f || deathStunTime > 0f)
        {
            if(defaulted){
                defaulted = false;
                moveSystem.canMove = false;
                moveSystem.actionPlaying = true;
                regeneratingHP = 0;
                deathCam.SetActive(true);
            }
        }
        else
        {
            if (!defaulted)
            {
				moveSystem.canMove = true;
				moveSystem.actionPlaying = false;
                defaulted = true;
                deathCam.SetActive(false);
			}
		}
        if(stunTimer > 0f) stunTimer -= Time.deltaTime;
        if(deathStunTime > 0f){
            deathStunTime -= Time.deltaTime;
            if(regeneratingHP < maxHP)
                regeneratingHP += Time.deltaTime * (maxHP / _deathStunTime);
            HP = Mathf.FloorToInt(regeneratingHP);
            EnemyStagger.StaggerInstance.staggerTimer = -1f;
            EnemyStagger.StaggerInstance.stunDuration = 1f;
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
        moveSystem.stamRegen = 0f;
        stunTimer = stunTime;
    }
}
