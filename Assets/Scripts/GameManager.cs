using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# ���� GameObject")]
    public PlayerManager player;
    public PlayerStatus playerStatus;
    public PoolManager pool;

    [Header("# �������� ������")]
    public GameObject[] Stages;
    public TMP_Text UIStage;
    public int stageIndex;
    public float maxGameTime;
    public float gameTime;

    [Header("# GUI")]
    [SerializeField] GameObject UIRestartBtn;

    [Header("# �ǰ� ���׸���(Material)")]
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
        // ���������� Ŭ�����ϸ�, ���������� �ִ� ��� ���� �״´�.
        GameManager.Instance.pool.BroadcastMessage("Dead", SendMessageOptions.DontRequireReceiver);

        // ���� ������ ��� ������ �÷��̾�� ����ȴ�.

        // ������ GUI�� �����ǰ�, ���� ������������ �������� ������ŭ ī�带 �̴´�.

        // ������������ ���� ���� ������ŭ �������� �̴´�.

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
