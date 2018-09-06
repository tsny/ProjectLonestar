using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HighlightComponent : MonoBehaviour
{
    public new Renderer renderer;
    public Shader highlightShader;
    public Shader standardShader;

    //private void Awake()
    //{
    //    renderer = GetComponent<Renderer>();
    //}

    private void OnMouseEnter()
    {
        renderer.material.shader = highlightShader;
    }

    private void OnMouseExit()
    {
        renderer.material.shader = standardShader;
    }
}
