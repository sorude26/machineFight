using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField]
    protected int _catchSEID = 21;
    [SerializeField]
    protected float _seVolume = 0.02f;
    [SerializeField]
    private float _itemLifeTime = 20f;
    [SerializeField]
    protected float _energyValue = 2f;
    private float _timer = 0;
    public abstract void CatchItem(ItemCatcher catcher);
    public virtual void CatchAction(ItemCatcher catcher)
    {
        CatchItem(catcher);
        catcher.PlayCatchEffect(_catchSEID,_seVolume);
        catcher.Player.RecoveryEnergy(_energyValue);
        _timer = 0;
        gameObject.SetActive(false);
    }
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _itemLifeTime)
        {
            _timer = 0;
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemCatcher>(out var catcher))
        {
            CatchAction(catcher);
        }
    }
}
