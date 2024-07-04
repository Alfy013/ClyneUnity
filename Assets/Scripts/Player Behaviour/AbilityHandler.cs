using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AbilityHandler : MonoBehaviour
{
    [SerializeField] TMP_Text state;
	[SerializeField] UIBarInterpolator UIBP;
	[SerializeField] Animator animator;
	[HideInInspector] public float stamina = 100f;
	[HideInInspector] public float stamRegen;
	[HideInInspector] public bool canMove = true;
	Ability abilityInUse;


    public abstract class Ability : MonoBehaviour{
        [SerializeField] internal string _inputName;
        [SerializeField] internal float _staminaCost;
        [SerializeField] internal float _cooldown;
		[SerializeField] internal bool channeled;
		[SerializeField] internal bool abortReturnsStamina = true;
		internal bool beingUsed;
		internal bool resetted;
		internal bool aborted;
		internal Animator animator;
        internal float cooldown;
        abstract internal void AbilitySetup();
		abstract internal void AbilityEffect();
		abstract internal void AbilityReset();
    }
    [SerializeField] Ability[] abilities;
    void Start()
    {
        stamRegen = 1f;
		stamina = 100f;
		UIBP._virtualMaxValue = 100;
		foreach(Ability ability in abilities){
			ability.animator = animator;
		}
    }
    void Update(){
		if(Input.GetKeyDown(KeyCode.F)){
			stamina = 100f;
			foreach(Ability ability in abilities){
				ability.cooldown = 0f;
			}
		}
		UIBP.value = stamina;
		if(abilityInUse != null)
			state.text = "State: " + abilityInUse._inputName;
		else state.text = "State: None";
		stamina = Mathf.Clamp(stamina, 0, 100);
		if (stamina < 100){
			stamRegen += Time.deltaTime;
		}
		if (stamina < 100 && stamRegen >= 3f){
			stamina += Time.deltaTime * stamRegen * 7.5f;
		}
		UIBP.value = stamina;
		foreach(Ability ability in abilities){
			if(!ability.channeled){
				if(Input.GetButtonDown(ability._inputName)){
            		if(ability.cooldown <= 0 && stamina > 1f && abilityInUse == null){
	            		ability.AbilitySetup();
						stamina -= ability._staminaCost;
						ability.cooldown = ability._cooldown;
						abilityInUse = ability;
					}
				}
        	}
			if(ability.channeled){
				bool inputCheck = false;
				if(ability._inputName != string.Empty) inputCheck = Input.GetButton(ability._inputName);
				if(inputCheck || ability.beingUsed){
					if(ability.cooldown <= 0 && stamina > 1f && (abilityInUse == null || abilityInUse == ability)){
						ability.AbilitySetup();
						stamina -= ability._staminaCost * Time.fixedDeltaTime;
						abilityInUse = ability;
						ability.resetted = false;
					}
				}
				if((!ability.beingUsed || (!inputCheck  && ability._inputName != string.Empty)) && !ability.resetted || stamina < 1f){
					ResetAbility();
					ability.resetted = true;
					ability.cooldown = ability._cooldown;
				}
			}
			if(ability.aborted){
				ability.cooldown = 0;
				if(ability.abortReturnsStamina)
					stamina += ability._staminaCost;
				ResetAbility();
			}
			if(ability.cooldown > 0f) ability.cooldown -= Time.deltaTime;
		}
		if(abilityInUse != null) stamRegen = 0f;
		if(Input.GetKeyDown(KeyCode.P)) Time.timeScale = 0.1f;
		if(Input.GetKeyDown(KeyCode.O)) Time.timeScale = 1f;
    }

	public void FireAbility(){
		if(abilityInUse != null)
			abilityInUse.AbilityEffect();
		else Debug.Log("Fired ability is null.");
	}
	public void ResetAbility(){
		if(abilityInUse != null){
			abilityInUse.AbilityReset();
			abilityInUse.aborted = false;
			abilityInUse = null;
		}else Debug.Log("Ability already null.");
	}
	void OnDisable(){
		abilityInUse = null;
		foreach(Ability ability in abilities){
			ability.AbilityReset();
			ability.cooldown = 0;
		}
		state.text = "State: Knocked";
	}
}
