using UnityEngine;
using System;

public class UnitControllerInstaller : MonoBehaviour
{
    [SerializeField] private CoinController coinControllerPrefab;
    [SerializeField] private SkeletonController skeletonControllerPrefab;

    public UnitController Instantiate(Unit unit, Vector3 position, Quaternion rotation, Transform spawner)
    {
        UnitController unitController;
        if(unit is Coin)
            unitController = GameObject.Instantiate<CoinController>(coinControllerPrefab, position, rotation, spawner);
        else if(unit is Skeleton)
            unitController = GameObject.Instantiate<SkeletonController>(skeletonControllerPrefab, position, rotation, spawner);
        else
            throw new NotImplementedException();
        unitController.Construct(unit);
        return unitController;
    }
}