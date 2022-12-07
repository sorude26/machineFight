using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StageSituation : Situation
{
    const int X_RANGE_MIN = 3;
    const int X_RANGE_MAX = 4;
    const int Y_RANGE_MIN = 2;
    const int Y_RANGE_MAX = 3;
    const int STAGE_COUNT = 3;
    [SerializeField]
    SwitchSpawnArea _switchSpawnArea;

    int _switchRemain;
    int _stageRemain;
    float _timer;


    public override void StartSituation()
    {
        base.StartSituation();
        foreach (Transform item in this.transform)
        {
            item.gameObject.SetActive(true);
        }
        _stageRemain = STAGE_COUNT;
        _timer = 0;
        StartStage();
    }

    public override void SituationUpdate()
    {
        _timer += Time.deltaTime;
    }

    public override void EndSituation()
    {
        _switchSpawnArea.MakeSpawnPositions(0, 0);
        ScoreManager.time = _timer;
        foreach (Transform item in this.transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void StartStage()
    {
        int xRange = Random.Range(X_RANGE_MIN, X_RANGE_MAX + 1);
        int yRange = Random.Range(Y_RANGE_MIN, Y_RANGE_MAX + 1);
        _switchRemain = xRange * yRange;
        var SwitchPrefabs = GameManager.Instance.SwitchPrefabs;
        foreach (Transform t in _switchSpawnArea.MakeSpawnPositions(xRange, yRange))
        {
            Switch make = SwitchPrefabs[Random.Range(0, SwitchPrefabs.Length)];
            Switch maked = Instantiate(make, t);
            maked.transform.localPosition = Vector3.zero;
            maked.transform.localRotation = Quaternion.identity;
            maked.OnTurnOn += () => CountChangeAndCheck(-1);
            maked.OnTurnOff += () => CountChangeAndCheck(1);
        }
    }

    private void NextStage()
    {
        --_stageRemain;
        if (_stageRemain == 0)
        {
            ScoreManager.time = _timer;
            GameManager.Instance.ChangeSituation(GameManager.SituationType.Result);
        }
        else
        {
            StartStage();
        }
    }

    private void CountChangeAndCheck(int count)
    {
        _switchRemain += count;
        if (_switchRemain == 0)
        {
            NextStage();
        }
    }


}
