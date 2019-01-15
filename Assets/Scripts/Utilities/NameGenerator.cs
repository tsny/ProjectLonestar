using UnityEngine;
using System.IO;
using System.Collections;

public class NameGenerator 
{
    public static Name Generate(Gender gender = Gender.Male)
    {
        var path = Application.streamingAssetsPath;

        if (gender == Gender.Male)
            path += "/female_names.json";
        else
            path += "/male_names.json";

        if (!File.Exists(path)) return null;

        var jsonFile = File.ReadAllText(path);
        JSONWrapper wrapper = JsonUtility.FromJson<JSONWrapper>(jsonFile);

        int randomFirstIndex = Random.Range(0, wrapper.Names.Length - 1);
        int randomLastIndex = Random.Range(0, wrapper.Names.Length - 1);
        string firstName = wrapper.Names[randomFirstIndex].First;
        string lastName = wrapper.Names[randomLastIndex].Last;
        return new Name(firstName, lastName);
    }    

    public static string GenerateFirstName(Gender gender = Gender.Male)
    {
        return Generate(gender).First;
    }

    public static string GenerateLastName(Gender gender = Gender.Male)
    {
        return Generate(gender).Last;
    }

    public static string GenerateFullName(Gender gender = Gender.Male)
    {
        var firstName = Generate(gender).First;
        var lastName = Generate(gender).Last;

        return firstName + " " + lastName;
    }
}

public enum Gender
{
    Male,
    Female
}

[System.Serializable]
public class Name
{
    public string First;
    public string Last;

    public Name(string first, string last)
    {
        First = first;
        Last = last;
    }
}

[System.Serializable]
public class JSONWrapper
{
    public Name[] Names;
}
