using EventCallbacks;
using Pathfinding;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

[System.Serializable]
public class UpgradeDataWrapper
{
    public UpgradeData upgradeData;
    public int timesChosen;

    public UpgradeDataWrapper(UpgradeData upgradeData)
    {
        this.upgradeData = upgradeData;
        timesChosen = 0;
    }
}

public class UpgradeManager : MonoBehaviour
{
    public InputAction openUpgrade;
    float pendingUpgrades = 0;
    public GameObject player;
    public TextMeshProUGUI pendingUpgradesText;
    public GameObject upgradePanel;

    public CharacterUpgradeHandler characterUpgradeHandler;
    [SerializeField] private List<UpgradeDataWrapper> upgradeDataWrappers = new List<UpgradeDataWrapper>();

    [SerializeField] private List<UpgradeData> upgradeData= new List<UpgradeData>();

    [SerializeField] private List<UpgradeData> shownUpgrades = new List<UpgradeData>();

    [SerializeField] private List<UpgradePanel> upgradePanels = new List<UpgradePanel>();

    private void Awake()
    {
        characterUpgradeHandler = GetComponent<CharacterUpgradeHandler>();
    }

    private void Start()
    {
        characterUpgradeHandler.Initialize(player);
        PopulateUpgradeData();
    }

    private void PopulateUpgradeData()
    { 
        // Clear any existing data
        upgradeData.Clear();
        upgradeDataWrappers.Clear();

        // Retrieve and add all character upgrades
        var characterUpgrades = Resources.LoadAll<CharacterUpgradeData>("UpgradeData");
        foreach (var upgrade in characterUpgrades)
        {
            upgradeData.Add(upgrade);
            upgradeDataWrappers.Add(new UpgradeDataWrapper(upgrade));
        }

        // Retrieve and add ability upgrades for currently equipped abilities
        var playerAbilityManager = player.GetComponentInChildren<PlayerAbilityManager>();

        foreach (var ability in playerAbilityManager.GetEquippedAbilities())
        {
            // Assuming AbilityData has a unique ID
            var projectileUpgrades = Resources.LoadAll<ProjectileUpgradeData>("UpgradeData").Where(a => a.ability.AbilityID == ability);
            foreach (var upgrade in projectileUpgrades)
            {
                upgradeData.Add(upgrade);
                upgradeDataWrappers.Add(new UpgradeDataWrapper(upgrade));
            }
            // Assuming AbilityData has a unique ID
            var meleeUpgrades = Resources.LoadAll<MeleeUpgradeData>("UpgradeData").Where(a => a.ability.AbilityID == ability);
            foreach (var upgrade in meleeUpgrades)
            {
                upgradeData.Add(upgrade);
                upgradeDataWrappers.Add(new UpgradeDataWrapper(upgrade));
            }
        }
    }

    private void OnEnable()
    {

        EventManager.AddGlobalListener<PlayerExperienceEvent>(OnExpEvent);
    }

    private void OnDisable()
    {
        EventManager.RemoveGlobalListener<PlayerExperienceEvent>(OnExpEvent);
    }

    void Update()
    {
        /*
        if (Input.GetButtonDown("Fire3"))
        {
            if (pendingUpgrades > 0 && !GameManager.Instance.isPaused)
            {
                OpenUpgradePanel();
            }
        }
        */
    }



    void OnExpEvent(PlayerExperienceEvent pXPEvent)
    {
        if (pXPEvent.isLevelUP) 
        {
            pendingUpgrades++;
            pendingUpgradesText.text = "+" + pendingUpgrades;
            TogglePendingUpgradesDisplay(true);
        }
    }

    void TogglePendingUpgradesDisplay(bool enable) 
    {
        pendingUpgradesText.gameObject.SetActive(enable);
    }

    public void ChooseUpgrade(int position)
    {
        pendingUpgrades--;

        if (pendingUpgrades <= 0)
        {
            TogglePendingUpgradesDisplay(false);
        }
        else
        {
            pendingUpgradesText.text = "+" + pendingUpgrades;
        }


        UpgradeDataWrapper udataWrapper = upgradeDataWrappers.FirstOrDefault(wrapper => wrapper.upgradeData == shownUpgrades[position]);
        if (udataWrapper != null)
        {
            // Increment the timesChosen counter
            udataWrapper.timesChosen++;

            // Remove the upgrade if it reaches the limit
            if (udataWrapper.upgradeData.upgradeLimit > -1 && udataWrapper.timesChosen >=  udataWrapper.upgradeData.upgradeLimit)
            {
                upgradeData.Remove(udataWrapper.upgradeData);
            }
        }

        characterUpgradeHandler.HandleUpgrade(shownUpgrades[position]);

    }


    void GenerateAvailableUpgrades(int noOfUpgrades)
    {
        shownUpgrades.Clear();

        if (upgradeData.Count < noOfUpgrades)
        {
            Debug.LogError("Not enough UpgradeData items in upgradeData list!");
            return;
        }

        List<UpgradeData> availableUpgrades = new List<UpgradeData>(upgradeData);

        for (int i = 0; i < noOfUpgrades; i++)
        {
            int randomIndex = Random.Range(0, availableUpgrades.Count);
            UpgradeData selectedUpgrade = availableUpgrades[randomIndex];

            shownUpgrades.Add(selectedUpgrade);

            availableUpgrades.RemoveAt(randomIndex);
        }
    }


    public void OpenUpgradePanel()
    {
        if (pendingUpgrades > 0 && !GameManager.Instance.isPaused)
        {
            GameManager.Instance.PauseGameToggle();
            GenerateAvailableUpgrades(upgradePanels.Count);

            for (int i = 0; i < upgradePanels.Count; i++)
            {
                upgradePanels[i].SetDisplay(shownUpgrades[i]);
            }
            upgradePanel.SetActive(true);
        }
    }

}
