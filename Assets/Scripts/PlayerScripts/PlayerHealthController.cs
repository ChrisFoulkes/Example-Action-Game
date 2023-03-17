using UnityEngine;

public class PlayerHealthController : HealthController
{
    protected override void GenerateCombatText(float amount)
    {
        if (amount > 0)
        {
            combatTextController.CreateFloatingCombatText("+" + amount, Color.green, false);
        }
        else
        {
            combatTextController.CreateFloatingCombatText(amount.ToString(), Color.red, false);
        }
    }
}
