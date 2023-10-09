using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Stage, Exp, Level, MagicStone, Time, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        PlayerStatus playerStatus = GameManager.Instance.playerStatus;
        StageManager stageManager = GameManager.Instance.stageManager;

        if (!playerStatus || !stageManager)
            return;

        switch (type)
        {
            case InfoType.Stage:
                myText.text = string.Format("Stage {0:F0}", GameManager.Instance.stageIndex+1);
                break;
            case InfoType.Exp:
                float curExp = playerStatus.exp;
                float maxExp = playerStatus.nextExp[playerStatus.level];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", playerStatus.level);
                break;
            case InfoType.MagicStone:
                myText.text = string.Format("{0:F0}", playerStatus.magicalStone);
                break;
            case InfoType.Time:
                float remainTime = stageManager.maxStageTime - stageManager.stageTime;
                if (remainTime >= 0)
                {
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                }
                else
                {
                    float minusTime = Mathf.Abs(remainTime);
                    int min = Mathf.FloorToInt(minusTime / 60);
                    int sec = Mathf.FloorToInt(minusTime % 60);
                    myText.text = string.Format("-{0:D2}:{1:D2}", min, sec);
                    myText.color = Color.red;
                }
                break;
            case InfoType.Health:
                float curHealth = playerStatus.HP;
                float maxHealth = playerStatus.MaxHP;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
