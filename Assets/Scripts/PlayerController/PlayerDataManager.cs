using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataManager : MonoBehaviour {
    #region Singleton
    public static PlayerDataManager instance;
    private void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of PlayerDataManager found!");
            return;
        }

        instance = this;
        UpdateHUD();
    }

    #endregion

    public PlayerData playerData;

    public Text moneyText;
    public Text levelText;
    public Text xpText;


    private bool editMode = false;

    void Start() {
        editMode = false;
    }

    // Method for buy a plant
    public void Buy(Plant plant, int amount) {
        if(plant.levelRequired <= playerData.level) {
            bool canAdd = Inventory.instance.Add(plant, amount);

            // Check if player have money and slots enough to buy
            if(plant.buyValue <= playerData.money && canAdd) {
                playerData.money -= plant.buyValue;
                UpdateHUD();
            }
        }
        else {
            Debug.Log("Not available for your level");
        }        
    }

    // Method for update money text
    private void UpdateHUD() {
        moneyText.text = "Dinheiro: " + playerData.money.ToString();
        levelText.text = "Level: " + playerData.level;
        xpText.text = playerData.xp + " / " + playerData.nextLvlXp;
    }

    // Method for adding money
    public void SetMoney(float moneyToAdd) {
        playerData.money += moneyToAdd;
        UpdateHUD();
    }

    // Method for adding XP
    public void SetXp(int xp) {
        playerData.xp += xp;

        if(playerData.xp >= playerData.nextLvlXp) {
            playerData.xp = playerData.nextLvlXp - playerData.xp;
            playerData.level++;
            playerData.nextLvlXp += 10;
        }

        UpdateHUD();
    }


    public void SetEditMode(bool editMode) { 
        this.editMode = editMode;
    }

    public bool GetEditMode() {
        return editMode;
    }
}
