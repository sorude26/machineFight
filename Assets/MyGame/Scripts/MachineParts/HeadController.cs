using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour, IPartsModel
{
    [SerializeField]
    private int _id = default;
    public int ID { get => _id; }
}
