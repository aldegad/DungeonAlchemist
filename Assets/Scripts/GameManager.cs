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
    public TMP_Text UIStage;
    public int stageIndex;
    public float maxGameTime;
    public float gameTime;

    [Header("# GUI")]
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

    private void Update()
    {
        gameTime += Time.deltaTime;
    }

    public void SetStage(int stageIndex)
    {
        // Change Stage
        if (stageIndex < Stages.Length-1)
        {
            if (stageIndex > 0)
            {
                Stages[stageIndex-1].SetActive(false);
            }

            Stages[stageIndex].SetActive(true);
            StageManager stageManager = Stages[stageIndex].GetComponentInChildren<StageManager>();
            maxGameTime = stageManager.maxStageTime;
            gameTime = 0;

            UIStage.text = "STAGE" + (stageIndex).ToString();
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
        GameManager.Instance.pool.BroadcastMessage("Dead", SendMessageOptions.DontRequireReceiver);

        // 땅에 떨어진 모든 마석이 플레이어에게 습득된다.

        // 레벨업 GUI가 생성되고, 현재 스테이지에서 레벨업한 갯수만큼 카드를 뽑는다.

        // 스테이지에서 얻은 상자 갯수만큼 아이템을 뽑는다.

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
