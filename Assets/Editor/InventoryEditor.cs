using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        Inventory inventory = (Inventory)target;
        base.OnInspectorGUI();

        if (inventory.Items != null)
        {
            if (GUILayout.Button("Dummy Item"))
            {
                inventory.AddItem(CreateInstance<Item>());
            }
        }
    }
}
