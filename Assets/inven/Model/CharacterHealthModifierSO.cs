using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        Damageable health = character.GetComponent<Damageable>();
        if (health != null)
            health.Heal((int)val);
    }
}