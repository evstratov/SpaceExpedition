using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelOne : MonoBehaviour, ILevel
{
    GameObject[] planets = new GameObject[8];
    GameObject clonePlanet;
    GameObject shield;
    GameObject hyperJump;
    GameObject booster;
    GameObject rocket;
    GameObject blackHole;
    
    bool isLevelStop = false;
    bool isFlashlightsGenerate = false;
    GameObject flashlight;

    float gameTunnel;
    public float startPlanetPointY;
    float startPointY;
    float betweenPlanet;
    Vector3 spawnPlanetPosition;
    Vector3 spawnObjectPosition;
    public float planetWait;
    // fuel
    public GameObject fuel;
    Vector3 spawnFuelPosition;
    GameController controller;

    public void Create(GameController gameController, GameObject rocket)
    {
        this.rocket = rocket;

        planets[0] = StaticPrefabs.planets[0];
        planets[1] = StaticPrefabs.planets[1];
        planets[2] = StaticPrefabs.planets[2];
        planets[3] = StaticPrefabs.planets[3];
        planets[4] = StaticPrefabs.planets[4];
        planets[5] = StaticPrefabs.planets[5];
        planets[6] = StaticPrefabs.planets[6];
        planets[7] = StaticPrefabs.planets[7];

        shield = StaticPrefabs.shield;
        hyperJump = StaticPrefabs.hyperJump;
        booster = StaticPrefabs.booster;
        fuel = StaticPrefabs.fuel;
        flashlight = StaticPrefabs.flashlight;

        blackHole = StaticPrefabs.blackHole;

        startPlanetPointY = 2000;
        startPointY = 2000;
        betweenPlanet = 200;
        gameTunnel = 250;
        controller = gameController;

        controller.skyMaterial.SetFloat("_Exposure", StaticPrefabs.skyIntensity);

        controller.StartCoroutine(SpawnPlanets());
        if (rocket != null)
        {
            controller.StartCoroutine(SpawnFuelRoutine());
            controller.StartCoroutine(SpawnHyperJumpRoutine());
            controller.StartCoroutine(SpawnShieldRoutine());
        }
    }

    public void Destroy()
    {
        isLevelStop = true;
        controller = null;
    }

    public IEnumerator SpawnPlanets()
    {
        bool firstPlanet = true;
        int countOfFirstPlanets = 7;
        spawnPlanetPosition.y = 0;
        float nextGenerationPoint = rocket.transform.position.y + betweenPlanet;
        while (!isLevelStop)
        {
            int planetIdx = Random.Range(0, planets.Length - 1);
            GameObject planet = planets[planetIdx];

            float planetSize = planet.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;
            //float rocketSize = rocket.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;
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
                    SpawnFuel(true);
                    SpawnShield(true);
                    SpawnHyperJump(true);
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

    public IEnumerator SpawnFuelRoutine()
    {
        while (!isLevelStop)
        {
            if (rocket != null)
            {
                SpawnFuel(false);
            }
            yield return new WaitForSeconds(5f);
        }
    }
    public IEnumerator SpawnHyperJumpRoutine()
    {
        while (!isLevelStop)
        {
            if (rocket != null)
            {
                SpawnHyperJump(false);
            }
            yield return new WaitForSeconds(7f);
        }
    }
    public IEnumerator SpawnShieldRoutine()
    {
        while (!isLevelStop)
        {
            SpawnShield(false);
            yield return new WaitForSeconds(4f);
        }
    }

    void SpawnFuel(bool isFirst)
    {
        float fuelSize = fuel.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

        spawnObjectPosition.x = Random.Range(-(gameTunnel) + fuelSize, gameTunnel - fuelSize);
        if (!isFirst)
            spawnObjectPosition.y = Random.Range(startPointY - 100, startPointY) + rocket.transform.position.y;
        else
            spawnObjectPosition.y = Random.Range(750, 1500) + rocket.transform.position.y;
        spawnObjectPosition.z = Random.Range(-(gameTunnel) + fuelSize, gameTunnel - fuelSize);
        Instantiate(fuel, spawnObjectPosition, Quaternion.Euler(-90, 0, 0));
    }
    void SpawnShield(bool isFirst)
    {
        float shieldSize = shield.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

        spawnObjectPosition.x = Random.Range(-(gameTunnel) + shieldSize, gameTunnel - shieldSize);
        if(!isFirst)
            spawnObjectPosition.y = Random.Range(startPointY - 50, startPointY) + rocket.transform.position.y;
        else
            spawnObjectPosition.y = Random.Range(750, 1500) + rocket.transform.position.y;
        spawnObjectPosition.z = Random.Range(-(gameTunnel) + shieldSize, gameTunnel - shieldSize);
        Instantiate(shield, spawnObjectPosition, Quaternion.Euler(-90, 0, 0));
    }
    void SpawnHyperJump(bool isFirst)
    {
        float hyperJumpSize = hyperJump.GetComponentsInChildren<MeshFilter>()[0].sharedMesh.bounds.size.x;

        spawnObjectPosition.x = Random.Range(-(gameTunnel) + hyperJumpSize, gameTunnel - hyperJumpSize);
        if (!isFirst)
            spawnObjectPosition.y = Random.Range(startPointY - 20, startPointY) + rocket.transform.position.y;
        else
            spawnObjectPosition.y = Random.Range(750, 1500) + rocket.transform.position.y;
        spawnObjectPosition.z = Random.Range(-(gameTunnel) + hyperJumpSize, gameTunnel - hyperJumpSize);
        Instantiate(hyperJump, spawnObjectPosition, Quaternion.Euler(-90, 0, 0));
    }
}

