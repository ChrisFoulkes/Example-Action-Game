using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

    public class PlayerAbilityManager : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter caster;
        [SerializeField] private List<HotkeySlot> hotkeySlots;
        private Dictionary<int, HotkeySlot> AbilityIDToHotkeySlot = new Dictionary<int, HotkeySlot>();


        [SerializeField] private List<AbilityData> abilityDataList = new List<AbilityData>();
        private Dictionary<int, Ability> abilities = new Dictionary<int, Ability>();

        public Transform projectileSpawnPos;

    private void Awake()
    {
        foreach (AbilityData aData in abilityDataList)
        {
            if (aData.abilityType == AbilityType.projectile)
            {
                abilities.Add(aData.AbilityID, new ProjectileAbility((ProjectileData)aData, caster.transform, projectileSpawnPos));
            }
            if (aData.abilityType == AbilityType.melee)
            {
                abilities.Add(aData.AbilityID, new MeleeAbility((MeleeData)aData, caster.transform, projectileSpawnPos));
            }
        }

        int slotIndex = 0;
        foreach (var abilityEntry in abilities)
        {
            if (slotIndex < hotkeySlots.Count)
            {
                hotkeySlots[slotIndex].Icon.sprite = abilityEntry.Value.sprite;
                AbilityIDToHotkeySlot.Add(abilityEntry.Key, hotkeySlots[slotIndex]);
                slotIndex++;
            }
            else
            {
                break;
            }
        }
    }

        private void Update()
        {
            if (!GameManager.Instance.isPaused && !caster.IsDead())
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    var ability = abilities.Values.ElementAt(0);
                    CastAbility(ability);
                }
                if (Input.GetButtonDown("Fire2"))
                {
                    var ability = abilities.Values.ElementAt(1);
                    CastAbility(ability);
                }
            }
        }

    public List<int> GetEquippedAbilities()
    {
        List<int> equippedAbilityIDs = new List<int>();

        foreach (var abilityEntry in abilities)
        {
            equippedAbilityIDs.Add(abilityEntry.Key);
        }

        return equippedAbilityIDs;
    }

    private void CastAbility(Ability ability)
        {
            if (ability.IsCastable())
            {
                ability.CastAbility();
                StartCoroutine(HandleAbilityCooldown(ability));
            }
        }

    public void UpgradeAbility(int abilityID, UpgradeEffect upgradeEffect)
    {
        if (!abilities.ContainsKey(abilityID))
        {
            Debug.LogWarning("No ability found with ID: " + abilityID);
            return;
        }

        Ability foundAbility = abilities[abilityID];

        if (foundAbility.abilityType == AbilityType.projectile && upgradeEffect is ProjectileUpgradeEffect projectileUpgradeEffect)
        {
            ProjectileAbility projAbility = foundAbility as ProjectileAbility;

            switch (projectileUpgradeEffect.upgradeType)
            {
                case ProjectileUpgradeTypes.projectileDamage:
                    projAbility.projData.projectileDamage += Mathf.RoundToInt(projectileUpgradeEffect.amount);
                    break;
                case ProjectileUpgradeTypes.projectileCount:
                    projAbility.projData.projectileCount += Mathf.RoundToInt(projectileUpgradeEffect.amount);
                    break;
                case ProjectileUpgradeTypes.projectileArc:
                    projAbility.projData.firingArc = Mathf.Clamp(projAbility.projData.firingArc + projectileUpgradeEffect.amount, 0, 360);
                    break;
            }
        }

        if (foundAbility.abilityType == AbilityType.melee && upgradeEffect is MeleeUpgradeEffect meleeUpgradeEffect)
        {
            MeleeAbility meleeAbility = foundAbility as MeleeAbility;

            switch (meleeUpgradeEffect.upgradeType)
            {
                case MeleeUpgradeTypes.meleeDamage:
                    meleeAbility.meleeData.meleeDamage += Mathf.RoundToInt(meleeUpgradeEffect.amount);
                    break;
                case MeleeUpgradeTypes.meleeCastSpeed:
                    meleeAbility.meleeData.castTime = Mathf.Max(meleeAbility.meleeData.castTime + meleeUpgradeEffect.amount,(meleeAbility.meleeData.originalCastTime * 0.75f));
                    break;
            }
        }
    }

    private IEnumerator HandleAbilityCooldown(Ability ability)
        {
            ability.SetCoolDown(true);
            AbilityIDToHotkeySlot[ability.ID].StartCoolDown(ability.cooldown);
            yield return new WaitForSeconds(ability.cooldown);
            ability.SetCoolDown(false);
        }
    }