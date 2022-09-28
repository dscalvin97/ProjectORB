using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayerController : PlayerController
{
    private float _lookDirectionAngle = 0f;
    private float _moveXSeed;
    private float _moveYSeed;
    private float _rotateSeed;

    public override void Start()
    {
        base.Start();
        _moveXSeed = Random.Range(-100f, 100f);
        _moveYSeed = Random.Range(-100f, 100f);
        _rotateSeed = Random.Range(-100f, 100f);
    }

    public override void Movement(Vector3 input)
    {
        float xVal = RandomInput(_moveXSeed);
        float yVal = RandomInput(_moveYSeed);
        Vector3 newInput = Vector3.ClampMagnitude(new Vector3(xVal, 0, yVal), 1);
        base.Movement(newInput);
    }

    public override void Rotation(Vector3 input)
    {
        _useMouseRotation = false;
        _lookDirectionAngle += RandomInput(_rotateSeed) * Time.deltaTime;

        Vector3 newInput = new Vector3(Mathf.Cos(_lookDirectionAngle), 0, Mathf.Sin(_lookDirectionAngle));
        Debug.Log(newInput);
        base.Rotation(newInput);
    }

    private float RandomInput(float seed)
    {
        return Mathf.Clamp((Mathf.PerlinNoise(Time.time * .5f, seed) * 2) - 1f, -1f, 1f);
    }
}
