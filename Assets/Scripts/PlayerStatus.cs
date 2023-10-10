using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    [Header("# �÷��̾� �⺻ ������")]
    public Sprite levelSprite;
    public int level = 1;
    public int levelUpInStage = 0;
    public int kill;
    public float exp;
    public int[] nextExp = { 10, 20, 30, 50, 80, 120, 180, 250, 320, 500, 640, 800, 1200, 1500, 2000, 3000, 4500, 6000, 10000, 15000, 21000, 30000 };
    public int magicalStone = 0;

    [Header("# �÷��̾� �ɷ�ġ")]
    public Sprite MaxHPSprite;
    public int MaxHP = 20;
    public int HP = 20;
    public string MaxHPText = "HP";
    public Sprite HPReGenerationSprite;
    public int HPReGeneration = 1;
    public string HPReGenerationText = "�ʴ� HP ȸ��";
    public Sprite MaxMPSprite;
    public int MaxMP = 20;
    public int MP = 20;
    public string MaxMPText = "MP";
    public Sprite MPReGenerationSprite;
    public int MPReGeneration = 1;
    public string MPReGenerationText = "�ʴ� MP ȸ��";
    public Sprite IntelligenceSprite;
    public int Intelligence = 1;
    public string IntelligenceText = "���� ���ݷ�";
    public Sprite StrengthSprite;
    public int Strength = 1;
    public string StrengthText = "���� ���ݷ�";
    public Sprite AgilitySprite;
    public int Agility = 1;
    public string AgilityText = "���� �� ���� ���� �ӵ�";
    public Sprite CharismaSprite;
    public int Charisma = 1;
    public string CharismaText = "��ȯ�� ���ݷ�";
    public Sprite PhysicalDefenseSprite;
    public int PhysicalDefense;
    public string PhysicalDefenseText = "���� ����";
    public Sprite MagicDefenseSprite;
    public int MagicDefense;
    public string MagicDefenseText = "���� ����";

    [Header("# �÷��̾� ������ �� ���� ������ �ɷ�ġ")]
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
        // ������ �ϸ�, ������ ������, exp�� �ʱ�ȭ �ȴ�.
        level++;
        exp = 0;

        // ������ ��, hp�� mp�� 1�� ��� ������.
        MaxHP++;
        HP++;
        MaxMP++;
        MP++;

        // ���������� ����Ǹ�, ���� ������������ ������ �� �� ��ŭ �������ͽ� ī�带 �̴´�.
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