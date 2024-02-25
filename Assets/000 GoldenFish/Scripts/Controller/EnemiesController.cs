using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unicorn
{
    public class EnemiesController : MonoBehaviour
    {
        [SerializeField] private Timer timer;
        
        [SerializeField] private LevelController _levelControlle;
        [SerializeField] private DifficultyAsset _difficultInfo;
        
        [SerializeField] public  List<int> timesSpawnPeriod;
        [SerializeField] public  List<int> SpawnAmount;

        [SerializeField] public List<int> SpecialTimes;
        [SerializeField] public int startTime;

        [HideInInspector] public bool isStartSpawn => IsStartToSpawn();
        public int difficulty;
        [HideInInspector] public float timeCount;
        [HideInInspector] public int periodCount;
        [HideInInspector] public int period;

        private int i = 0;
        private void Start()
        {
            timesSpawnPeriod = _difficultInfo.DifficultInfos[0].timeSpawnPeriod;
            SpawnAmount = _difficultInfo.DifficultInfos[0].timeSpawnAmount;
            period = GetNextSpawnTime();
        }

        [Button]
        public void InCreaaseDifficulty()
        {
            if(difficulty >= 2)
            {
                return;
            }
            
            difficulty++;
            timesSpawnPeriod = _difficultInfo.DifficultInfos[2].timeSpawnPeriod;
            SpawnAmount = _difficultInfo.DifficultInfos[2].timeSpawnAmount;
        }
        
        
        public bool IsStartToSpawn()
        {
            
            Debug.Log(timer.levelTime - timer.currentTime >= startTime);
            return timer.levelTime - timer.currentTime >= startTime;
        }
        
        public int GetNextSpawnTime()
        {
            return Random.Range(timesSpawnPeriod[0], timesSpawnPeriod[0]);
        }

        public int GetAmountOfSpawn()
        {
            return Random.Range(SpawnAmount[0], SpawnAmount[1]);
        }

        
        
        private void Update()
        {
            if (!_levelControlle.isPlaying || !isStartSpawn)
            {
                return;
            }

            timeCount += Time.deltaTime;
            if (timeCount >= 1)
            {
                timeCount = 0;
                periodCount++;
                if (periodCount == period)
                {
                    periodCount = 0;
                    period = GetNextSpawnTime();
                    StartCoroutine(Spawn(GetAmountOfSpawn()));
                }
            }

            if (timer.currentTime == SpecialTimes[i])
            {
                //SpawnBoss;
                i++;
                InCreaaseDifficulty();
                for (int j = 0; j < i; j++)
                {
                    _levelControlle.SpawnShark();
                    for (int k = 0; k < i * 2; k++)
                    {
                        _levelControlle.SpawnBalrog();
                    }
                }
                // _levelControlle.SpawnShark();
                // if (i >= 1)
                // {
                //     _levelControlle.SpawnShark();
                // }
                // i++;
                // if (i == SpecialTimes.Count-1)
                // {
                //     _levelControlle.SpawnShark();
                //     _levelControlle.SpawnShark();
                //     _levelControlle.SpawnShark();
                //     _levelControlle.SpawnShark();
                // }
            }


        }

        private IEnumerator Spawn(int numberOfSpawn)
        {
            var yieldTime = Random.Range(0.3f, 0.6f);
            for (int i = 0; i < numberOfSpawn; i++)
            {
                _levelControlle.SpawnBalrog();
                yield return Yielders.Get(yieldTime);
            }
        }
    }
}
