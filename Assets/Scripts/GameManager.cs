using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# 전역 GameObject")]
    public PlayerManager player;
    public PlayerStatus playerStatus;
    public PoolManager pool;

    [Header("# 스테이지 데이터")]
    public GameObject[] Stages;
    public int stageIndex;
    public StageManager stageManager;

    [Header("# GUI")]
    public GameObject UILevelUp;
    [SerializeField] GameObject UIRestartBtn;

    [Header("# 피격 마테리얼(Material)")]
    public Material EnemyDefaultMaterial;
    public Material EnemyDamagedMaterial;

    void Awake()
    {
        Instance = this;
        // tempt
        SetStage(0);
    }

    public void SetStage(int stageIndex)
    {
        // Change Stage
        if (stageIndex < Stages.Length)
        {
            if (stageIndex > 0)
            {
                Stages[stageIndex-1].SetActive(false);
            }
            stageManager = Stages[stageIndex].GetComponentInChildren<StageManager>();
            Stages[stageIndex].SetActive(true);
        }
        else
        {
            // Game Clear
            // Player Control Lock
            Time.timeScale = 0;
            // Reset UI
            
            TMP_Text btnText = UIRestartBtn.GetComponentInChildren<TMP_Text>();
            btnText.text = "Game Clear";
            UIRestartBtn.SetActive(true);
        }
    }

    public void StageClear()
    {
        // 스테이지를 클리어하면, 스테이지에 있는 모든 몹이 죽는다.
        // 땅에 떨어진 모든 마석이 플레이어에게 습득된다.(레이어를 무시하고 달려온다.) => MagicalStoneScript
        stageManager.isStageClear = true;

        // 레벨업 GUI가 생성되고, 현재 스테이지에서 레벨업한 갯수만큼 카드를 뽑는다.
        StartCoroutine(OpenLevelUpUI());

        // 스테이지에서 얻은 상자 갯수만큼 아이템을 뽑는다.

    }
    IEnumerator OpenLevelUpUI()
    {
        yield return new WaitForSeconds(2f);
        // playerStatus.
        UILevelUp.SetActive(true);
    }

    public void HealthDown(int point = 1)
    {
        if (playerStatus.HP > 0)
        {
            playerStatus.HP -= point;
        }

        if(playerStatus.HP <= 0)
        {
            player.OnDie();
            UIRestartBtn.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Health Down
            HealthDown(99999);

            PlayerReposition();
            collision.transform.position = new Vector2(0,0);
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector2(0, 0);
        player.VelocityZero();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void GetExp()
    {
        playerStatus.exp++;

        if (playerStatus.exp >= playerStatus.nextExp[playerStatus.level])
        {
            playerStatus.PlayerLevelUp();
        }
    }

    public void GetMagicalStone()
    {
        playerStatus.magicalStone++;
    }
}
