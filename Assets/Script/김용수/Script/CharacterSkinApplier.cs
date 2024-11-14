using UnityEngine;

public class CharacterSkinApplier : MonoBehaviour
{
    public SkinnedMeshRenderer faceRenderer;
    public SkinnedMeshRenderer bodyRenderer;

    private void Start()
    {
        ApplySavedSkins();
    }

    public void ApplySavedSkins()
    {
        (Material faceMaterial, Material bodyMaterial) = Skinmanager.GetLastSavedSkinMaterials();

        if (faceRenderer != null)
        {
            faceRenderer.material = faceMaterial;
        }

        if (bodyRenderer != null)
        {
            bodyRenderer.material = bodyMaterial;
        }

        Debug.Log("Applied saved skins to character in the new scene.");
    }
}