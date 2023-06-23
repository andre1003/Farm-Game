using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoreTutorial", menuName = "ScriptableObjects/Tutorials/Store Tutorial")]
public class StoreTutorial : TutorialInstruction
{
    public override void SetupActions()
    {
        // Call base SetupActions method
        base.SetupActions();

        //// Actions
        // Buy
        actions[0] += BuyApple;

        // Sell
        actions[1] += SellBanana;
    }




    #region Actions
    /// <summary>
    /// Check if player bought an apple.
    /// </summary>
    private void BuyApple()
    {
        actionCompleted = Inventory.instance.inventory.HasPlant("apple");
    }

    /// <summary>
    /// Check if player sold at least one banana.
    /// </summary>
    private void SellBanana()
    {
        actionCompleted = Store.instance.HasSold("banana");
    }
    #endregion
}
