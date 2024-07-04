using System;
using Unity.VisualScripting;
using UnityEngine;

public class Slashing : AbilityHandler.Ability
{
    [SerializeField] GameObject slashProjectile;
    [SerializeField] Transform slashTransform;
    internal override void AbilitySetup()
    {
        animator.SetBool("Firing", true);
        beingUsed = true;
    }
    internal override void AbilityEffect()
    {
        Instantiate(slashProjectile, slashTransform.position, slashTransform.transform.rotation);
    }
    internal override void AbilityReset()
    {
        animator.SetBool("Firing", false);
        beingUsed = false;
    }
}
