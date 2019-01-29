using UnityEngine;

public class TargetingInfo : MonoBehaviour 
{
    private bool _selected;
    public bool Selected { get { return _selected; } }
    public bool showHealthOnSelect = true;

    public bool targetable = true;

    public Health health;
    public string indicatorHeaderText = "Object - Info - Distance";

    public static event TargetEventHandler Spawned;
    public delegate void TargetEventHandler(TargetingInfo sender);

    public void Init(string header, Health health, bool showHealthOnSelect = true)
    {
        indicatorHeaderText = header;
        this.health = health;
        this.showHealthOnSelect = showHealthOnSelect;
    }

    void Start()
    {
        if (Spawned != null) Spawned(this);
    }

    public void ToggleSelected(bool val)
    {
        _selected = true;
    }

    private void OnMouseOver() 
    {
        if (Input.GetMouseButtonUp(0) && targetable)
        {
            ToggleSelected(true);
        }     
    }

}