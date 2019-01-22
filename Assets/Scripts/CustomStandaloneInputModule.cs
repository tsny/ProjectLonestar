using UnityEngine;
using UnityEngine.EventSystems;

public class CustomStandaloneInputModule : StandaloneInputModule
{
    public PointerEventData GetPointerData()
    {
        return m_PointerData[kMouseLeftId];
    }
}
