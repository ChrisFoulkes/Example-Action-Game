using EventCallbacks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilityManager : MonoBehaviour
{
    private PlayerCasterContext abilityCaster;

    [SerializeField] private PlayerCharacter caster;
    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private List<HotkeySlot> hotkeySlots;
    private Dictionary<int, HotkeySlot> AbilityIDToHotkeySlot = new Dictionary<int, HotkeySlot>();


    [SerializeField] private List<AbilityData> abilityDataList = new List<AbilityData>();
    private Dictionary<int, Ability> abilities = new Dictionary<int, Ability>();
    private Dictionary<string, int> abilityNameToIndex = new Dictionary<string, int> {
        { "Ability-1", 0 },
        { "Ability-2", 1 },
        { "Ability-3", 2 },
        { "MovementAbility", 3 }
    };

    [SerializeField] private bool isCasting;
    private Ability bufferedAbility;
    private bool hasBufferedAbility = false;

    private void Awake()
    {
        abilityCaster = new PlayerCasterContext(caster.transform, projectileSpawnPos);

        foreach (AbilityData aData in abilityDataList)
        {
            Ability newAbility = aData.AbilityFactory.Create(aData, abilityCaster);
            abilities.Add(aData.AbilityID, newAbility);
        }

        int slotIndex = 0;
        foreach (var abilityEntry in abilities)
        {
            if (slotIndex < hotkeySlots.Count)
            {
                hotkeySlots[slotIndex].Icon.sprite = abilityEntry.Value.Sprite;
                AbilityIDToHotkeySlot.Add(abilityEntry.Key, hotkeySlots[slotIndex]);
                slotIndex++;
            }
            else
            {
                break;
            }
        }
    }

    public void AbilityButtonPress(InputAction.CallbackContext context)
    {
        if (!GameManager.Instance.isPaused && !caster.IsDead())
        {
            if (context.performed)
            {
                string actionName = context.action.name;
                if (abilityNameToIndex.ContainsKey(actionName))
                {
                    var ability = abilities.Values.ElementAt(abilityNameToIndex[actionName]);
                    TryCastAbility(ability);
                }
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

    private void TryCastAbility(Ability ability)
    {
        if (ability.IsCastable() && !isCasting)
        {
            CastAbility(ability);
        }
        else if (ability.RemainingCooldown() <= 1)
        {
            // Buffer the ability if another ability is not being cast and cooldown is 1 second or lower
            if (ability.IsBufferable)
            {
                bufferedAbility = ability;
                hasBufferedAbility = true;
                StartCoroutine(HandleBufferedAbility());
            }
        }
    }

    public void CastAbility(Ability ability) 
    {
        ability.CastAbility();
        StartCoroutine(HandleAbilityCooldown(ability));
        StartCoroutine(HandleAbilityCasting(ability));

        if (ability.ShouldStopMovementOnCast) 
        {
            PlayerStopMovementEvent stopMovementEvent = new PlayerStopMovementEvent{duration = ability.CastTime };
            EventManager.Raise(stopMovementEvent);
        }

        if (ability.ShouldForceCastingDirection) 
        {
            SetPlayerFacingDirectionEvent setDirectionEvent = new SetPlayerFacingDirectionEvent(AbilityUtils.GetFacingDirection(caster.transform.position), ability.CastTime);
            EventManager.Raise(setDirectionEvent);
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
        AbilityIDToHotkeySlot[ability.ID].StartCoolDown(ability.Cooldown);
        yield return new WaitForSeconds(ability.Cooldown);
        ability.SetCoolDown(false);
    }

    private IEnumerator HandleAbilityCasting(Ability ability)
    {
        isCasting = true;
        yield return new WaitForSeconds(ability.CastTime);
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

    private IEnumerator AutoCast(Ability ability) //Testing Tool
    {
        while (true)
        {
            CastAbility(ability);
            yield return new WaitForSeconds(ability.Cooldown);
        }
    }
}