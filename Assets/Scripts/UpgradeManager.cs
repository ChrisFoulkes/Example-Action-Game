using EventCallbacks;
using Pathfinding;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    float pendingUpgrades = 0;
    public GameObject player;
    public TextMeshProUGUI pendingUpgradesText;
    public GameObject pendingUpgradePanel;
    public GameObject upgradePanel;

    public Button upgradeButton;

    public CharacterUpgradeHandler characterUpgradeHandler;

    public List<UpgradeData> upgradeData= new List<UpgradeData>(); 

    public List<UpgradeData> shownUpgrades = new List<UpgradeData>();

    public List<UpgradePanel> upgradePanels = new List<UpgradePanel>();

    private void Awake()
    {
        characterUpgradeHandler = GetComponent<CharacterUpgradeHandler>();
        characterUpgradeHandler = new CharacterUpgradeHandler(player);
    }

    private void Start()
    {
        PlayerExperienceEvent.RegisterListener(OnExpEvent);
    }

    private void OnDestroy()
    {
        PlayerExperienceEvent.UnregisterListener(OnExpEvent);
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

        pendingUpgradePanel.SetActive(enable);


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

        if (udata.baseUpgradeType == BaseUpgradeType.characterUpgrade) 
        {
            if (udata.GetType() != typeof(CharacterUpgradeData))
            {
                Debug.LogWarning(udata.name + "erronious base-type incorrectly flagged as CharacterUpgrade");
            }
            else 
            {
                characterUpgradeHandler.HandleUpgrade((CharacterUpgradeData)udata);
            }
        }
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

    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (pendingUpgrades > 0) 
            {
                OpenUpgradePanel();
            }
        }
    }


    void OpenUpgradePanel() 
    {
        GameManager.Instance.PauseGameToggle();
        GenerateAvailableUpgrades(2);

        for (int i = 0; i < upgradePanels.Count; i++)
        {
            upgradePanels[i].SetDisplay(shownUpgrades[i]);
        }
        upgradePanel.SetActive(true);
    }

}
