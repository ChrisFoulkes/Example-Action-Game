using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCasterContext : AbilityCasterContext
{
    public Transform ProjectileSpawnPos { get; }
    public Rigidbody2D CasterRigidbody { get; }
    public PlayerMovement PlayerMovement { get; }
    public CharacterStatsController CharacterStatsController { get; }
    public PlayerStatusTracker PlayerStatusTracker { get; }
    public BuffController BuffController { get; }
    public PlayerCasterContext(Transform caster, Transform projectileSpawnPos) : base(caster)
    {
        ProjectileSpawnPos = projectileSpawnPos;
        CasterRigidbody = caster.GetComponent<Rigidbody2D>();
        PlayerMovement = caster.GetComponent<PlayerMovement>();
        CharacterStatsController = caster.GetComponent<CharacterStatsController>();
        BuffController = caster.GetComponent<BuffController>();
        PlayerStatusTracker = caster.GetComponentInChildren<PlayerStatusTracker>();
    }
}

public class EnemyCasterContext : AbilityCasterContext
{
    public EnemyCasterContext(Transform caster) : base(caster)
    {

    }
}

public abstract class AbilityCasterContext
{
    public Transform transform { get; }


    public AbilityCasterContext(Transform caster)
    {
        transform = caster;
    }
}

public class PlayerAbilityManager : MonoBehaviour
{
    private PlayerCasterContext abilityCaster;

    [SerializeField] private PlayerCharacter caster;
    [SerializeField] private Transform projectileSpawnPos;
    [SerializeField] private List<HotkeySlot> hotkeySlots;
    private Dictionary<int, HotkeySlot> AbilityIDToHotkeySlot = new Dictionary<int, HotkeySlot>();


    [SerializeField] private List<AbilityData> abilityDataList = new List<AbilityData>();
    private Dictionary<int, Ability> abilities = new Dictionary<int, Ability>();



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
                switch (context.action.name)
                {
                    case "Ability-1":
                        var ability1 = abilities.Values.ElementAt(0);
                        CastAbility(ability1);
                        break;
                    case "Ability-2":
                        var ability2 = abilities.Values.ElementAt(1);
                        CastAbility(ability2);
                        break;
                    case "Ability-3":
                        var ability3 = abilities.Values.ElementAt(2);
                        CastAbility(ability3);
                        break;
                    case "MovementAbility":
                        var ability4 = abilities.Values.ElementAt(3);
                        CastAbility(ability4);
                        break;
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
            if (ability.IsBufferable)
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
        AbilityIDToHotkeySlot[ability.ID].StartCoolDown(ability.Cooldown);
        yield return new WaitForSeconds(ability.Cooldown);
        ability.SetCoolDown(false);
    }
    private IEnumerator AutoCast(Ability ability)
    {
        CastAbility(ability);
        yield return new WaitForSeconds(ability.Cooldown);
        StartCoroutine(AutoCast(ability));
    }


    //Bug (if cooldown < casting time 
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
}