using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorDirection
{
    North = 0,
    South = 1,
    East = 2,
    West = 3,
}
public class FloorController : MonoBehaviour
{
    [SerializeField]
    private GateWall _northWall = default;
    [SerializeField]
    private GateWall _southWall = default;
    [SerializeField]
    private GateWall _eastWall = default;
    [SerializeField]
    private GateWall _westWall = default;
    private GateWall[] _walls = default;
    private bool _isInitialize = false;
    private void Start()
    {
        if (_isInitialize == true)
        {
            return;
        }
        InitializeWall();
    }
    private void InitializeWall()
    {
        _isInitialize = true;
        _walls = new GateWall[] { _northWall, _southWall, _eastWall, _westWall };
    }
    public void OpenGate(FloorDirection direction)
    {
        if (_isInitialize == false)
        {
            InitializeWall();
        }
        _walls[(int)direction].OpenGate();
    }
    public void SetGete(bool north, bool south, bool east, bool weast)
    {
        if (_isInitialize == false)
        {
            InitializeWall();
        }
        SetGate(FloorDirection.North, north);
        SetGate(FloorDirection.South, south);
        SetGate(FloorDirection.East, east);
        SetGate(FloorDirection.West, weast);
    }
    public void SetGate(FloorDirection direction, bool isGate = false)
    {
        if (_isInitialize == false)
        {
            InitializeWall();
        }
        _walls[(int)direction].SetGateWall(isGate);
    }
    public void OpenGate()
    {
        if (_isInitialize == false)
        {
            InitializeWall();
        }
        foreach (var wall in _walls)
        {
            wall.OpenGate();
        }
    }

}