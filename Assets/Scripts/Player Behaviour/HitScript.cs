using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HitScript : MonoBehaviour
{
    [HideInInspector] public int HP = 250;
    private int actualHP;
    private bool defaulted;
    private float stunTimer;
    [SerializeField] Animator playeranim;
	[SerializeField] TMP_Text UIHPText;
	[SerializeField] TMP_Text UIHitText;
    [SerializeField] PlayerHandler moveSystem;
    private int hitsTaken = 0;
    float cd = 0f;
    public void TakeHit(int damage, float stunTime = 0)
    {
        HP -= damage;
        hitsTaken++;
        moveSystem.stamRegen = 0f;
    }
    private void Update()
    {
        UIHPText.text = "Health: " + HP;
		UIHitText.text = "Hits taken: " + hitsTaken;
        if (stunTimer > 0f)
        {
            defaulted = false;
            moveSystem.canMove = false;
            moveSystem.actionPlaying = true;
            stunTimer -= Time.deltaTime;
        }
        else
        {
            if (!defaulted)
            {
				moveSystem.canMove = true;
				moveSystem.actionPlaying = false;
                defaulted = true;
			}
		}

        if (Input.GetKeyDown(KeyCode.F))
        {
            hitsTaken = 0;
            HP = 250;
            moveSystem.stamina = 100f;
        }
        if (cd > 0f) cd -= Time.deltaTime;
    }
	private void OnTriggerEnter(Collider other)
	{
        if (other.CompareTag("EnemySwordHitbox"))
        {
            HP -= 50;
            hitsTaken++;
        }
	}
}
