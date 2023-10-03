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
    public TMP_Text UIhealth;
    public int health = 10;

    [Header("# 스테이지 데이터")]
    public GameObject[] Stages;
    public TMP_Text UIStage;
    public int stageIndex;
    public TMP_Text UIStageTime;
    public float maxGameTime;
    public float gameTime;

    [Header("# 포인트")]
    [SerializeField] TMP_Text UIPoint;
    public int totalPoint;
    public int stagePoint;

    [Header("# GUI")]
    [SerializeField] GameObject UIRestartBtn;

    [Header("# 피격 마테리얼(Material)")]
    public Material EnemyDefaultMaterial;
    public Material EnemyDamagedMaterial;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
        UIhealth.text = (health).ToString();
    }

    public void NextStage()
    {
        // Change Stage
        if (stageIndex < Stages.Length-1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "STAGE" + (stageIndex + 1).ToString();
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
        

        // Calculate Point
        totalPoint += stagePoint;
        stagePoint = 0;
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
            HealthDown(99);

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

    public void setStageTime(float stageTime)
    {
        UIStageTime.text = Mathf.Floor(stageTime).ToString();
    }
}
