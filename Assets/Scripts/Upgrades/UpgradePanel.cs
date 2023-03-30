using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    UpgradeData uData;

    public Image image;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDisplay(UpgradeData data)
    {
        uData = data;
        image.sprite = data.upgradeIcon;
        titleText.text = data.upgradeName;
        descriptionText.text = data.upgradeDescription;

    }
}
