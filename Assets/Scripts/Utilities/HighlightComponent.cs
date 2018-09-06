using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HighlightComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Material material;
    public Material highlightMaterial;
    public Shader highlightShader;
    public Shader standardShader;

    private void Awake()
    {
        material = GetComponent<Material>();
        print("test");
    }

    private void OnMouseEnter()
    {
        print("more"); 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        material.shader = highlightShader;
        print("enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        material.shader = standardShader;
    }
}
