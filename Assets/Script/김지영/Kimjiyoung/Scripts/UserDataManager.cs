using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System;

[System.Serializable]
public class UserData
{
    public int Coin = 1000; // 유저 초기 코인 값
    public int Level = 1; // 유저 초기 레벨 값
    public float Exp = 0; // 유저 초기 경험치 값
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

        // UserData 파일 경로 설정 (PlayerData 폴더)
        string userDataFolderPath = Path.Combine(Application.persistentDataPath, "UserData");

        // 만약 PlayerData 폴더가 없다면 생성
        if (!Directory.Exists(userDataFolderPath))
        {
            Directory.CreateDirectory(userDataFolderPath);
        }

        // UserData 파일 경로 설정 (UserData 폴더 안에)
        userDataFilePath = Path.Combine(userDataFolderPath, "userData.json");
        // UserData 로드
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
            // 파일이 없으면 기본 값으로 초기화
            userData = new UserData();
            SaveUserData();
        }
        UpdateUI(); // UI 업데이트
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

    // 코인 추가 및 사용 메서드
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
