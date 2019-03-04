using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class AIManager : Object
{
    private static AIManager _inst;
    public static AIManager Instance
    {
        get
        {
            if (!_inst) _inst = new AIManager();
            return _inst;
        }
    }
    public float verificationFreq = 3;
    public float maxDistFromPlayer = 1000;    
    private List<StateController> agents;
    private List<GameObject> mapTargets;

    private AIManager()
    {
        SceneManager.activeSceneChanged += (scn, scn2) => { agents.Clear(); }; 
        agents = FindObjectsOfType<StateController>().ToList();
        StateController.Spawned += AddAgent;

        //StaticCoroutine.StartCoroutine(TestCheck());

        mapTargets = GameObject.FindGameObjectsWithTag("MapTarget").ToList();
    }

    private IEnumerator TestCheck()
    {
        while (true)
        {
            Debug.Log("Test");
            yield return new WaitForSeconds(verificationFreq);
        }
    }

    public GameObject GetRandomGotoPoint()
    {
        if (mapTargets == null || mapTargets.Count <= 0) return null;

        return mapTargets[new System.Random().Next(mapTargets.Count)].gameObject;
    }

    public void AddAgent(StateController agent)
    {
        if (agents.Contains(agent)) return;
        else agents.Add(agent);
    }

    private void OnDestroy()
    {
        // Unsubscribe from any previous events
        StateController.Spawned -= AddAgent;
    }

    public void StopAllAgents()
    {
        agents.ForEach(x => x.FullStop());
    }

    public void DestroyAllAgents()
    {
        agents.ForEach(x => Destroy(x.gameObject));
        agents.Clear();
    }

    private void VerifyAgentDistances()
    {
        // Foreach agent
            // If too far from player and not important to player/scene
                // destroy agent
    }

    private void VerifyAgentExistance()
    {
        VerifyAgentDistances();

        // Some kind of check whether the agent has been in a state too long?
        // Have a float field for this threshold
    }
}