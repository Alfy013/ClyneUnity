using UnityEngine;
using TMPro;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] int maxHP = 250;
    [SerializeField] Animator playeranim;
	[SerializeField] TMP_Text UIHPText;
	[SerializeField] TMP_Text UIHitText;
    [SerializeField] GameObject deathCam;
    private bool defaulted = true;
    private int HP = 250;

    
    PlayerHandler moveSystem;
    private int hitsTaken = 0;

    private void Start(){
        moveSystem = FindObjectOfType<PlayerHandler>();
    }

    private void Update()
    {
        UIHPText.text = "Health: " + HP;
		UIHitText.text = "Hits taken: " + hitsTaken;
        if (HP <= 0)
        {
            if(defaulted){
                defaulted = false;
                moveSystem.StopAction();
                moveSystem.StartAction(0, PlayerHandler.PlayerState.Knocked, true);
                deathCam.SetActive(true);
                EnemyStagger.StaggerInstance.staggerTimer = -1f;
                Debug.Log("dead");
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
        moveSystem.stamRegen = 0f;
    }
    public void Unknocked(){
        HP = maxHP;
        moveSystem.StopAction();
        playeranim.SetBool("Knocked", false);
        deathCam.SetActive(false);
        defaulted = true;
        moveSystem.stamina = 100f;
        Debug.Log("test");
    }
}
