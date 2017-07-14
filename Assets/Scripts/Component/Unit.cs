using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour {

	public Tile tile { get; protected set; }
    public Directions dir;

    public void Place (Tile target)
    {
        // make sure old tile location is not still pointing to this unit
        if (tile != null && tile.content == gameObject)
            tile.content = null;

        // link unity and tile references
        tile = target;

        if (target != null)
            target.content = gameObject;
    }

    // aligns unit position and rotation with tile and direction fields
    public void Match()
    {
        transform.localPosition = tile.center;
        transform.localEulerAngles = dir.ToEuler();
    }
}
