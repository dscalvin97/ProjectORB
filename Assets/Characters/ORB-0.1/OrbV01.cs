using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbV01 : EnemyController
{
    public override void Rotation()
    {
        base.Rotation();
        _mesh.transform.Rotate(Vector3.right, Mathf.Atan2(_rig.velocity.magnitude, 2), Space.Self);
    }
}
