using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("# 전역 GameObject")]
    public Player player;
    public PoolManager pool;

    [Header("# 플레이어 데이터")]
    public int level;
    public int kill;
    public float exp;
    public int[] nextExp = { 10, 20, 30, 50, 80, 120, 180, 250, 320, 500, 640, 800, 1200, 1500, 2000, 3000, 4500, 6000, 10000, 15000, 21000, 30000 };
    public int health;
    public int maxHealth;

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

    public void HealthDown(int point = 1)
    {
        if (health > 0)
        {
            health -= point;
        }

        if(health <= 0)
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
        exp++;

        if (exp >= nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
