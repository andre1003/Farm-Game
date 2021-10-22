using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour {
    public PlayerData playerData;
    public Text moneyText;

    #region Singleton
    public static PlayerDataManager instance;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of PlayerDataManager found!");
            return;
        }

        instance = this;
        UpdateMoneyText();
    }

    #endregion


    // Method for buy a plant
    public void Buy(Plant plant) {
        bool canAdd = Inventory.instance.Add(plant);

        // Check if player have money and slots enough to buy
        if(plant.buyValue <= playerData.money && canAdd) {
            playerData.money -= plant.buyValue;
            UpdateMoneyText();
        }
    }

    // Method for update money text
    private void UpdateMoneyText() {
        moneyText.text = "Dinheiro: " + playerData.money.ToString();
    }

    // Method for adding money
    public void SetMoney(float moneyToAdd) {
        playerData.money += moneyToAdd;
    }

    // Method for adding XP
    public void SetXp(int xp) {
        playerData.xp += xp;

        if(playerData.xp >= playerData.nextLvlXp) {
            playerData.xp = playerData.nextLvlXp - playerData.xp;
            playerData.level++;
            playerData.nextLvlXp += 10;
        }
    }
}
