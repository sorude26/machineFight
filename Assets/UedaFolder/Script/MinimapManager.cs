using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class MinimapManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyIcon;
    [SerializeField] GameObject _targetIcon;
    private List<GameObject> _enemyIcons = new List<GameObject>();
    [SerializeField]  Camera targetCamera;
    [SerializeField] RectTransform parentUI;

    void Update()
    {
        int i = 0;
        foreach (var target in LockOnController.Instance?.LockOnTargets)
        {
            if(_enemyIcons.Count - 1 < i)
            {
                if (target.DamageChecker.BossTarget)
                {
                    _enemyIcons.Add(Instantiate(_targetIcon,
                        target.DamageChecker.transform.position, 
                        _enemyIcon.transform.rotation, 
                        parentUI));
                }
                else
                {
                    _enemyIcons.Add(Instantiate(_enemyIcon,
                        target.DamageChecker.transform.position,
                        _enemyIcon.transform.rotation, 
                        parentUI));
                }
                
            }
                _enemyIcons[i].transform.position = target.DamageChecker.transform.position;
            i++;
        }

    }
}
