using EventCallbacks;
using Pathfinding;
using System.Collections.Generic;
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
        if (Input.GetButtonDown("Fire2"))
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

        switch (udata.baseUpgradeType)
        {
            case BaseUpgradeType.characterUpgrade:
                if (udata is CharacterUpgradeData characterUpgradeData)
                {
                    HandleCharacterUpgrade(characterUpgradeData);
                }
                else
                {
                    Debug.LogWarning(udata.name + "erroneous base-type incorrectly flagged as CharacterUpgrade");
                }
                break;

            case BaseUpgradeType.abilityUpgrade:
                if (udata is ProjectileUpgradeData abilityUpgradeData)
                {
                    HandleAbilityUpgrade(abilityUpgradeData);
                }
                else
                {
                    Debug.LogWarning(udata.name + "erroneous base-type incorrectly flagged as AbilityUpgrade");
                }
                break;
        }
    }

    private void HandleCharacterUpgrade(CharacterUpgradeData upgradeData)
    {
        characterUpgradeHandler.HandleCharacterUpgrade(upgradeData);
    }

    private void HandleAbilityUpgrade(ProjectileUpgradeData upgradeData)
    {
        characterUpgradeHandler.HandleAttackUpgrade(upgradeData);
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
