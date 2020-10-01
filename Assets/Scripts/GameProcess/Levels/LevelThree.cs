using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelThree : MonoBehaviour, ILevel
{
    // planet
    GameObject[] planets = new GameObject[24];
    GameObject clonePlanet;
    GameObject shield;
    GameObject hyperJump;
    GameObject booster;

    bool isLevelStop = false;

    public float startPlanetPointY;
    float startPointY;
    float betweenPlanet;
    Vector3 spawnObjectPosition;
    Vector3 spawnPlanetPosition;
    public float planetWait;

    GameObject rocket;
    GameObject blackHole;
    float gameTunnel;
    GameController controller;

    // fuel
    GameObject fuel;
    Vector3 spawnFuelPosition;
    // asteroid
    GameObject asteroid;
    public float startAsteroidPointY = 2000;
    Vector3 spawnAsteroidPosition;
    public int asteroidCount = 80;
    public int asteroidWaveCount = 8;
    public float spawnAsteroidWait = 0.3f;
    public float waveAsteroidWait = 27;

    public void Create(GameController gameController, GameObject rocket)
    {
        this.rocket = rocket;
        startPlanetPointY = 2000;
        startPointY = 2000;
        betweenPlanet = 200;
        gameTunnel = 250;

        planets[0] = StaticPrefabs.planets[0];
        planets[1] = StaticPrefabs.planets[1];
        planets[2] = StaticPrefabs.planets[2];
        planets[3] = StaticPrefabs.planets[3];
        planets[4] = StaticPrefabs.planets[4];
        planets[5] = StaticPrefabs.planets[5];
        planets[6] = StaticPrefabs.planets[6];
        planets[7] = StaticPrefabs.planets[7];
        planets[8] = StaticPrefabs.planets[8];
        planets[9] = StaticPrefabs.planets[9];
        planets[10] = StaticPrefabs.planets[10];
        planets[11] = StaticPrefabs.planets[11];
        planets[12] = StaticPrefabs.planets[12];
        planets[13] = StaticPrefabs.planets[13];
        planets[14] = StaticPrefabs.planets[14];
        planets[15] = StaticPrefabs.planets[15];
        planets[16] = StaticPrefabs.planets[16];
        planets[17] = StaticPrefabs.planets[17];
        planets[18] = StaticPrefabs.planets[18];
        planets[19] = StaticPrefabs.planets[19];
        planets[20] = StaticPrefabs.planets[20];
        planets[21] = StaticPrefabs.planets[21];
        planets[22] = StaticPrefabs.planets[22];
        planets[23] = StaticPrefabs.planets[23];

        blackHole = StaticPrefabs.blackHole;
        shield = StaticPrefabs.shield;
        hyperJump = StaticPrefabs.hyperJump;
        booster = StaticPrefabs.booster;
        fuel = StaticPrefabs.fuel;
        asteroid = StaticPrefabs.asteroid;

        gameController.StartCoroutine(SpawnPlanets());
        if (rocket != null)
        {
            gameController.StartCoroutine(SpawnFuelRoutine());
            gameController.StartCoroutine(SpawnHyperJumpRoutine());
            gameController.StartCoroutine(SpawnShieldRoutine());
            gameController.StartCoroutine(SpawnBoosterRoutine());
        }
        gameController.StartCoroutine(SpawnAsteroidWaves());
        controller = gameController;

        AddBlackHole();
    }

    public void Destroy()
    {
        isLevelStop = true;
        controller = null;
    }

    public IEnumerator SpawnPlanets()
    {
        bool firstPlanet = true;
        int countOfFirstPlanets = 3;
        spawnPlanetPosition.y = 0;
        float nextGenerationPoint = rocket.transform.position.y + betweenPlanet;
        while (!isLevelStop)
        {
            int planetIdx = Random.Range(0, planets.Length - 1);
            GameObject planet = planets[planetIdx];

            float planetSize = planet.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;
            float rocketSize = rocket.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

            if (rocket != null)
            {
                if (!firstPlanet)
                    while (nextGenerationPoint > rocket.transform.position.y)
                    {
                        yield return null;
                    }
                else
                {
                    countOfFirstPlanets--;
                    if (countOfFirstPlanets == 0)
                        firstPlanet = false;
                    // planet
                    spawnPlanetPosition.y += betweenPlanet;
                }
                float scale = Random.Range(50, 100);

                // planet
                float planetX = Random.Range(-(gameTunnel - rocketSize) + planetSize * scale, gameTunnel - rocketSize - planetSize * scale);
                float planetZ = rocket.transform.position.z + Random.Range(-50, 50);

                if (planetZ + planetSize * scale >= gameTunnel - rocketSize)
                    planetZ = gameTunnel - rocketSize - (planetSize * scale);
                if (planetZ + planetSize * scale <= -(gameTunnel - rocketSize))
                    planetZ = -(gameTunnel - rocketSize) + (planetSize * scale);

                spawnPlanetPosition.x = planetX;
                if (!firstPlanet)
                    spawnPlanetPosition.y = rocket.transform.position.y + startPlanetPointY;
                spawnPlanetPosition.z = planetZ;

                planet.GetComponent<Planet>().tmpScale = scale;

                clonePlanet = Instantiate(planet, spawnPlanetPosition, Quaternion.identity);
                nextGenerationPoint = rocket.transform.position.y + betweenPlanet;
            }
        }
    }

    IEnumerator SpawnAsteroidWaves()
    {
        float asteroidSize = asteroid.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;
        while (!isLevelStop)
        {
            for (int j = 0; j < asteroidWaveCount; j++)
            {
                spawnAsteroidPosition.x = rocket.transform.position.x;
                spawnAsteroidPosition.z = rocket.transform.position.z;

                for (int i = 0; i < asteroidCount; i++)
                {
                    //if (rocket != null)
                    //{
                    //    spawnAsteroidPosition.x = Random.Range(-(gameTunnel) + asteroidSize, gameTunnel - asteroidSize );
                    //    spawnAsteroidPosition.y = startAsteroidPointY + rocket.transform.position.y;
                    //    spawnAsteroidPosition.z = Random.Range(-(gameTunnel) + asteroidSize , gameTunnel - asteroidSize );

                    //    Instantiate(asteroid, spawnAsteroidPosition, Quaternion.identity);

                    //    yield return new WaitForSeconds(spawnAsteroidWait);
                    //}
                    if (rocket != null)
                    {

                        spawnAsteroidPosition.y = Random.Range(-400, 400) + startAsteroidPointY + rocket.transform.position.y;

                        Instantiate(asteroid, spawnAsteroidPosition, Quaternion.identity);

                        spawnAsteroidPosition.x = Random.Range(-gameTunnel, gameTunnel);
                        spawnAsteroidPosition.z = Random.Range(-gameTunnel, gameTunnel);
                    }
                }
                yield return new WaitForSeconds(spawnAsteroidWait);
            }
                yield return new WaitForSeconds(waveAsteroidWait);
        }
    }

    void AddBlackHole()
    {
        Vector3 spawnPosHole;
        float x = 500;
        float y = rocket.transform.position.y + startPlanetPointY;
        float z = 0;
        spawnPosHole = new Vector3(x, y, z);
        Instantiate(blackHole, spawnPosHole, Quaternion.identity);
    }
    public IEnumerator SpawnBoosterRoutine()
    {
        while (!isLevelStop)
        {
            SpawnBooster();
            yield return new WaitForSeconds(3f);
        }
    }
    public IEnumerator SpawnShieldRoutine()
    {
        while (!isLevelStop)
        {
            SpawnShield();
            yield return new WaitForSeconds(3);
        }
    }
    public IEnumerator SpawnFuelRoutine()
    {
        while (!isLevelStop)
        {
            SpawnFuel();
            yield return new WaitForSeconds(5f);
        }
    }
    public IEnumerator SpawnHyperJumpRoutine()
    {
        while (!isLevelStop)
        {
            SpawnHyperJump();
            yield return new WaitForSeconds(6f);
        }
    }
    void SpawnFuel()
    {
        float fuelSize = fuel.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

        spawnObjectPosition.x = Random.Range(-(gameTunnel) + fuelSize, gameTunnel - fuelSize);
        spawnObjectPosition.y = Random.Range(startPointY , startPointY) + rocket.transform.position.y;
        spawnObjectPosition.z = Random.Range(-(gameTunnel) + fuelSize, gameTunnel - fuelSize);
        Instantiate(fuel, spawnObjectPosition, Quaternion.Euler(-90, 0, 0));
    }
    void SpawnShield()
    {
        float shieldSize = shield.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

        spawnObjectPosition.x = Random.Range(-(gameTunnel) + shieldSize, gameTunnel - shieldSize);
        spawnObjectPosition.y = Random.Range(startPointY - 70, startPointY) + rocket.transform.position.y;
        spawnObjectPosition.z = Random.Range(-(gameTunnel) + shieldSize, gameTunnel - shieldSize);
        Instantiate(shield, spawnObjectPosition, Quaternion.Euler(-90, 0, 0));
    }
    void SpawnHyperJump()
    {
        float hyperJumpSize = shield.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

        spawnObjectPosition.x = Random.Range(-(gameTunnel) + hyperJumpSize, gameTunnel - hyperJumpSize);
        spawnObjectPosition.y = Random.Range(startPointY - 20, startPointY) + rocket.transform.position.y;
        spawnObjectPosition.z = Random.Range(-(gameTunnel) + hyperJumpSize, gameTunnel - hyperJumpSize);
        Instantiate(hyperJump, spawnObjectPosition, Quaternion.Euler(-90, 0, 0));
    }
    void SpawnBooster()
    {
        float boosterSize = booster.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

        spawnObjectPosition.x = Random.Range(-(gameTunnel) + boosterSize, gameTunnel - boosterSize);
        spawnObjectPosition.y = Random.Range(startPointY - 50, startPointY) + rocket.transform.position.y;
        spawnObjectPosition.z = Random.Range(-(gameTunnel) + boosterSize, gameTunnel - boosterSize);
        Instantiate(booster, spawnObjectPosition, Quaternion.Euler(90, 180, 0));
    }
}


