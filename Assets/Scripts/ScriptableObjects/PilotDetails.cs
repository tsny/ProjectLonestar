using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Ship/PilotDetails")]
public class PilotDetails : ScriptableObject
{
    private string firstName = "First";
    private string lastName = "Last";

    public string FirstName
    {
        get
        {
            firstName = NameGenerator.GenerateFirstName();
            return firstName;
        }
    }

    public string LastName
    {
        get
        {
           lastName = NameGenerator.GenerateLastName();
           return lastName;
        }
    }

    public string FullName
    {
        get
        {
            return firstName + " " + lastName;
        }
    }

}
