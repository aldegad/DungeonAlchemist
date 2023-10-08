using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Gear, Heal }

    [Header("# ���� ����")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;

    [Header("# ���� ������")]
    public float baseDamage;
    public int baseCount;
    public int basePenetration;
    public float baseAttackSpeed;

    public float[] damages;
    public int[] counts;
    public int[] penetrations;
    public float[] attackSpeed;

    [Header("# ����ü")]
    public GameObject projectile;
}
