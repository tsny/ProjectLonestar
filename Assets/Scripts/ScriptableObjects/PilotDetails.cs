using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/PilotDetails")]
public class PilotDetails : ScriptableObject
{
    public string firstName = "First";
    public string lastName = "Last";

    private void Awake()
    {
        firstName = NameGenerator.GenerateFirstName();
        lastName = NameGenerator.GenerateLastName();
    }

    public string FullName
    {
        get
        {
            return firstName + " " + lastName;
        }
    }

}
