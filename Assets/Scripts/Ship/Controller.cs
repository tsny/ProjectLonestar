using UnityEngine;

public class Controller : MonoBehaviour 
{
    public event PossessEventHandler PossessedPawn;
    public event ReleaseEventHandler ReleasedPawn;
    public delegate void PossessEventHandler(Controller sender, Pawn pawn);
    public delegate void ReleaseEventHandler(Controller sender, Pawn pawn);

    protected bool _isAcceptingInput;
    public bool IsAcceptingInput
    {
        get 
        {
            return _isAcceptingInput;
        }

        set 
        {
            _isAcceptingInput = value;            
        }
    }

    protected Pawn _currentPawn;
    public Pawn CurrentPawn
    {
        get
        {
            return _currentPawn;
        }
    }

    protected virtual void OnPossess(Pawn pawn)
    {
        if (PossessedPawn != null) PossessedPawn(this, pawn);
    }

    protected virtual void OnRelease(Pawn pawn)
    {
        if (ReleasedPawn != null) ReleasedPawn(this, pawn);
    }

    public virtual void Possess(Pawn pawnToPossess)
    {
        if (_currentPawn == pawnToPossess) return;

        if (pawnToPossess == null) 
        {
            Release();
            return;
        }

        _currentPawn = pawnToPossess;

        _currentPawn.BecomePossessed(this);

        OnPossess(pawnToPossess);
    }

    public virtual void Release()
    {
        OnRelease(_currentPawn);
        _currentPawn.BecomeReleased();
        _currentPawn = null;
    }
}