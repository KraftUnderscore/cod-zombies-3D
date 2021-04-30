using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int totalScore = 0;
    private UIManager uIManager;

    private void Awake()
    {
        if (instance == null) instance = this;
        else DestroyImmediate(gameObject);
    }

    private void Start()
    {
        uIManager = GetComponent<UIManager>();
    }

    public void IncreaseScore(int amount)
    {
        totalScore += amount;
        uIManager.UpdateScoreText(totalScore);
    }
}
