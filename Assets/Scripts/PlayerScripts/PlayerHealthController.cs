using Pathfinding;
using UnityEngine;

public class PlayerHealthController : HealthController
{
    public void Start()
    {
        Initialize(20);
    }
    protected override void GenerateCombatText(float amount, FloatingColourType colourType = FloatingColourType.Generic)
    {
        if (amount > 0)
        {
            FloatingCombatTextController.Instance.CreateFloatingCombatText("+" + amount, transform, FloatingColourType.Heal, false);
        }
        else if (amount < 0)
        {
            FloatingCombatTextController.Instance.CreateFloatingCombatText(amount.ToString(), transform, colourType, false);
        }
    }

}