using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RocketFly : MonoBehaviour
{
    public delegate void ChangeLevel(byte currentLevel);
    public event ChangeLevel ChangeLevelEvent;

    //Animator anim;
    // sound
    AudioSource coinSound;
    public AudioSource resourceSound;
    AudioSource jumpSound;
    AudioSource loopMusic;
    AudioSource gameOverSound;
    // levels
    public byte currentLevel;

    public bool isGameOver;
    bool isFuelFreeze;
    // UI
    Text textScore;
    Text textSpeed;
    Text textLevel;
    Text textResource;
    Text textGameOver;
    Text textAddScore;
    Image fuelProgress;

    // fuel
    public float fuel;
    public int countOfFuel;
    

    Transform soplo;
    public Quaternion soploRotation;

    Camera rocket_camera;

    public int score = 0;
    public int gameTunel = 300;
    // boost
    public int boost;
    // hyper jump
    bool isHyperJump;

    // movement
    Transform childRocket;
    bl_Joystick Joystick;
    Vector3 fly;
    Rigidbody rbRocket;

    // settings
    public float speed;
    float startSpeed;
    public float control;
    public float resourceTime;
    public float fuelConsumptionSpeed;
    // shield
    public bool isShieldActive;
    public GameObject shieldSphere;
    bool lockRocket = false;
    float speedRotate = 100f;
    Vector3 rotation = new Vector3(0, 10, 0);

    // flashlight 
    public float lightPower;
    public GameObject flashlight;

    // resourses time
    public float shieldTime = 7;
    public float freezeTime = 10;
    public float jumpTime = 2;
    public float boostTime = 10;
    public float flashlightTime = 15;


    // Start is called before the first frame update
    void Start()
    {
        startSpeed = speed;
        isFuelFreeze = false;

        Joystick = GameObject.FindWithTag("Joystick").GetComponent<bl_Joystick>();
        soplo = transform.GetComponentsInChildren<Transform>()[1];

        textScore = GameObject.FindWithTag("Canvas").GetComponentsInChildren<Text>()[0];
        textSpeed = GameObject.FindWithTag("Canvas").GetComponentsInChildren<Text>()[1];
        textLevel = GameObject.FindWithTag("Canvas").GetComponentsInChildren<Text>()[2];
        textResource = GameObject.FindWithTag("Canvas").GetComponentsInChildren<Text>()[3];
        textAddScore = GameObject.FindWithTag("Canvas").GetComponentsInChildren<Text>()[6];

        fuelProgress = GameObject.FindWithTag("Canvas").GetComponentsInChildren<Image>()[0];
        rocket_camera = Camera.main;

        coinSound = textAddScore.GetComponent<AudioSource>();
        resourceSound = textResource.GetComponent<AudioSource>();
        jumpSound = textSpeed.GetComponent<AudioSource>();
        loopMusic = textLevel.GetComponent<AudioSource>();
        gameOverSound = fuelProgress.GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("Sound") == "On")
        {
            loopMusic.loop = true;
            loopMusic.Play();
        }

        fly = new Vector3(0, speed, 0);
        childRocket = transform.GetComponentsInChildren<Transform>()[1];
        rbRocket = GetComponent<Rigidbody>();
        currentLevel = 255;
        boost = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textScore.text = $"Score: {score}";
        textSpeed.text = string.Format("Speed: {0:0.0}", speed);
        //textLevel.text = $"Level: {currentLevel + 1}";
        
        FuelConsuption();
        
        RocketMoving();
        RocketSpeed();
        IsNextLvl();
    }
    void ShowShield()
    {
        if (isShieldActive)
        {
            Animation shieldAnim = shieldSphere.GetComponent<Animation>();
            shieldAnim["Shield"].speed = 1 / resourceTime;
            shieldAnim.Play("Shield");
        }
    }
    void HyperJumpRotation()
    {
        if (isHyperJump)
        {
            childRocket.Rotate(rotation, speedRotate * Time.deltaTime);
            soploRotation = childRocket.rotation;
        }
    }
    void IsNextLvl()
    {
        switch (currentLevel)
        {
            case 255:
                currentLevel++;
                ChangeLevelEvent.Invoke(currentLevel);
                break;
            case 0:
                if (score >= 7)
                {
                    currentLevel++;
                    ChangeLevelEvent.Invoke(currentLevel);
                }
                break;
            case 1:
                if (score >= 15)
                {
                    currentLevel++;
                    ChangeLevelEvent.Invoke(currentLevel);
                }
                break;
            case 2:
                if (score >= 25)
                {
                    currentLevel++;
                    ChangeLevelEvent.Invoke(currentLevel);
                }
                break;
            case 3:
                if (score >= 35)
                {
                    currentLevel++;
                    ChangeLevelEvent.Invoke(currentLevel);
                }
                break;
            case 4:
                if (score >= 45)
                {
                    currentLevel++;
                    ChangeLevelEvent.Invoke(currentLevel);
                }
                break;
        }
        
    }
    #region Resourses and Abilities
    public void Break()
    {
        StartCoroutine(BreakCoroutine());
    }
    IEnumerator BreakCoroutine()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            resourceSound.Play();
        float saveSpeed = speed;
        speed /= 2;
        score += 2;
        PlayScoreAddAnim(2);
        yield return new WaitForSeconds(4 / resourceTime);
        speed = saveSpeed;
    }
    public void Freeze()
    {
        if (!isGameOver)
            StartCoroutine(FreezeCoroutine());
    }
    IEnumerator FreezeCoroutine()
    {
        // blue
        fuelProgress.color = new Color32(12, 102, 255, 255);
        isFuelFreeze = true;
        yield return new WaitForSeconds(freezeTime * resourceTime);
        // green
        fuelProgress.color = new Color32(17, 130, 1, 255);
        isFuelFreeze = false;
    }
    public void Shield()
    {
        if (!isGameOver)
            StartCoroutine(ShieldCoroutine());
    }
    IEnumerator ShieldCoroutine()
    {
        shieldSphere.SetActive(true);
        isShieldActive = true;
        ShowShield();
        AddScore();
        yield return new WaitForSeconds(shieldTime * resourceTime);
        shieldSphere.SetActive(false);
        isShieldActive = false;
    }
    
    public void HyperJump()
    {
        if (!isGameOver)
            if (speed < 100)
            StartCoroutine(HyperJumpCoroutine());
    }
    IEnumerator HyperJumpCoroutine()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            jumpSound.Play();
        float saveSpeed = speed;
        isHyperJump = true;
        speed = 100f;
        PlayScoreAddAnim(5);
        score += 5;
        SetAllCollidersStatus(false);
        float cameraAngle = rocket_camera.fieldOfView;
        StartCoroutine(CameraAnglePlusRoutine(cameraAngle));
        yield return new WaitForSeconds(jumpTime);
        StartCoroutine(CameraAngleMinusRoutine(cameraAngle));
        //rocket_camera.fieldOfView = cameraAngle;
        isHyperJump = false;
        speed = saveSpeed;
        SetAllCollidersStatus(true);
    }
    IEnumerator CameraAnglePlusRoutine(float cameraAngle)
    {
        while (rocket_camera.fieldOfView < 60)
        {
            rocket_camera.fieldOfView += 0.5f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator CameraAngleMinusRoutine(float cameraAngle)
    {
        while (rocket_camera.fieldOfView > cameraAngle)
        {
            rocket_camera.fieldOfView -= 1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    void HyperLevelChange()
    {
        StartCoroutine(HyperLevelChangeCoroutine());
    }
    IEnumerator HyperLevelChangeCoroutine()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            jumpSound.Play();
        float saveSpeed = speed;
        speed = 100;
        SetAllCollidersStatus(false);
        //GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1);
        speed = saveSpeed;
        //maxSpeed += 2;
        speed += 2;
        SetAllCollidersStatus(true);
        //GetComponent<Collider>().enabled = true;
        
    }
    /*void ScoreBoost()
    {
        if (!isGameOver)
        {
            StopCoroutine(ScoreBoostCoroutine());
            StartCoroutine("ScoreBoostCoroutine");
        }
    }
    IEnumerator ScoreBoostCoroutine()
    {
        boost = 2;
        yield return new WaitForSeconds(boostTime * resourceTime);
        boost = 1;
    }*/
    public IEnumerator AddFuel(float value)
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            resourceSound.Play();
        if (fuel == 0)
        {
            StartCoroutine(RegenerateSpeed());
        }
        fuel += value;
        fuelProgress.fillAmount = fuel;
        yield return null;
    }
    public IEnumerator AddFlashlight(float value)
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            resourceSound.Play();

        lightPower += value;
        yield return null;
    }
    public IEnumerator RegenerateSpeed()
    {
        while(speed < startSpeed)
        {
            speed += 0.02f;
            yield return null;
        }
        
    }
    #endregion

    public void SetAllCollidersStatus(bool active)
    {
        foreach (Collider c in GetComponents<Collider>())
        {
            c.enabled = active;
        }
    }
    public void AddScore()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
            coinSound.Play();
        PlayScoreAddAnim(boost);
        score += boost;
    }
    void PlayScoreAddAnim(int value)
    {
        textAddScore.text = $"+{value}";
        Animation scoreAnim = textAddScore.GetComponent<Animation>();
        scoreAnim["AddScoreAnim"].speed = speed / 2;
        scoreAnim.Play("AddScoreAnim");
    }

    void FuelConsuption()
    {
        if (isFuelFreeze)
            return;

        fuelProgress.fillAmount = fuel;

        if (fuel < 0.25)
            // red
            fuelProgress.color = new Color32(216, 1, 2, 255);
        else
            // green
            fuelProgress.color = new Color32(17, 130, 1, 255);

        if (fuel > 0)
            fuel -= 0.0003f * fuelConsumptionSpeed;
    }

    void RocketMoving()
    {
        if (!isGameOver && Joystick != null && !lockRocket)
        {
            float x = Joystick.Horizontal;
            float y = transform.position.y;
            float z = -Joystick.Vertical;
            if (!isShieldActive)
            {
                if(transform.name.Contains("StarBus"))
                    soploRotation = Quaternion.Euler(-90 + z * 3 , 0, -x * 3);
                else
                    soploRotation = Quaternion.Euler(z * 3, 0, -x * 3);
            }
            
            Vector3 correctPos = Vector3.zero;
            if (transform.position.z >= gameTunel && z >= 0)
            {
                //transform.position.z = 300;
                correctPos = new Vector3(transform.position.x, transform.position.y, gameTunel);
                z = 0;
            }
            else if (transform.position.z <= -gameTunel && z <= 0)
            {
                //transform.position.z = -300;
                correctPos = new Vector3(transform.position.x, transform.position.y, -gameTunel);
                z = 0;
            }
            if (transform.position.x >= gameTunel && x >= 0)
            {
                //transform.position.x = 300;
                correctPos = new Vector3(gameTunel, transform.position.y, transform.position.z);
                x = 0;
            }
            else if (transform.position.x <= -gameTunel && x <= 0)
            {
                //transform.position.x = -300;
                correctPos = new Vector3(-gameTunel, transform.position.y, transform.position.z);
                x = 0;
            }
            if (correctPos != Vector3.zero)
                transform.position = correctPos;
            float multiplier = speed >= 2 ? 10 * control * Mathf.Log(speed, 3) : 6 * control;
            Vector3 translate = (new Vector3(x, 0, z) * Time.deltaTime * multiplier);
            transform.Translate(translate);
        }
    }
    void RocketSpeed()
    {
        fly.y = speed;
        transform.position += fly;

        /*if (countOfFuel == 3)
        {
            //maxSpeed += 0.5f;
            speed += 0.5f;
            countOfFuel = 0;
        }*/
        if (fuel <= 0 && speed > 0.08f)
        {
            speed -= 0.02f;
        }
        if (fuel <= 0 && speed <= 0.08f)
        {
            GameOver();
        }
    }
    public void BlackHoleDeath()
    {
        lockRocket = true;
        //maxSpeed = 0.1f;
        speed = 0.1f;
    }
    public void GameOver()
    {
        if (!isGameOver && PlayerPrefs.GetString("Sound") == "On")
        {
            loopMusic.Stop();
            if (fuel > 0)
                gameOverSound.Play();
        }
        isGameOver = true;
        StartCoroutine(GameOverCoroutine());
    }
    IEnumerator GameOverCoroutine()
    {
        textGameOver = GameObject.FindWithTag("Canvas").GetComponentsInChildren<Text>()[4];
        textGameOver.text = "Game Over";
        textGameOver.GetComponent<Animation>().Play("LevelTextAnim");

        yield return new WaitForSeconds(3);

        int best = PlayerPrefs.GetInt("Best");
        if (best < score)
            PlayerPrefs.SetInt("Best", score);

        if (PlayerPrefs.HasKey("Coins"))
        {
            int coins = PlayerPrefs.GetInt("Coins");
            coins += score;
            PlayerPrefs.SetInt("Coins", coins);
        }
        else
        {
            PlayerPrefs.SetInt("Coins", score);
        }
        PlayerPrefs.SetInt("Score", score);
        SceneManager.LoadScene(2);
    }
}
