using UnityEngine;

public class Timer : MonoBehaviour
{
    [ReadOnly]
    public float timeRemaining = 5;
    public float duration = 5;
    private bool isDecrementing;

    private string methodToCall;
    private MonoBehaviour owner;

    // Call this initializer if the timer needs to call a certain method on its parent
    public void Initialize(float duration, MonoBehaviour owner, string methodToCall)
    {
        this.methodToCall = methodToCall;
        this.owner = owner;

        Initialize(duration);
    }

    // Call this initializer if you don't want to invoke a method at the end of the timer's life
    public void Initialize(float duration)
    {
        this.duration = duration;
        timeRemaining = duration;
        isDecrementing = true;
    }

    private void Update()
    {
        if (!isDecrementing) return;

        if (timeRemaining > 0) timeRemaining -= Time.deltaTime;

        else
        {
            if(owner != null) owner.Invoke(methodToCall, 0f);
            Destroy(this);
        }
    }

    public void Restart(bool startTimer)
    {
        timeRemaining = duration;
        isDecrementing = startTimer;
    }

    public void Pause()
    {
        isDecrementing = false;
    }

    public void Resume()
    {
        isDecrementing = true;
    }
}
