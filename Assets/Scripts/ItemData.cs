using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Gear, Heal }

    [Header("# 메인 정보")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;

    [Header("# 레벨 데이터")]
    public float baseDamage;
    public int baseCount;
    public int basePenetration;
    public float baseAttackSpeed;

    public float[] damages;
    public int[] counts;
    public int[] penetrations;
    public float[] attackSpeed;

    [Header("# 투사체")]
    public GameObject projectile;
}
