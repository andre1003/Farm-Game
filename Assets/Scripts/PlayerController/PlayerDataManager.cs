using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDataManager {
    public static PlayerData playerData;

    public static void Buy(Plant plant) {
        if(plant.buyValue <= playerData.money) {
            playerData.money -= plant.buyValue;
        }
    }

    public static void SetPlayerData(PlayerData data) {
        playerData = data;
    }

    public static string GetMoney() {
        return "Dinheiro: " + playerData.money.ToString();
    }
}
