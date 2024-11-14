using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject targetObject; // AudioSource�� ���� ��� ���� ������Ʈ
    private AudioSource targetAudioSource;

    [Range(0f, 1f)]
    public float initialVolume = 0.2f; // �ʱ� ������, Inspector���� ���� ����

    private const string VolumePrefsKey = "TargetObjectVolume";

    private void Start()
    {
        // ��� ������Ʈ���� AudioSource ������Ʈ�� �����ɴϴ�
        if (targetObject != null)
        {
            targetAudioSource = targetObject.GetComponent<AudioSource>();
            if (targetAudioSource == null)
            {
                Debug.LogError("Target object does not have an AudioSource component!");
                return;
            }
        }
        else
        {
            Debug.LogError("Target object is not assigned!");
            return;
        }

        // PlayerPrefs���� ����� ������ �������ų� �ʱⰪ�� ����մϴ�
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefsKey, initialVolume);

        // �����̴� ����
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = savedVolume;

        // ������ �߰�
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        // �ʱ� ���� ����
        ChangeVolume(volumeSlider.value);
    }

    private void ChangeVolume(float volume)
    {
        if (targetAudioSource != null)
        {
            targetAudioSource.volume = volume;
            PlayerPrefs.SetFloat(VolumePrefsKey, volume);
            PlayerPrefs.Save();
        }
    }

    // ������ �ʱⰪ���� �����ϴ� �޼���
    public void ResetVolumeToInitial()
    {
        volumeSlider.value = initialVolume;
        ChangeVolume(initialVolume);
    }
}
