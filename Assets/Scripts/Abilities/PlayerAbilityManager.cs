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


    public void UpgradeAbility(int slot, ProjectileUpgradeTypes upgradeType, float amount) 
    {
        if (slot > abilities.Count) 
        {
            Debug.LogWarning("altering ability in empty slot: " + slot);
            return;
        }

        if (abilities[slot].abilityType == AbilityType.projectile)
        {
            ProjectileAbility projAbility = abilities[0] as ProjectileAbility;

            if (upgradeType == ProjectileUpgradeTypes.projectileDamage)
            {
                projAbility.projData.projectileDamage += Mathf.RoundToInt(amount);
            }
            if (upgradeType == ProjectileUpgradeTypes.projectileCount)
            {
                projAbility.projData.projectileCount += Mathf.RoundToInt(amount);
            }
            if (upgradeType == ProjectileUpgradeTypes.projectileArc) 
            {
                if (projAbility.projData.firingArc + amount < 360)
                {
                    projAbility.projData.firingArc += amount;
                }
                else 
                {
                    projAbility.projData.firingArc = 360;
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
