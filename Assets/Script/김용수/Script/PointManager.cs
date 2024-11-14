using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    private const int INITIAL_POINTS = 1000;
    private const string POINTS_KEY = "PlayerPoints";

    private static PointManager _instance;
    public static PointManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PointManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("PointManager");
                    _instance = go.AddComponent<PointManager>();
                }
            }
            return _instance;
        }
    }

    private int currentPoints;

    private void Awake()
    {
        // �̱��� ������ �����ϸ�, �� ���� �����͸� �������� ����
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            LoadPoints();
        }
    }

    private void LoadPoints()
    {
        currentPoints = PlayerPrefs.GetInt(POINTS_KEY, INITIAL_POINTS);
    }

    public void SavePoints()
    {
        PlayerPrefs.SetInt(POINTS_KEY, currentPoints);
        PlayerPrefs.Save();
    }

    public int GetPoints()
    {
        return currentPoints;
    }

    public void SetPoints(int points)
    {
        currentPoints = points;
    }

    public bool SpendPoints(int amount)
    {
        if (currentPoints >= amount)
        {
            currentPoints -= amount;
            SavePoints();
            return true;
        }
        return false;
    }

    public void AddPoints(int amount)
    {
        currentPoints += amount;
        SavePoints();
    }

    public void ResetPoints()
    {
        currentPoints = INITIAL_POINTS;
        SavePoints();
    }
}
