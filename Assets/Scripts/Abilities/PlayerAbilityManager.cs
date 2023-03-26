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

    public bool isCasting;
    private Ability bufferedAbility;
    private bool hasBufferedAbility = false;

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
            if (aData.abilityType == AbilityType.movement)
            {
                abilities.Add(aData.AbilityID, new MovementAbility((MovementData)aData, caster.transform));
            }
            if (aData.abilityType == AbilityType.buff)
            {
                abilities.Add(aData.AbilityID, new BuffAbility((BuffData)aData, this));
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
                if (Input.GetButtonDown("Jump"))
                {
                    var ability = abilities.Values.ElementAt(2);
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
        if (ability.IsCastable() && !isCasting)
        {
            ability.CastAbility();
            StartCoroutine(HandleAbilityCooldown(ability));
            StartCoroutine(HandleAbilityCasting(ability));
        }
        else if (ability.RemainingCooldown() <= 1)
        {
            // Buffer the ability if another ability is not being cast and cooldown is 1 second or lower
            if (ability.isBufferable)
            {
                bufferedAbility = ability;
                hasBufferedAbility = true;
                StartCoroutine(HandleBufferedAbility());
            }
        }
    }

    public void UpgradeAbility(int abilityID, UpgradeEffect upgradeEffect)
    {
        if (!abilities.ContainsKey(abilityID))
        {
            Debug.LogWarning("No ability found with ID: " + abilityID);
            return;
        }

        abilities[abilityID].ApplyUpgrade(upgradeEffect);
    }

    private IEnumerator HandleAbilityCooldown(Ability ability)
        {
            ability.SetCoolDown(true);
            AbilityIDToHotkeySlot[ability.ID].StartCoolDown(ability.cooldown);
            yield return new WaitForSeconds(ability.cooldown);
            ability.SetCoolDown(false);
    }


    //Bug (if casting time < 
    private IEnumerator HandleAbilityCasting(Ability ability)
    {
        isCasting = true;
        yield return new WaitForSeconds(ability.castTime);
        isCasting = false;
    }
    private IEnumerator HandleBufferedAbility()
    {
        while (hasBufferedAbility)
        {
            if (bufferedAbility.IsCastable() && !isCasting)
            {
                hasBufferedAbility = false;
                CastAbility(bufferedAbility);
            }
            else
            {
                yield return null; // Wait for the next frame before checking again
            }
        }
    }
}