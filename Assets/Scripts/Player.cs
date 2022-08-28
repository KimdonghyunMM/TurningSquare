using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.gameObject.GetComponent<EnemyController>();

        if (enemy.isCount)
        {
            GameManager.instance._score.Value++;
            Debug.Log("������ ȹ���߼�.");
        }
        else
        {
            PlayerPrefs.SetInt("bestscore", GameManager.instance._bestScoreInteger);
            Time.timeScale = 0f;
            GameManager.instance._gameOver.Value = true;
            Debug.Log("������ ����Ƽ�.");
        }

        EnemyPullManager.instance.DestroyCube(enemy);
    }

}
