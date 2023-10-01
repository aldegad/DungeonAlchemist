using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public int health = 10;

    [SerializeField] PlayerMove player;

    // Start is called before the first frame update
    public void NextStage()
    {
        stageIndex++;

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
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Health Down
            HealthDown(99);

            collision.attachedRigidbody.velocity = Vector2.zero;
            collision.transform.position = new Vector2(0,0);
        }
    }


}
