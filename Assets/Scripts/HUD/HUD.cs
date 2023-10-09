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
        switch (type)
        { 
            case InfoType.Stage:
                myText.text = string.Format("Stage {0:F0}", GameManager.Instance.stageIndex+1);
                break;
            case InfoType.Exp:
                float curExp = GameManager.Instance.playerStatus.exp;
                float maxExp = GameManager.Instance.playerStatus.nextExp[GameManager.Instance.playerStatus.level];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.Instance.playerStatus.level);
                break;
            case InfoType.MagicStone:
                myText.text = string.Format("{0:F0}", GameManager.Instance.playerStatus.magicalStone);
                break;
            case InfoType.Time:
                float remainTime = GameManager.Instance.maxGameTime - GameManager.Instance.gameTime;
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
                float curHealth = GameManager.Instance.playerStatus.HP;
                float maxHealth = GameManager.Instance.playerStatus.MaxHP;
                mySlider.value = curHealth / maxHealth;
                break;
        }
    }
}
