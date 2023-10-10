using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    [Header("# 플레이어 기본 데이터")]
    public Sprite levelSprite;
    public int level = 1;
    public int levelUpInStage = 0;
    public int kill;
    public float exp;
    public int[] nextExp = { 10, 20, 30, 50, 80, 120, 180, 250, 320, 500, 640, 800, 1200, 1500, 2000, 3000, 4500, 6000, 10000, 15000, 21000, 30000 };
    public int magicalStone = 0;

    [Header("# 플레이어 능력치")]
    public Sprite MaxHPSprite;
    public int MaxHP = 20;
    public int HP = 20;
    public string MaxHPText = "HP";
    public Sprite HPReGenerationSprite;
    public int HPReGeneration = 1;
    public string HPReGenerationText = "초당 HP 회복";
    public Sprite MaxMPSprite;
    public int MaxMP = 20;
    public int MP = 20;
    public string MaxMPText = "MP";
    public Sprite MPReGenerationSprite;
    public int MPReGeneration = 1;
    public string MPReGenerationText = "초당 MP 회복";
    public Sprite IntelligenceSprite;
    public int Intelligence = 1;
    public string IntelligenceText = "마법 공격력";
    public Sprite StrengthSprite;
    public int Strength = 1;
    public string StrengthText = "물리 공격력";
    public Sprite AgilitySprite;
    public int Agility = 1;
    public string AgilityText = "마법 및 물리 공격 속도";
    public Sprite CharismaSprite;
    public int Charisma = 1;
    public string CharismaText = "소환수 공격력";
    public Sprite PhysicalDefenseSprite;
    public int PhysicalDefense;
    public string PhysicalDefenseText = "물리 방어력";
    public Sprite MagicDefenseSprite;
    public int MagicDefense;
    public string MagicDefenseText = "마법 방어력";

    [Header("# 플레이어 레벨업 시 습득 가능한 능력치")]
    public int[] MaxHPLevelUp = { 3, 6, 9 };
    public int[] HPReGenerationLevelUp = { 1, 2, 3 };
    public int[] MaxMPLevelUp = { 3, 6, 9 };
    public int[] MPReGenerationLevelUp = { 1, 2, 3 };
    public int[] IntelligenceLevelUp = { 2, 4, 6 };
    public int[] StrengthLevelUp = { 2, 4, 6 };
    public int[] AgilityLevelUp = { 2, 4, 6 };
    public int[] CharismaLevelUp = { 2, 4, 6 };
    public int[] PhysicalDefenseLevelUp = { 2, 4, 6 };
    public int[] MagicDefenseLevelUp = { 2, 4, 6 };

    public void PlayerLevelUp()
    {
        // 레벨업 하면, 레벨이 오르고, exp가 초기화 된다.
        level++;
        exp = 0;

        // 레벨업 시, hp와 mp가 1씩 즉시 오른다.
        MaxHP++;
        HP++;
        MaxMP++;
        MP++;

        // 스테이지가 종료되면, 현재 스테이지에서 레벨업 된 수 만큼 스테이터스 카드를 뽑는다.
        levelUpInStage++;
    }

    public void GetLevelUpCards()
    {
        for (int cardIndex = 0; cardIndex < 4; cardIndex++)
        {
            int statusIndex = Random.Range(0, 10);
            int statusGradeDice = Random.Range(0, 10); // 0 ~ 4 normal, 5 ~ 8 rare, 9 epic
            int statusGradeIndex = 0;
            if (statusGradeDice < 5)
            {
                statusGradeIndex = 0;
            }
            else if (statusGradeDice < 8)
            {
                statusGradeIndex = 1;
            }
            else {
                statusGradeIndex = 2;
            }
        }
    }

    /* Dictionary<string,int> GetLevelUpStatusCard(int statusIndex, int statusGradeIndex)
    {
        Dictionary<string,int> statusCard = new Dictionary<string, int>();
        switch (statusIndex)
        {
            case 0:
                statusCard.Add("status", "HP");
                statusCard.Add("value", MaxHP[]);
                return new Dictionary<string, int>();
            default:
                return 
        }
    } */
}