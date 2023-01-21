using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    private const int SIDE_FLOOR_COUNT = 3;
    [SerializeField]
    private Transform[] _floorPositions = default;
    [SerializeField]
    private FloorController[] _allFloor = default;
    [SerializeField]
    private FloorEvent[] _allEvent = default;
    [SerializeField]
    private FloorPattern[] _allPatterns = default;
    [SerializeField]
    private FloorController _bossFloor = default;
    [SerializeField]
    private int _bossGateOpenCount = 2;
    private FloorPattern _currentPattern = default;
    private int _currentCount = 0;
    private void Start()
    {
        SetFloorPattern();
        ShuffleEvent();
        SetFloorEvent();
    }
    private void ShuffleEvent()
    {
        for (int i = 0; i < _allEvent.Length; i++)
        {
            int r = Random.Range(0, _allEvent.Length);
            var randamEvent = _allEvent[r];
            _allEvent[r] = _allEvent[i];
            _allEvent[i] = randamEvent;
        }
    }
    private void SetFloorPattern()
    {
        int r = Random.Range(0, _allPatterns.Length);
        _currentPattern = _allPatterns[r];
        for (int i = 0; i < _allFloor.Length; i++)
        {
            _allFloor[i].SetGete(_currentPattern.FloorGateSets[i]);
        }
    }
    private void SetFloorEvent()
    {
        for (int i = 0; i < _allFloor.Length; i++)
        {
            _allEvent[i].SetID(i);
            _allEvent[i].OnFloorClear += FloorClear;
            _allEvent[i].gameObject.transform.position = _floorPositions[i].position;
            _allEvent[i].gameObject.SetActive(true);
            foreach (var key in _currentPattern.KeyFloor)
            {
                if (i != key) { continue; }
                _allEvent[i].OnFloorClear += OpenBossGate;
            }
        }
    }
    private void FloorClear(int id)
    {
        if (id + SIDE_FLOOR_COUNT < _allFloor.Length) //South
        {
            _allFloor[id + SIDE_FLOOR_COUNT].OpenGate(FloorDirection.South);
        }
        if (id - SIDE_FLOOR_COUNT >= 0) //North
        {
            _allFloor[id - SIDE_FLOOR_COUNT].OpenGate(FloorDirection.North);
        }
        if (id % SIDE_FLOOR_COUNT > 0) //East
        {
            _allFloor[id - 1].OpenGate(FloorDirection.East);
        }
        if (id % SIDE_FLOOR_COUNT < SIDE_FLOOR_COUNT - 1) //West
        {
            _allFloor[id + 1].OpenGate(FloorDirection.West);
        }
        _allFloor[id].OpenGate();
        
    }
    private void OpenBossGate(int id)
    {
        _currentCount++;
        if (_currentCount >= _bossGateOpenCount)
        {
            _bossFloor.OpenGate();
        }
    }
}
