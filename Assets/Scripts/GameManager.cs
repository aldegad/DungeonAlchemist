using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float gameTime;
    public float maxGameTime = 2 * 10f;

    public Player player;
    public PoolManager pool;
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health = 10;

    [SerializeField] GameObject[] Stages;

    [SerializeField] TMP_Text UIStageTime;
    [SerializeField] TMP_Text UIhealth;
    [SerializeField] TMP_Text UIPoint;
    [SerializeField] TMP_Text UIStage;
    [SerializeField] GameObject UIRestartBtn;

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
