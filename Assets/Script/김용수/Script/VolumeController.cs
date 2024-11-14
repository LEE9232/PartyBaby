using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;
    public GameObject targetObject; // AudioSource를 가진 대상 게임 오브젝트
    private AudioSource targetAudioSource;

    [Range(0f, 1f)]
    public float initialVolume = 0.2f; // 초기 볼륨값, Inspector에서 조절 가능

    private const string VolumePrefsKey = "TargetObjectVolume";

    private void Start()
    {
        // 대상 오브젝트에서 AudioSource 컴포넌트를 가져옵니다
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

        // PlayerPrefs에서 저장된 볼륨을 가져오거나 초기값을 사용합니다
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefsKey, initialVolume);

        // 슬라이더 설정
        volumeSlider.minValue = 0f;
        volumeSlider.maxValue = 1f;
        volumeSlider.value = savedVolume;

        // 리스너 추가
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        // 초기 볼륨 설정
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

    // 볼륨을 초기값으로 리셋하는 메서드
    public void ResetVolumeToInitial()
    {
        volumeSlider.value = initialVolume;
        ChangeVolume(initialVolume);
    }
}
