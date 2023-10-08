using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // basic set
        name = "Gear " + data.itemId;
        transform.parent = GameManager.Instance .transform;
        transform.localPosition = Vector3.zero;

        // property set
        type = data.itemType;
        rate = data.damages[0];
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
    }

    void RateUp()
    { 
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    // weapon.attackSpeed = 
                    break;
            }
        }
    }
}
