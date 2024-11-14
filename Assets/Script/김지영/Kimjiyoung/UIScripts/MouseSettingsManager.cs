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
    public float mouseSensitivity = 50.00f; // ���콺 ���� �⺻��
}

public class MouseSettingsManager : MonoBehaviour
{
    public static MouseSettingsManager Instance;
    public MouseSettings mouseSettings;

    #region private
    private string settingsFilePath;

    private const float MinMouseSensitivity = 0.1f;
    private const float MaxMouseSensitivity = 100f;

    private float previousValidValue; // ���� ��ȿ�� �� ����� ����
    private bool isSliderChainging = false; // �����̴� �� ���� �� ���θ� Ȯ���ϴ� ����
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

        // ���� ���� ��� ����
        settingsFilePath = Path.Combine(Application.persistentDataPath, "mouseSettings.json");

        // ���� �ҷ�����
        LoadSettings();
    }


    private void Start()
    {
        // �ʱ� ������ ����
        mouseSlider.minValue = MinMouseSensitivity;
        mouseSlider.maxValue = MaxMouseSensitivity;
        mouseSlider.value = mouseSettings.mouseSensitivity;
        mouseInputField.text = mouseSettings.mouseSensitivity.ToString("0.00"); // �ʱ� �������� Input Field�� ǥ��
        UpdateMouseText(mouseSettings.mouseSensitivity);

        // Slider�� ���� ����� �� ȣ��Ǵ� �̺�Ʈ ������ ���
        mouseSlider.onValueChanged.AddListener(OnMouseSliderChanged);

        // Input Field�� ���� ����� �� ȣ��Ǵ� �̺�Ʈ ������ ���
        mouseInputField.onEndEdit.AddListener(OnMouseInputChange);

        previousValidValue = mouseSettings.mouseSensitivity; // �ʱ� ���� ������ �ʱ�ȭ
    }

    public void SaveSettings()
    {
        // ������ ����
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
            // ������ ���� ��� �ʱ� �������� ���
            mouseSettings = new MouseSettings();
        }
    }

    // Slider �� ���� �Լ�
    public void OnMouseSliderChanged(float value)
    {
        if (!isSliderChainging)
        {
            // Slider �� ���� ������ ǥ��
            isSliderChainging = true;

            float newValue = Mathf.Clamp(value, MinMouseSensitivity, MaxMouseSensitivity);

            // ���� ��ȿ�� �� ����
            previousValidValue = newValue;

            // ������ ������Ʈ
            mouseSettings.mouseSensitivity = newValue;
            UpdateMouseText(newValue);

            // Slider �� ������Ʈ
            mouseSlider.value = newValue;

            // Input Field �� ������Ʈ
            mouseInputField.text = newValue.ToString("0.00");

            // ���� �� �ǽð����� ����
            SaveSettings();

            // Slider ���� �Ϸ�
            isSliderChainging = false;
        }
    }

    public void OnMouseInputChange(string value)
    {
        float parsedValue;
        if (float.TryParse(value, out parsedValue))
        {
            // �Էµ� ���� ��ȿ�� ������ ���ϴ��� Ȯ��
            if (parsedValue >= MinMouseSensitivity && parsedValue <= MaxMouseSensitivity)
            {
                // ���� ��ȿ�� �� ����
                previousValidValue = parsedValue;

                // ������ ������Ʈ
                mouseSettings.mouseSensitivity = parsedValue;
                UpdateMouseText(parsedValue);

                // Slider �� ������Ʈ
                mouseSlider.value = parsedValue;

                // Input Field �� ������Ʈ (�Ҽ��� �� ��° �ڸ����� ǥ��)
                mouseInputField.text = parsedValue.ToString("0.00");

                // ������ �ǽð����� ����
                SaveSettings();
            }
            else
            {
                // ������ ��� ��� ���� ��ȿ�� ������ ����
                mouseInputField.text = previousValidValue.ToString("0.00");

                // Slider�� ���� ��ȿ�� ������ ����
                mouseSlider.value = previousValidValue;
            }
        }
        else
        {
            // ��ȿ���� ���� �Է��� ��� ���� ��ȿ�� ������ ����
            mouseInputField.text = previousValidValue.ToString("0.00");

            // Slider�� ���� ��ȿ�� ������ ����
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
