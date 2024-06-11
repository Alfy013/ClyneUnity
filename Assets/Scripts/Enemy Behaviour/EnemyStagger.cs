using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStagger : MonoBehaviour
{
	public static EnemyStagger StaggerInstance;
	[SerializeField] int maxHP;
	[SerializeField] ParticleSystem hit;
	[SerializeField] GameObject[] rotationPoints;
	[SerializeField] TMP_Text hpText;
	[SerializeField] Image hpFill;
	[SerializeField] Color _maxColor;
	[SerializeField] Color _minColor;
	[SerializeField] float _fillStabilizeMultiplier;
	[SerializeField] float healthFlashExponent;
	[SerializeField] float flashingThreshold;
	[SerializeField] float damageFromProjectiles;
	[HideInInspector]
	public float HP;
	public bool staggered;
	private float hitCooldown;
	private float clampedHP;
    private float oldClampedHP_IP;
    private float newClampedHP_IP;
    private float _IP_timer;
    private float IP_timer;


    private void Start(){
        oldClampedHP_IP = 1;
        newClampedHP_IP = 1;
        clampedHP = 1;
    }

	private void Awake()
	{
		if (StaggerInstance == null) StaggerInstance = this;
		HP = 0;
		hitCooldown = 0f;
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
        hpFill.fillAmount = clampedHP;
        hpText.text = (int)HP + "/" + maxHP;
        hpText.color = Color.Lerp(_minColor, _maxColor, (float)HP/maxHP);
        hpFill.color = Color.Lerp(_minColor, _maxColor, (float)HP/maxHP);
        if((float)HP / maxHP <= flashingThreshold / 100f && HP > 0) hpText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(-0.5f, 0.5f, Mathf.PingPong(Time.time * Mathf.Pow(healthFlashExponent, 1 - ((float)HP / maxHP / flashingThreshold / 100f)), 1)));
        else hpText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, -0.5f);
		if(HP > 0f) staggered = false;
		else staggered = true;
		if(Input.GetKeyDown(KeyCode.Alpha1)){
			foreach(GameObject point in rotationPoints){
				point.transform.position = new Vector3(transform.position.x, 5.66f, transform.position.z);
			}
			HP = maxHP;
		}
		if(hitCooldown > 0f)
		{
			hitCooldown -= Time.deltaTime;
			//animator.SetBool("wasHit", true);
		}
		//else animator.SetBool("wasHit", false);


	}

	private void OnTriggerEnter(Collider collision)
	{
		if (collision.CompareTag("FProjectile"))
		{
			hitCooldown = 0.4f;
			HP -= damageFromProjectiles;
			hit.Play();
		}
	}
}
