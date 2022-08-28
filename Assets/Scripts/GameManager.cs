using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int _bestScoreInteger;

    public Text _curScore, _bestScore;
    [SerializeField] private GameObject _gameObject;

    [HideInInspector] public IntReactiveProperty _score = new IntReactiveProperty(0);
    [HideInInspector] public BoolReactiveProperty _gameOver = new BoolReactiveProperty(false);
    private List<IDisposable> _subject = new List<IDisposable>();
    private void Awake()
    {
        if (instance == null)
            instance = this;

        _subject.Add(_score.TakeUntilDestroy(this).Subscribe(x =>
        {
            _curScore.text = $"Score : {x}";
            if (x >= PlayerPrefs.GetInt("bestscore"))
            {
                _bestScore.text = $"BestScore : {x}";
                _bestScoreInteger = x;
            }
        }));

        _subject.Add(_gameOver.TakeUntilDestroy(this).Subscribe(_gameObject.SetActive));
    }

    private void Start()
    {
        _bestScore.text = $"BestScore : {PlayerPrefs.GetInt("bestscore")}";
    }

    private void Update()
    {
        if (_gameOver.Value)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1f;
                _gameOver.Value = false;
            }
        }
    }

    private void OnDestroy()
    {
        if (_subject != null)
        {
            for(var i = 0; i < _subject.Count; i++)
            {
                _subject[i].Dispose();
            }
        }
    }
}
