using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SoundSettings
{
    public float soundSensitivity = 100f; // 사운드 볼륨 기본값
}

public class SoundSettingsManager : MonoBehaviour
{
    public static SoundSettingsManager Instance;
    public SoundSettings soundSettings;

    #region private
    private string settingsFilePath;

    // Soun Min ~ Max
    private const float MinSoundSensitivity = 0;
    private const float MaxSoundSensitivity = 100;

    private float previousValiValue; // 이전 유효한 값 저장용 변수
    private bool isSliderChinging = false; // 슬라이더 값 변경 중 여부를 확인하는 변수
    #endregion

    #region UI Contect
    // UI 요소 연결
    public Slider soundSlider;
    public TMP_InputField soundInputField;
    public TextMeshProUGUI soundValueText;
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
        settingsFilePath = Path.Combine(Application.persistentDataPath, "soundSettings.json");

        // 설정 불러오기
        LoadSettings();
    }


    private void Start()
    {
        // 초기 설정값 적용
        soundSlider.minValue = MinSoundSensitivity;
        soundSlider.maxValue = MaxSoundSensitivity;
        soundSlider.value = soundSettings.soundSensitivity;
        soundInputField.text = soundSettings.soundSensitivity.ToString("0.00"); // 초기 설정값을 Input Field에 표시
        UpdateSoundText(soundSettings.soundSensitivity);

        // Slider의 값이 변경될 때 호출되는 이벤트 리스너 등록
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);

        // Input Field의 값이 변경될 때 호출되는 이벤트 리스너 등록
        soundInputField.onEndEdit.AddListener(OnSoundInputChange);

        // 초기 설정 값으로 초기화
        previousValiValue = soundSettings.soundSensitivity;
    }

    public void SaveSettings()
    {
        // 설정값 저장
        soundSettings.soundSensitivity = soundSlider.value;

        string json = JsonUtility.ToJson(soundSettings);
        File.WriteAllText(settingsFilePath, json);
    }

    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string json = File.ReadAllText(settingsFilePath);
            soundSettings = JsonUtility.FromJson<SoundSettings>(json);
        }
        else
        {
            // 파일이 없을 경우 초기 설정값을 사용
            soundSettings = new SoundSettings();
        }
    }

    public void OnSoundSliderChanged(float value)
    {
        if (!isSliderChinging)
        {
            // Slider 값 변경 중임을 표시
            isSliderChinging = true;

            float newValue = Mathf.Clamp(value, MinSoundSensitivity, MaxSoundSensitivity);

            // 이전 유효한 값 저장
            previousValiValue = newValue;

            // 설정값 업데이트
            soundSettings.soundSensitivity = newValue;
            UpdateSoundText(newValue);

            // Slider 값 업데이트
            soundSlider.value = newValue;

            // Input Field 값 업데이트
            soundInputField.text = newValue.ToString("0.00");

            // 설정 값 실시간으로 저장
            SaveSettings();

            // Slider 변경 완료
            isSliderChinging = false;
        }
    }

    public void OnSoundInputChange(string value)
    {
        float parsedValue;
        if (float.TryParse(value, out parsedValue))
        {
            if (parsedValue >= MinSoundSensitivity && parsedValue <= MaxSoundSensitivity)
            {
                // 이전 유효한 값 저장
                previousValiValue = parsedValue;

                // 설정값 업데이트
                soundSettings.soundSensitivity = parsedValue;
                UpdateSoundText(parsedValue);

                // Slider 값 업데이트
                soundSlider.value = parsedValue;
                
                // Input Field 값 업데이트 (소수점 두 번째 자리까지 표시)
                soundInputField.text = parsedValue.ToString("0.00");

                // 설정값 실시간으로 저장
                SaveSettings();
            }
            else
            {
                // 범위를 벗어난 경우 이전 유효 값으로 복구
                soundInputField.text = previousValiValue.ToString("0.00");

                // Slider도 이전 유효 값으로 복구
                soundSlider.value = previousValiValue;
            }
        }
        else
        {
            // 유효하지 않은 입력일 경우 이전 유효한 값으로 복구
            soundInputField.text = previousValiValue.ToString("0.00");

            // Slider도 이전 유효한 값으로 설정
            soundSlider.value = previousValiValue;
        }
    }

    private void UpdateSoundText(float value)
    {
        soundValueText.text = string.Format("Sound Volume: {0}", value.ToString("0"));
    }

    private void OnDisable()
    {
        SaveSettings();
    }
}
