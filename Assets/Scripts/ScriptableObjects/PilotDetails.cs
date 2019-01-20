using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/PilotDetails")]
public class PilotDetails : ScriptableObject
{
    public string firstName = "First";
    public string lastName = "Last";

    public string FullName
    {
        get
        {
            return firstName + " " + lastName;
        }
    }

}
