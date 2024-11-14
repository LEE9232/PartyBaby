using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Unity.VisualScripting.AssemblyQualifiedNameParser;



[System.Serializable]
public class MouseSettings
{
    public float mouseSensitivity = 50.00f; // 마우스 감도 기본값
}

public class MouseSettingsManager : MonoBehaviour
{
    public static MouseSettingsManager Instance;
    public MouseSettings mouseSettings;

    #region private
    private string settingsFilePath;

    private const float MinMouseSensitivity = 0.1f;
    private const float MaxMouseSensitivity = 100f;

    private float previousValidValue; // 이전 유효한 값 저장용 변수
    private bool isSliderChainging = false; // 슬라이더 값 변경 중 여부를 확인하는 변수
    #endregion

    #region UI Contect
    public Slider mouseSlider;
    public TMP_InputField mouseInputField;
    public TextMeshProUGUI mouseValueText;
    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 설정 파일 경로 설정
        settingsFilePath = Path.Combine(Application.persistentDataPath, "mouseSettings.json");

        // 설정 불러오기
        LoadSettings();
    }


    private void Start()
    {
        // 초기 설정값 적용
        mouseSlider.minValue = MinMouseSensitivity;
        mouseSlider.maxValue = MaxMouseSensitivity;
        mouseSlider.value = mouseSettings.mouseSensitivity;
        mouseInputField.text = mouseSettings.mouseSensitivity.ToString("0.00"); // 초기 설정값을 Input Field에 표시
        UpdateMouseText(mouseSettings.mouseSensitivity);

        // Slider의 값이 변경될 때 호출되는 이벤트 리스너 등록
        mouseSlider.onValueChanged.AddListener(OnMouseSliderChanged);

        // Input Field의 값이 변경될 때 호출되는 이벤트 리스너 등록
        mouseInputField.onEndEdit.AddListener(OnMouseInputChange);

        previousValidValue = mouseSettings.mouseSensitivity; // 초기 설정 값으로 초기화
    }

    public void SaveSettings()
    {
        // 설정값 저장
        mouseSettings.mouseSensitivity = mouseSlider.value;

        string json = JsonUtility.ToJson(mouseSettings);
        File.WriteAllText(settingsFilePath, json);
    }

    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            mouseSettings = JsonUtility.FromJson<MouseSettings>(json);
        }
        else
        {
            // 파일이 없을 경우 초기 설정값을 사용
            mouseSettings = new MouseSettings();
        }
    }

    // Slider 값 변경 함수
    public void OnMouseSliderChanged(float value)
    {
        if (!isSliderChainging)
        {
            // Slider 값 변경 중임을 표시
            isSliderChainging = true;

            float newValue = Mathf.Clamp(value, MinMouseSensitivity, MaxMouseSensitivity);

            // 이전 유효한 값 저장
            previousValidValue = newValue;

            // 설정값 업데이트
            mouseSettings.mouseSensitivity = newValue;
            UpdateMouseText(newValue);

            // Slider 값 업데이트
            mouseSlider.value = newValue;

            // Input Field 값 업데이트
            mouseInputField.text = newValue.ToString("0.00");

            // 설정 값 실시간으로 저장
            SaveSettings();

            // Slider 변경 완료
            isSliderChainging = false;
        }
    }

    public void OnMouseInputChange(string value)
    {
        float parsedValue;
        if (float.TryParse(value, out parsedValue))
        {
            // 입력된 값이 유효한 범위에 속하는지 확인
            if (parsedValue >= MinMouseSensitivity && parsedValue <= MaxMouseSensitivity)
            {
                // 이전 유효한 값 저장
                previousValidValue = parsedValue;

                // 설정값 업데이트
                mouseSettings.mouseSensitivity = parsedValue;
                UpdateMouseText(parsedValue);

                // Slider 값 업데이트
                mouseSlider.value = parsedValue;

                // Input Field 값 업데이트 (소수점 두 번째 자리까지 표시)
                mouseInputField.text = parsedValue.ToString("0.00");

                // 설정값 실시간으로 저장
                SaveSettings();
            }
            else
            {
                // 범위를 벗어난 경우 이전 유효한 값으로 복구
                mouseInputField.text = previousValidValue.ToString("0.00");

                // Slider도 이전 유효한 값으로 설정
                mouseSlider.value = previousValidValue;
            }
        }
        else
        {
            // 유효하지 않은 입력일 경우 이전 유효한 값으로 복구
            mouseInputField.text = previousValidValue.ToString("0.00");

            // Slider도 이전 유효한 값으로 설정
            mouseSlider.value = previousValidValue;
        }
    }


    private void UpdateMouseText(float value)
    {
        mouseValueText.text = string.Format("Mouse Sensitivity: {0:0.00}", value);
    }

    private void OnDisable()
    {
        SaveSettings();
    }

}
