using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DebugConsole : MonoBehaviour
{
    public VerticalLayoutGroup verticalLayoutGroup;
    public GameObject content;
    public GameObject debugMethodButtonPrefab;

    private PlayerController playerController;
    private ShipSpawner shipSpawner;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        shipSpawner = FindObjectOfType<ShipSpawner>();

        PopulateDebugConsole(GetDebugMethods());
    }

    private void Start()
    {
        content.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) && GameManager.instance.debugMode)
        {
            content.SetActive(!content.activeSelf);
        }
    }

    private MethodInfo[] GetDebugMethods()
    {
        var methodInfo = typeof(DebugConsole).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        List<MethodInfo> debugMethods = new List<MethodInfo>();

        foreach (var method in methodInfo)
        {
            if (method.ReturnType == typeof(void) && method.GetParameters().Length == 0)
            {
                debugMethods.Add(method);
            }
        }

        return debugMethods.ToArray();
    }

    private void PopulateDebugConsole(MethodInfo[] methodInfo)
    {
        foreach (var method in methodInfo)
        {
            var currButton = Instantiate(debugMethodButtonPrefab, verticalLayoutGroup.transform).GetComponent<Button>();
            currButton.onClick.AddListener(() => Invoke(method.Name, 0));
            currButton.GetComponentInChildren<Text>().text = method.Name;
        }
    }

    public void GodMode()
    {
        playerController.controlledShip.invulnerable = !playerController.controlledShip.invulnerable;
        print("Godmode : " + playerController.controlledShip.invulnerable);
    }

    public void SpawnDefaultShip()
    {
        shipSpawner.SpawnDefaultShip();
    }

    public void UnlimitedEnergy()
    {
        playerController.controlledShip.hardpointSystem.EnableInfiniteEnergy();
    }

    public void UnlimitedAfterburner()
    {
        playerController.controlledShip.hardpointSystem.afterburnerHardpoint.drain = 0;
    }

    public void UnPossess()
    {
        playerController.UnPossess();
    }

    public void SpawnNewPlayerShip()
    {
        var newShip = shipSpawner.SpawnDefaultShip();
        playerController.Possess(newShip);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
