using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HighlightComponent : MonoBehaviour
{
    public Renderer rnd;
    public Shader highlight;

    private Shader orig;

    private void Awake()
    {
        if (rnd == null)
        {
            rnd = GetComponent<Renderer>();
        }

        orig = rnd.material.shader;
    }

    private void OnMouseOver()
    {
        ToggleShader(1);
    }

    private void OnMouseExit()
    {
        ToggleShader(2);
    }

    public void ToggleShader(int toggle)
    {
        switch (toggle)
        {
            case 0:
                rnd.material.shader = rnd.material.shader == orig ? highlight : orig;
                break;
            case 1:
                rnd.material.shader = highlight;
                break;
            case 2:
                rnd.material.shader = orig;
                break;
        }
    }
}
