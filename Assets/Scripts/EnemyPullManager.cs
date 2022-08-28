using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPullManager : MonoBehaviour
{
    public static EnemyPullManager instance;

    private Queue<EnemyController> _enemyQueue = new Queue<EnemyController>();
    [SerializeField] private int _enemyQueueCapacity = 6;
    [SerializeField] private bool _autoQueueGrow = true;

    [SerializeField] private GameObject _enemyPref;
    [SerializeField] private GameObject _player;

    public float _minTimeValue, _maxTimeValue, _minSpeedValue, _maxSpeedValue, _fasterInterval;

    private RectTransform rectTransform;
    private float _playTime;

    private void Awake()
    {
        //SingleTon
        if (instance == null)
            instance = this;

        rectTransform = GetComponent<RectTransform>();

        InitializeEnemyQueue();
    }

    private void Start()
    {
        StartCoroutine(CoSpawnEnemy());
    }

    private void Update()
    {
        _playTime += Time.deltaTime;
        if (_playTime >= _fasterInterval)
        {
            if(_minTimeValue > 0.1f)
            {
                _minTimeValue -= Time.deltaTime;
                if (_maxTimeValue > _minTimeValue)
                    _maxTimeValue -= Time.deltaTime;
            }

            if(_minSpeedValue < 10f)
            {
                _minSpeedValue += Time.deltaTime;
                if (_maxSpeedValue > _minSpeedValue)
                    _maxSpeedValue += Time.deltaTime;
            }
            GameManager.instance._score.Value += 10;
            _playTime = 0f;
        }
    }

    private void InitializeEnemyQueue()
    {
        for(var i  = 0; i < _enemyQueueCapacity; i++)
        {
            AddEnemyToQueue();
        }
    }

    private void AddEnemyToQueue()
    {
        var enemy = Instantiate(_enemyPref, Vector2.zero, Quaternion.identity,transform).GetComponent<EnemyController>();
        enemy.gameObject.SetActive(false);
        _enemyQueue.Enqueue(enemy);
    }

    private EnemyController Spawn(Color color, Vector2 pos)
    {
        if(_enemyQueue.Count == 0)
        {
            if (_autoQueueGrow)
            {
                _enemyQueueCapacity++;
                AddEnemyToQueue();
            }
            else
            {
                Debug.LogError("풀에 적을 넣을 수가 업슴");
                return null;
            }
        }

        var enemy = _enemyQueue.Dequeue();
        enemy.transform.position = pos;
        enemy._img.color = color;
        enemy.gameObject.SetActive(true);

        return enemy;
    }

    public void DestroyCube(EnemyController enemy)
    {
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.position = Vector2.zero;
        enemy._tarPos = Vector2.zero;
        enemy._dir = Vector2.zero;
        enemy.isDead = true;
        enemy.isCount = false;
        enemy.gameObject.SetActive(false);
        _enemyQueue.Enqueue(enemy);
    }

    public EnemyController SpawnRnd()
    {
        var rndColor = GenerateRndColor();
        var enemy = Spawn(rndColor, Vector2.zero);
        enemy.isDead = false;
        if (rndColor == Color.red)
            enemy.isCount = true;
        else
            enemy.isCount = false;
        enemy.transform.localPosition = GenerateRndPos();
        enemy._tarPos = (Vector2)_player.transform.position;
        return enemy;
    }

    private Color GenerateRndColor()
    {
        var num = Random.Range(0, 99);
        if (num <= 79)
            return Color.black;
        else
            return Color.red;
    }
    private Vector2 GenerateRndPos()
    {
        return new Vector2(Random.Range(-rectTransform.rect.width / 2, rectTransform.rect.width / 2), 0);
    }

    private IEnumerator CoSpawnEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minTimeValue, _maxTimeValue));
            var enemy = SpawnRnd();
            enemy.StartToTarget(_minSpeedValue, _maxSpeedValue);
        }
    }
}
