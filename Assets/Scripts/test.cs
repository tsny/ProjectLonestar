using UnityEngine;

public class test : MonoBehaviour 
{
    public GameObject tester;

    public void method(GameObject input)
    {
        if (tester)
        {
            if (Application.isPlaying) Destroy(tester);
            else DestroyImmediate(tester);
        }

        tester = Instantiate(input, transform);
    }
}