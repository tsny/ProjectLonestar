using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "AI/Actions/FindRandomTarget")]
public class GetRandomTarget : FLAction
{
    public bool findOnlyShips = true;
    public bool includeAllies;
    // Select only allies?

    public override void Init() {}

    public override void Act(StateController controller)
    {
        if (findOnlyShips)
        {
            List<Ship> ships = FindObjectsOfType<Ship>().ToList();

            ships.Remove(controller.ship);
            ships.Remove(GameSettings.pc.ship);

            foreach (var ship in controller.allies)
            {
                ships.Remove(ship);
            }

            if (ships.Count <= 0) return;

            var rnd = new System.Random();
            int r = rnd.Next(ships.Count); 

            controller.targetTrans = ships[r].transform;
        }
        else
        {
            var objs = FindObjectsOfType<MeshRenderer>();
            if (objs.Length <= 0) return;
            var rnd = new System.Random();
            int r = rnd.Next(objs.Length);

            controller.targetTrans = objs[r].transform;
        }
    }
}