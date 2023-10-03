using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    // Start is called before the first frame update
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        // ���� ��ǥ�� ī�޶� ���� ��ũ�� ��ǥ�� �����ϴ� �Լ�
        rect.position = Camera.main.WorldToScreenPoint(GameManager.Instance.player.transform.position);
    }
}
