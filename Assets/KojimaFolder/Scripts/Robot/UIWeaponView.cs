using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponView : MonoBehaviour
{
    const string STANDBY_TEXT = "STAND BY";
    const string READY_TEXT = "READY";
    const string NOCONNECTION_TEXT = "NO CONNECTION";
    const string NOCONNECTION_AMMO_TEXT = "---";
    const string OUT_OF_AMMO_TEXT = "OUT OF AMMO";
    const string RELOAD_TEXT = "RELOAD";
    const string INFINITY_TEXT = "INF";

    const int RIGHT = 0;
    const int LEFT = 1;
    const int BACK = 2;


    [SerializeField]
    PlayerMachineController _machine;
    [Header("0右1左2背中")]
    [SerializeField]
    Text[] _weaponCondition;
    [SerializeField]
    Text[] _weaponNum;
    [SerializeField]
    Text[] _weaponAllNum;

    bool[] _onOff = new bool[3];
    WeaponBase[] _weapons = new WeaponBase[3];

    private IEnumerator Start()
    {
        yield return null;

        if (_machine == null) yield break;

        _weapons[RIGHT] = _machine.MachineController.BodyController.RightHand.WeaponBase;
        _weapons[LEFT] = _machine.MachineController.BodyController.LeftHand.WeaponBase;
        _weapons[BACK] = _machine.MachineController.BodyController.BackPack.BackPackWeapon;

        _weapons[RIGHT].OnCount += UpdateRight;
        PlayerVrCockpit.Instance.WeaponSwitch(RIGHT).OnTurnOff += UpdateRight;
        PlayerVrCockpit.Instance.WeaponSwitch(RIGHT).OnTurnOn += UpdateRight;
        _weapons[LEFT].OnCount += UpdateLeft;
        PlayerVrCockpit.Instance.WeaponSwitch(LEFT).OnTurnOff += UpdateLeft;
        PlayerVrCockpit.Instance.WeaponSwitch(LEFT).OnTurnOn += UpdateLeft;
        //バックパックはない場合があるのでnullチェックが必要
        if (_weapons[BACK] != null)
        {
            _weapons[BACK].OnCount += UpdateBack;
            PlayerVrCockpit.Instance.WeaponSwitch(BACK).OnTurnOff += UpdateBack;
            PlayerVrCockpit.Instance.WeaponSwitch(BACK).OnTurnOn += UpdateBack;
        }
        yield return null;

        UpdateWeaponStatus(RIGHT);
        UpdateWeaponStatus(LEFT);
        UpdateWeaponStatus(BACK);
    }

    private void UpdateRight()
    {
        UpdateWeaponStatus(RIGHT);
    }

    private void UpdateLeft()
    {
        UpdateWeaponStatus(LEFT);
    }

    void UpdateBack()
    {
        UpdateWeaponStatus(BACK);
    }

    private void UpdateWeaponStatus(int id)
    {
        if (_weapons[id] == null)
        {
            //武器がないので接続なし表示
            _weaponCondition[id].text = NOCONNECTION_TEXT;
            _weaponAllNum[id].text = NOCONNECTION_AMMO_TEXT;
            _weaponNum[id].text = NOCONNECTION_AMMO_TEXT;
            return;
        }


        //武器総弾数の表示
        if (_weapons[id].MaxAmmunitionCapacity <= 0 || _weapons[id].Type != WeaponType.HandGun)
        {
            //予備弾薬無限の武器なので無限表示
            _weaponAllNum[id].text = INFINITY_TEXT;
        }
        else
        {
            //予備弾薬数の表示
            _weaponAllNum[id].text = _weapons[id]._currentAmmunition.ToString();
        }

        //装填されている弾数の表示
        if (_weapons[id].MagazineCount <= 0 || _weapons[id].Type != WeaponType.HandGun)
        {
            //装弾数無限のため無限表示
            _weaponNum[id].text = INFINITY_TEXT;
        }
        else
        {
            //装弾数表示
            _weaponNum[id].text = _weapons[id]._currentMagazine.ToString();
        }

        //武器状態表示
        if (_weapons[id]._currentMagazine == 0 && _weapons[id].Type != WeaponType.HandGun)
        {
            //装弾数ゼロのためリロード表示か欠乏表示を行う
            if (_weapons[id]._currentAmmunition == 0)
            {
                //弾薬欠乏表示
                _weaponCondition[id].text = OUT_OF_AMMO_TEXT;
            }
            else
            {
                //リロード表示
                _weaponCondition[id].text = READY_TEXT;
            }
        }
        else
        {
            if (PlayerVrCockpit.Instance.WeaponSwitch(id).IsOn)
            {
                //武器アクティブ表示
                _weaponCondition[id].text = READY_TEXT;
            }
            else
            {
                //武器スタンバイ表示
                _weaponCondition[id].text = STANDBY_TEXT;
            }
        }
    }
}
