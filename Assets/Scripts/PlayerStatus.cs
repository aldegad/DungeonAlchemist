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
    //public Dicti
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
}