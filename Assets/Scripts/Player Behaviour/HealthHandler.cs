using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] int maxHP = 250;
    [SerializeField] Animator playeranim;
	[SerializeField] TMP_Text UIHPText;
	[SerializeField] TMP_Text UIHitText;
    [SerializeField] GameObject deathCam;
    [SerializeField] Image hpfill;
    [SerializeField] float _fillStabilizeMultiplier;
    [SerializeField] Color _maxColor;
    [SerializeField] Color _minColor;
    [SerializeField] float healthFlashExponent = 10;
    [SerializeField] ParticleSystem hit;
    private bool defaulted = true;
    private int HP = 250;
    private float clampedHP;
    private float oldClampedHP_IP;
    private float newClampedHP_IP;
    private float _IP_timer;
    private float IP_timer;

    
    PlayerHandler moveSystem;
    private int hitsTaken = 0;

    private void Start(){
        moveSystem = FindObjectOfType<PlayerHandler>();
        oldClampedHP_IP = 1;
        newClampedHP_IP = 1;
        clampedHP = 1;
    }

    private void Update()
    {
        HP = Mathf.Clamp(HP, 0, maxHP);
        oldClampedHP_IP = newClampedHP_IP;
        newClampedHP_IP = (float)HP / (float)maxHP;

        if(!Mathf.Approximately(oldClampedHP_IP, newClampedHP_IP)){
            _IP_timer = Mathf.Abs(oldClampedHP_IP - newClampedHP_IP) * _fillStabilizeMultiplier;
            IP_timer = _IP_timer;
        }
        if(IP_timer > 0f){
            clampedHP = Mathf.Lerp(clampedHP, newClampedHP_IP, 1 - IP_timer/_IP_timer);
            IP_timer -= Time.deltaTime;
        }
        hpfill.fillAmount = clampedHP;
        UIHPText.text = HP + "/" + maxHP;
		UIHitText.text = "Hits taken: " + hitsTaken;
        UIHPText.color = Color.Lerp(_minColor, _maxColor, (float)HP/maxHP);
        if((float)HP / maxHP <= 0.4f && HP > 0) UIHPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(-0.5f, 0.5f, Mathf.PingPong(Time.time * Mathf.Pow(healthFlashExponent, 1 - ((float)HP / maxHP / 0.4f)), 1)));
        else UIHPText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -0.5f);
        if (HP <= 0)
        {
            if(defaulted){
                defaulted = false;
                moveSystem.StopAction();
                moveSystem.StartAction(0, PlayerHandler.PlayerState.Knocked, true);
                deathCam.SetActive(true);
                EnemyStagger.StaggerInstance.staggerTimer = -1f;
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
