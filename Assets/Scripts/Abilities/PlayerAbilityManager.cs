using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAbilityManager : MonoBehaviour
{
    [SerializeField] private PlayerCharacter caster;
    [SerializeField] private HotkeySlot hotkeySlot;

    [SerializeField] private List<AbilityData>  abilityData = new List<AbilityData>();
    [SerializeField] private List<Ability> abilities = new List<Ability>();

    public Transform projectileSpawnPos;
    void Start()
    {
        foreach (AbilityData aData in abilityData) 
        {
            if (aData.abilityType == AbilityType.projectile) 
            {
                abilities.Add(new ProjectileAbility((ProjectileData)aData, caster.transform, projectileSpawnPos));
            }
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused && !caster.IsDead())
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (abilities[0].IsCastable())
                {
                    abilities[0].CastAbility();
                    StartCoroutine(StartTickCooldown(abilities[0]));
                }
            }
        }
    }

    private IEnumerator StartTickCooldown(Ability ability)
    {
        ability.SetCoolDown(true);
        hotkeySlot.StartCoolDown(ability.cooldown);
        yield return new WaitForSeconds(ability.cooldown);
        ability.SetCoolDown(false);
    }
}
