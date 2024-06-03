using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeatures : MovementPlayer
{

    public void PlayerRotation(LayerMask GroundMask)
    {
        //Create radius from camera and debug
        Ray radius = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(radius.origin, radius.direction * 100, Color.red);

        //Creating ray's impact in the ground for player looking
        RaycastHit ground_impact;

        if (Physics.Raycast(radius, out ground_impact, 100, GroundMask))
        {
            Vector3 crosshairPos = ground_impact.point - transform.position; //From the impact position based on the player's position
            crosshairPos.y = transform.position.y; //Cancelling y-axis to avoid player looking up/down
            Rotate(crosshairPos);
        }

    }
}
