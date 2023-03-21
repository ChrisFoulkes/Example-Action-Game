using EventCallbacks;
using Pathfinding;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UpgradeManager : MonoBehaviour
{
    float pendingUpgrades = 0;
    public GameObject player;
    public TextMeshProUGUI pendingUpgradesText;
    public GameObject upgradePanel;

    public CharacterUpgradeHandler characterUpgradeHandler;

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

        // Retrieve and add all character upgrades
        var characterUpgrades = Resources.LoadAll<CharacterUpgradeData>("UpgradeData");
        upgradeData.AddRange(characterUpgrades);

        // Retrieve and add ability upgrades for currently equipped abilities
        var playerAbilityManager = player.GetComponentInChildren<PlayerAbilityManager>();

        foreach (var ability in playerAbilityManager.GetEquippedAbilities())
        {
            // Assuming AbilityData has a unique ID
            var projectileUpgrades = Resources.LoadAll<ProjectileUpgradeData>("UpgradeData").Where(a => a.ability.AbilityID == ability);
            upgradeData.AddRange(projectileUpgrades);

            // Assuming AbilityData has a unique ID
            var meleeUpgrades = Resources.LoadAll<MeleeUpgradeData>("UpgradeData").Where(a => a.ability.AbilityID == ability);
            upgradeData.AddRange(meleeUpgrades);
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
        if (Input.GetButtonDown("Fire3"))
        {
            if (pendingUpgrades > 0 && !GameManager.Instance.isPaused)
            {
                OpenUpgradePanel();
            }
        }
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

    public void ChooseUpgrade(int positon) 
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


        UpgradeData udata = shownUpgrades[positon];

        characterUpgradeHandler.HandleUpgrade(udata);
       
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


    void OpenUpgradePanel() 
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
