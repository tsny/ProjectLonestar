using UnityEngine;

public class Pawn : MonoBehaviour
{
    private Controller _controller;
    public Controller Controller 
    {
        get
        {
            return _controller;
        }
    }

    public virtual void OnPossessed() { }
    public virtual void OnReleased() { }

    public void BecomeReleased()
    {
        _controller = null;
        OnReleased();
    }

    public void BecomePossessed(Controller controller)
    {
        _controller = controller;
        OnPossessed();
    }
}
