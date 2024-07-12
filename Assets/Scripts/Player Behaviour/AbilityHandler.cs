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
	//[HideInInspector] public float stamRegen;
	[HideInInspector] public bool canMove = true;
	Ability abilityInUse;
	Ability lastAbilityUsed;

    public abstract class Ability : MonoBehaviour{
        [SerializeField] internal string _inputName;
        [SerializeField] internal float _staminaCost;
        [SerializeField] internal float _cooldown;
		[SerializeField] internal bool consumeStaminaOnEffect;
		[SerializeField] internal bool channeled;
		internal bool resetted;
		internal bool beingUsed;
		internal Animator animator;
        internal float cooldown;
		internal AbilityHandler abilityHandler;
        abstract internal void AbilitySetup();
		abstract internal void AbilityEffect();
		abstract internal void AbilityReset();
		public void StopAbility(){
			abilityHandler.EndCurrentAbility();
		}
    }
    [SerializeField] Ability[] abilities;
    void Start()
    {
        //stamRegen = 1f;
		stamina = 0f;
		UIBP._virtualMaxValue = 100;
		foreach(Ability ability in abilities){
			ability.animator = animator;
			ability.abilityHandler = this;
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
		/*if (stamina < 100){
			stamRegen += Time.deltaTime;
		}*/
		/*if (stamina < 100 && stamRegen >= 3f){
			stamina += Time.deltaTime * stamRegen * 7.5f;
		}*/
		UIBP.value = stamina;
		foreach(Ability ability in abilities){
			if(!ability.channeled){
				bool inputCheck = false;
				float inputAxis = -2;
				if(ability._inputName != string.Empty){
					inputCheck = Input.GetButtonDown(ability._inputName);
					inputAxis = Input.GetAxisRaw(ability._inputName);
				}
				if(inputCheck || inputAxis == 1 || ability.beingUsed){
            		if(ability.cooldown <= 0 && stamina >= ability._staminaCost && abilityInUse == null){
	            		ability.AbilitySetup();
						if(!ability.consumeStaminaOnEffect)
							stamina -= ability._staminaCost;
						abilityInUse = ability;
						ability.cooldown = ability._cooldown;
						ability.resetted = false;
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
				if(!ability.beingUsed || (!inputCheck  && ability._inputName != string.Empty)|| stamina < 1f){
					if(!ability.resetted){
						EndCurrentAbility();
						ability.resetted = true;
						ability.cooldown = ability._cooldown;
					}
				}
			}
			if(ability.cooldown > 0f && abilityInUse != ability) ability.cooldown -= Time.deltaTime;
		}
		//if(abilityInUse != null) stamRegen = 0f;
		if(Input.GetKeyDown(KeyCode.P)) Time.timeScale = 0.1f;
		if(Input.GetKeyDown(KeyCode.O)) Time.timeScale = 1f;
    }

	public void FireAbility(){
		if(abilityInUse != null){
			abilityInUse.AbilityEffect();
			if(abilityInUse.consumeStaminaOnEffect) stamina -= abilityInUse._staminaCost;
		}
		else{
			lastAbilityUsed.AbilityEffect();
			if(lastAbilityUsed.consumeStaminaOnEffect) stamina -= lastAbilityUsed._staminaCost;
		}
	}
	public void EndCurrentAbility(){
		if(abilityInUse != null){
			abilityInUse.resetted = true;
			lastAbilityUsed = abilityInUse;
			abilityInUse.AbilityReset();
			abilityInUse.cooldown = abilityInUse._cooldown;
			abilityInUse = null;
		}else Debug.Log("Ability already null.");
	}
	void OnDisable(){
		lastAbilityUsed = null;
		abilityInUse = null;
		foreach(Ability ability in abilities){
			ability.AbilityReset();
			ability.cooldown = 0;
		}
		state.text = "State: Knocked";
	}
}
