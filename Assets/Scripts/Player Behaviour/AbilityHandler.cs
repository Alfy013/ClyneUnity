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
	int currentAbility;
	Ability abilityInUse;


    public abstract class Ability : MonoBehaviour{
        [SerializeField] internal string _inputName;
        [SerializeField] internal float _staminaCost;
        [SerializeField] internal float _cooldown;
		[SerializeField] internal bool channeled;
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
		UIBP._maxValue = 100;
		foreach(Ability ability in abilities){
			ability.animator = animator;
		}
    }
    void Update(){
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
			if(Input.GetButtonDown(ability._inputName)){
            	if(ability.cooldown <= 0 && stamina > 1f && abilityInUse == null){
	            	ability.AbilitySetup();
					stamina -= ability._staminaCost;
					ability.cooldown = ability._cooldown;
					abilityInUse = ability;
				}
        	}
			if(ability.aborted){
				ability.cooldown = 0;
				stamina += ability._staminaCost;
				ResetAbility();
			}
			if(ability.cooldown > 0f) ability.cooldown -= Time.deltaTime;
		}
		if(abilityInUse != null) stamRegen = 0f;
    }

	public void FireAbility(){
		if(abilityInUse != null)
			abilityInUse.AbilityEffect();
		else Debug.Log("Fired ability is null.");
	}
	public void ResetAbility(){
		if(abilityInUse != null){
			abilityInUse.AbilityReset();
			abilityInUse.cooldown = abilityInUse._cooldown;
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
    /*public void StopAction()
	{
		playerState = PlayerState.None;
		canMove = true;
	}*/

    /*public bool StartAction(float stamCost, PlayerState state, bool stopMove = false)
	{
		if(stamina > 1 && (playerState == state || playerState == PlayerState.None) && stamCost < stamina){
			canMove = !stopMove;
			playerState = state;
			stamRegen = 0f;
			stamina -= stamCost;
			return true;
		}
		return false;
	}
*/
}
