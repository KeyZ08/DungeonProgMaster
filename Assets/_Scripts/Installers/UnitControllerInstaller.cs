using UnityEngine;
using System;

public class UnitControllerInstaller : MonoBehaviour
{
    [SerializeField] private CoinController coinControllerPrefab;
    [SerializeField] private SkeletonController skeletonControllerPrefab;

    public BaseUnitController Instantiate(Unit unit, Vector3 position, Quaternion rotation, Transform spawner)
    {
        BaseUnitController unitController;
        if (unit is Coin coin)
        {
            var coinController = GameObject.Instantiate<CoinController>(coinControllerPrefab, position, rotation, spawner);
            coinController.Construct(coin);
            unitController = coinController;
        }
        else if (unit is Skeleton skeleton)
        {
            var skeletonController = GameObject.Instantiate<SkeletonController>(skeletonControllerPrefab, position, rotation, spawner);
            skeletonController.Construct(skeleton);
            unitController= skeletonController;
        }
        else
            throw new NotImplementedException();
        return unitController;
    }
}