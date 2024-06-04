using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;


    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;


    }

    void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        switch(data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] *100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;


        }

    }



    //버튼을 누르면 레벨 업 되면서 강화되는 함수
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = data.counts[level];  // 레벨마다 총알 수 증가

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += 1;  // 레벨업 할 때마다 총알 수 하나씩 증가

                    weapon.LevelUp(nextDamage, nextCount);
                }
                level++;
                break;
                // 다른 케이스 처리
        }

        // 아이템 최대 레벨 처리
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }



}
