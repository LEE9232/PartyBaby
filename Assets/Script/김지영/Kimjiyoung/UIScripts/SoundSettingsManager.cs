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
    public float soundSensitivity = 100f; // ���� ���� �⺻��
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

    private float previousValiValue; // ���� ��ȿ�� �� ����� ����
    private bool isSliderChinging = false; // �����̴� �� ���� �� ���θ� Ȯ���ϴ� ����
    #endregion

    #region UI Contect
    // UI ��� ����
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

        // ���� ���� ��� ����
        settingsFilePath = Path.Combine(Application.persistentDataPath, "soundSettings.json");

        // ���� �ҷ�����
        LoadSettings();
    }


    private void Start()
    {
        // �ʱ� ������ ����
        soundSlider.minValue = MinSoundSensitivity;
        soundSlider.maxValue = MaxSoundSensitivity;
        soundSlider.value = soundSettings.soundSensitivity;
        soundInputField.text = soundSettings.soundSensitivity.ToString("0.00"); // �ʱ� �������� Input Field�� ǥ��
        UpdateSoundText(soundSettings.soundSensitivity);

        // Slider�� ���� ����� �� ȣ��Ǵ� �̺�Ʈ ������ ���
        soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);

        // Input Field�� ���� ����� �� ȣ��Ǵ� �̺�Ʈ ������ ���
        soundInputField.onEndEdit.AddListener(OnSoundInputChange);

        // �ʱ� ���� ������ �ʱ�ȭ
        previousValiValue = soundSettings.soundSensitivity;
    }

    public void SaveSettings()
    {
        // ������ ����
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
            // ������ ���� ��� �ʱ� �������� ���
            soundSettings = new SoundSettings();
        }
    }

    public void OnSoundSliderChanged(float value)
    {
        if (!isSliderChinging)
        {
            // Slider �� ���� ������ ǥ��
            isSliderChinging = true;

            float newValue = Mathf.Clamp(value, MinSoundSensitivity, MaxSoundSensitivity);

            // ���� ��ȿ�� �� ����
            previousValiValue = newValue;

            // ������ ������Ʈ
            soundSettings.soundSensitivity = newValue;
            UpdateSoundText(newValue);

            // Slider �� ������Ʈ
            soundSlider.value = newValue;

            // Input Field �� ������Ʈ
            soundInputField.text = newValue.ToString("0.00");

            // ���� �� �ǽð����� ����
            SaveSettings();

            // Slider ���� �Ϸ�
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
                // ���� ��ȿ�� �� ����
                previousValiValue = parsedValue;

                // ������ ������Ʈ
                soundSettings.soundSensitivity = parsedValue;
                UpdateSoundText(parsedValue);

                // Slider �� ������Ʈ
                soundSlider.value = parsedValue;
                
                // Input Field �� ������Ʈ (�Ҽ��� �� ��° �ڸ����� ǥ��)
                soundInputField.text = parsedValue.ToString("0.00");

                // ������ �ǽð����� ����
                SaveSettings();
            }
            else
            {
                // ������ ��� ��� ���� ��ȿ ������ ����
                soundInputField.text = previousValiValue.ToString("0.00");

                // Slider�� ���� ��ȿ ������ ����
                soundSlider.value = previousValiValue;
            }
        }
        else
        {
            // ��ȿ���� ���� �Է��� ��� ���� ��ȿ�� ������ ����
            soundInputField.text = previousValiValue.ToString("0.00");

            // Slider�� ���� ��ȿ�� ������ ����
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
