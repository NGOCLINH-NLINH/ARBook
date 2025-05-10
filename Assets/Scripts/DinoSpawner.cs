using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class DinoSpawner : MonoBehaviour
{
    public GameObject modelPrefab;

    private GameObject spawnedModel;
    private ObserverBehaviour mObserverBehaviour;

    private void Awake()
    {
        mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnDestroy()
    {
        if (mObserverBehaviour)
        {
            mObserverBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        if (targetStatus.Status == Status.TRACKED || targetStatus.Status == Status.EXTENDED_TRACKED)
        {
            SpawnDino();
        }
        else
        {
            RemoveDino();
        }
    }

    private void SpawnDino()
    {
        if (spawnedModel == null)
        {
            spawnedModel = Instantiate(modelPrefab, transform.position, transform.rotation);
        }

        // if (DinoInfoManager.Instance != null)
        // {
        //     DinoInfoManager.Instance.SetCurrentDino(modelPrefab.name);
        // }
    }

    private void RemoveDino()
    {
        if (spawnedModel != null)
        {
            Destroy(spawnedModel);
            spawnedModel = null;
        }
    }
}
