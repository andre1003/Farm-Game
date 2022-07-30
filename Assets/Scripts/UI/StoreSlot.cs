using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour {
    public Image icon;

    public Image levelImage;
    public Image costImage;

    public Text levelText;
    public Text costText;

    private Plant plant;

    public void AddPlant(Plant plant) {
        this.plant = plant;
        icon.sprite = plant.icon;

        levelText.enabled = true;
        costText.enabled = true;

        levelText.text = plant.levelRequired.ToString();
        costText.text = plant.buyValue.ToString();

        icon.enabled = true;
        levelImage.enabled = true;
        costImage.enabled = true;
    }

    public void ClearSlot() {
        plant = null;
        icon.sprite = null;

        levelImage.enabled = false;
        costImage.enabled = false;

        levelText.enabled = false;
        costText.enabled = false;

        icon.enabled = false;

        //levelText.text = "0";
        //costText.text = "000,00";
    }


    public void AddPlantToSell(Plant plant, int amount) {
        this.plant = plant;
        icon.sprite = plant.icon;

        levelText.enabled = true;
        costText.enabled = true;

        levelText.text = amount.ToString();
        costText.text = plant.baseSellValue.ToString();

        icon.enabled = true;
        levelImage.enabled = true;
        costImage.enabled = true;
    }
    public void Buy() {
        if(plant != null)
            PlayerDataManager.instance.Buy(plant, 1);
    }
}
