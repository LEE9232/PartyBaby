using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class Skinmanager : MonoBehaviour
{
    public static Skinmanager Instance { get; private set; }

    public SkinnedMeshRenderer[] faceRenderers;
    public SkinnedMeshRenderer[] bodyRenderers;
    public Material[] faceMaterials;
    public Material[] bodyMaterials;
    public int[] faceSkinCosts;
    public int[] bodySkinCosts;
    public TextMeshProUGUI pointsText;

    public Button[] purchaseFaceButtons;
    public Button[] applyFaceButtons;
    public Button[] purchaseBodyButtons;
    public Button[] applyBodyButtons;

    // Private fields
    private int currentFaceSkinIndex = 0;
    private int currentBodySkinIndex = 0;
    private HashSet<int> purchasedFaceSkins = new HashSet<int>();
    private HashSet<int> purchasedBodySkins = new HashSet<int>();

    // Public properties
    public int CurrentFaceSkinIndex
    {
        get { return currentFaceSkinIndex; }
    }

    public int CurrentBodySkinIndex
    {
        get { return currentBodySkinIndex; }
    }

    [System.Serializable]
    private class SkinData
    {
        public int currentFaceSkinIndex;
        public int currentBodySkinIndex;
        public List<int> purchasedFaceSkins;
        public List<int> purchasedBodySkins;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadSavedData();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdatePointsDisplay();
        UpdateAllSkinButtonsUI();
    }
    private void Update()
    {
        UpdatePointsDisplay();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene {scene.name} loaded in mode {mode}");
        ReinitializeRenderers();
        ApplySavedSkins();
    }

    private void ReinitializeRenderers()
    {
        faceRenderers = GameObject.FindGameObjectsWithTag("FaceRenderer")
                                  .Select(obj => obj.GetComponent<SkinnedMeshRenderer>()).ToArray();
        bodyRenderers = GameObject.FindGameObjectsWithTag("BodyRenderer")
                                  .Select(obj => obj.GetComponent<SkinnedMeshRenderer>()).ToArray();
    }

    private void LoadSavedData()
    {
        if (PlayerPrefs.HasKey("SkinData"))
        {
            string json = PlayerPrefs.GetString("SkinData");
            SkinData data = JsonUtility.FromJson<SkinData>(json);

            currentFaceSkinIndex = data.currentFaceSkinIndex;
            currentBodySkinIndex = data.currentBodySkinIndex;
            purchasedFaceSkins = new HashSet<int>(data.purchasedFaceSkins);
            purchasedBodySkins = new HashSet<int>(data.purchasedBodySkins);
        }
        else
        {
            currentFaceSkinIndex = 0;
            currentBodySkinIndex = 0;
            purchasedFaceSkins = new HashSet<int> { 0 };
            purchasedBodySkins = new HashSet<int> { 0 };
        }

        Debug.Log($"Loaded data: Face: {currentFaceSkinIndex}, Body: {currentBodySkinIndex}, Purchased Face: {string.Join(",", purchasedFaceSkins)}, Purchased Body: {string.Join(",", purchasedBodySkins)}");
    }

    private void SaveData()
    {
        SkinData data = new SkinData
        {
            currentFaceSkinIndex = currentFaceSkinIndex,
            currentBodySkinIndex = currentBodySkinIndex,
            purchasedFaceSkins = new List<int>(purchasedFaceSkins),
            purchasedBodySkins = new List<int>(purchasedBodySkins)
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("SkinData", json);
        PlayerPrefs.Save();
        Debug.Log("Skin data saved.");
    }

    private void UpdateAllSkinButtonsUI()
    {
        for (int i = 0; i < faceMaterials.Length; i++)
        {
            UpdateSkinButtonsUI(true, i);
        }
        for (int i = 0; i < bodyMaterials.Length; i++)
        {
            UpdateSkinButtonsUI(false, i);
        }
    }

    public void UpdateSkinButtonsUI(bool isFace, int index)
    {
        Debug.Log($"UpdateSkinButtonsUI started: isFace={isFace}, index={index}");
        bool isPurchased = IsSkinPurchased(isFace, index);

        if (isFace)
        {
            if (index < purchaseFaceButtons.Length && index < applyFaceButtons.Length)
            {
                purchaseFaceButtons[index].gameObject.SetActive(!isPurchased);
                applyFaceButtons[index].gameObject.SetActive(isPurchased);
                Debug.Log($"Face buttons updated: purchase={!isPurchased}, apply={isPurchased}");
            }
            else
            {
                Debug.LogWarning($"Invalid face button index: {index}");
            }
        }
        else
        {
            if (index < purchaseBodyButtons.Length && index < applyBodyButtons.Length)
            {
                purchaseBodyButtons[index].gameObject.SetActive(!isPurchased);
                applyBodyButtons[index].gameObject.SetActive(isPurchased);
                Debug.Log($"Body buttons updated: purchase={!isPurchased}, apply={isPurchased}");
            }
            else
            {
                Debug.LogWarning($"Invalid body button index: {index}");
            }
        }
    }

    public void PreviewFaceSkin(int index)
    {
        PreviewSkin(true, index);
    }

    public void PreviewBodySkin(int index)
    {
        PreviewSkin(false, index);
    }

    public void PurchaseFaceSkin(int index)
    {
        PurchaseSkin(true, index);
    }

    public void PurchaseBodySkin(int index)
    {
        PurchaseSkin(false, index);
    }

    public void ApplyFaceSkin(int index)
    {
        ApplySkin(true, index);
    }

    public void ApplyBodySkin(int index)
    {
        ApplySkin(false, index);
    }

    public void PreviewSkin(bool isFace, int index)
    {
        if (isFace)
        {
            if (index >= 0 && index < faceMaterials.Length)
            {
                foreach (var renderer in faceRenderers)
                {
                    renderer.material = faceMaterials[index];
                }
            }
        }
        else
        {
            if (index >= 0 && index < bodyMaterials.Length)
            {
                foreach (var renderer in bodyRenderers)
                {
                    renderer.material = bodyMaterials[index];
                }
            }
        }
    }

    public void PurchaseSkin(bool isFace, int index)
    {
        int cost = isFace ? faceSkinCosts[index] : bodySkinCosts[index];
        HashSet<int> purchasedSkins = isFace ? purchasedFaceSkins : purchasedBodySkins;

        if (purchasedSkins.Contains(index))
        {
            Debug.Log("�̹� ������ ��Ų�Դϴ�.");
            return;
        }

        if (PointManager.Instance.SpendPoints(cost))
        {
            purchasedSkins.Add(index);
            SaveData();
            Debug.Log($"��Ų ���� ����! ���� ����Ʈ: {PointManager.Instance.GetPoints()}");
            UpdatePointsDisplay();
            UpdateSkinButtonsUI(isFace, index);
        }
        else
        {
            Debug.Log($"����Ʈ�� �����մϴ�. �ʿ� ����Ʈ: {cost}, ���� ����Ʈ: {PointManager.Instance.GetPoints()}");
        }
    }

    public void ApplySkin(bool isFace, int index)
    {
        HashSet<int> purchasedSkins = isFace ? purchasedFaceSkins : purchasedBodySkins;

        if (purchasedSkins.Contains(index))
        {
            if (isFace)
            {
                currentFaceSkinIndex = index;
                foreach (var renderer in faceRenderers)
                {
                    renderer.material = faceMaterials[index];
                }
            }
            else
            {
                currentBodySkinIndex = index;
                foreach (var renderer in bodyRenderers)
                {
                    renderer.material = bodyMaterials[index];
                }
            }
            SaveData();
            Debug.Log("��Ų�� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.Log("�������� ���� ��Ų�� ������ �� �����ϴ�.");
        }
    }

    private void ApplySavedSkins()
    {
        if (currentFaceSkinIndex >= 0 && currentFaceSkinIndex < faceMaterials.Length)
        {
            foreach (var renderer in faceRenderers)
            {
                renderer.material = faceMaterials[currentFaceSkinIndex];
            }
        }
        if (currentBodySkinIndex >= 0 && currentBodySkinIndex < bodyMaterials.Length)
        {
            foreach (var renderer in bodyRenderers)
            {
                renderer.material = bodyMaterials[currentBodySkinIndex];
            }
        }
    }

    public void AddPlayerPoints(int amount)
    {
        PointManager.Instance.AddPoints(amount);
        UpdatePointsDisplay();
        Debug.Log($"{amount} ����Ʈ�� �߰��Ǿ����ϴ�. �� ����Ʈ: {PointManager.Instance.GetPoints()}");
    }

    private void UpdatePointsDisplay()
    {
        if (pointsText != null)
        {
            pointsText.text = $"{PointManager.Instance.GetPoints()}";
        }
    }

    public bool IsSkinPurchased(bool isFace, int index)
    {
        return isFace ? purchasedFaceSkins.Contains(index) : purchasedBodySkins.Contains(index);
    }

    public void ResetPurchases()
    {
        purchasedFaceSkins.Clear();
        purchasedBodySkins.Clear();

        // �⺻ ��Ų(�ε��� 0)�� �׻� ���ŵ� ���·� ����
        purchasedFaceSkins.Add(0);
        purchasedBodySkins.Add(0);

        // ���� ��Ų�� �⺻ ��Ų���� ����
        currentFaceSkinIndex = 0;
        currentBodySkinIndex = 0;

        // ��Ų ����
        ApplySavedSkins();

        // ������ ����
        SaveData();

        // UI ������Ʈ
        UpdateAllSkinButtonsUI();

        Debug.Log("��� ���Ű� �ʱ�ȭ�Ǿ����ϴ�.");
    }

    public static (Material faceMaterial, Material bodyMaterial) GetLastSavedSkinMaterials()
    {
        if (PlayerPrefs.HasKey("SkinData"))
        {
            string json = PlayerPrefs.GetString("SkinData");
            SkinData data = JsonUtility.FromJson<SkinData>(json);

            Material faceMaterial = Instance.faceMaterials[data.currentFaceSkinIndex];
            Material bodyMaterial = Instance.bodyMaterials[data.currentBodySkinIndex];

            return (faceMaterial, bodyMaterial);
        }

        // ����� �����Ͱ� ������ �⺻ ��Ų ��ȯ
        return (Instance.faceMaterials[0], Instance.bodyMaterials[0]);
    }
}
