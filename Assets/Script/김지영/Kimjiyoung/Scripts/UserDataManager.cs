using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

[System.Serializable]
public class UserData
{
    public int Coin = 1000; // ���� �ʱ� ���� ��
    public int Level = 1; // ���� �ʱ� ���� ��
    public float Exp = 0; // ���� �ʱ� ����ġ ��
}

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance;
    public UserData userData;

    public TextMeshProUGUI[] userCoinTexts;
    public TextMeshProUGUI userLevel;

    private string userDataFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }        

        // UserData ���� ��� ���� (PlayerData ����)
        string userDataFolderPath = Path.Combine(Application.persistentDataPath, "UserData");

        // ���� PlayerData ������ ���ٸ� ����
        if (!Directory.Exists(userDataFolderPath))
        {
            Directory.CreateDirectory(userDataFolderPath);
        }

        // UserData ���� ��� ���� (UserData ���� �ȿ�)
        userDataFilePath = Path.Combine(userDataFolderPath, "userData.json");
        // UserData �ε�
        LoadUserData();
    }

    private void LoadUserData()
    {
        if (File.Exists(userDataFilePath))
        {
            string json = File.ReadAllText(userDataFilePath);
            userData = JsonUtility.FromJson<UserData>(json);
        }
        else
        {
            // ������ ������ �⺻ ������ �ʱ�ȭ
            userData = new UserData();
            SaveUserData();
        }
        UpdateUI(); // UI ������Ʈ
    }

    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(userData);
        File.WriteAllText(userDataFilePath, json);
    }

    public void UpdateUI()
    {
        foreach (var text in userCoinTexts)
        {
            text.text = userData.Coin.ToString();
        }
        userLevel.text = userData.Level.ToString();
    }

    // ���� �߰� �� ��� �޼���
    public bool ModifyCoin(int amount)
    {
        if (userData.Coin + amount > 0)
        {
            userData.Coin += amount;
            SaveUserData();
            UpdateUI();
            return true;
        }
        else
        {
            return false;
        }
    }

}
