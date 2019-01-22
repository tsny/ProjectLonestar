using UnityEngine;

public class TestAI : MonoBehaviour 
{
    public Ship ship;
    public Ship target;

    void Update()
    {
        if (target == null) return;
        ship.engine.Throttle = 1; 
        ship.aimPosition = target.transform.position;

        Quaternion newRot = Quaternion.LookRotation(target.transform.position - ship.transform.position);
        ship.transform.rotation = Quaternion.Slerp(ship.transform.rotation, newRot, ship.engineStats.turnSpeed * Time.deltaTime);

        //ship.FireActiveWeapons();
    }
}