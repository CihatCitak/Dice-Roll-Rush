using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;

public class PlayerController : MonoBehaviour
{
    #region Singleton
    public static PlayerController Instance { get { return instance; } }
    private static PlayerController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    [SerializeField] InputSettings inputSettings;
    [SerializeField] DiceSettings diceSetting;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float forwardMovementSpeed = 1f, sideMovementSensitivity = 1f;
    [Header("Partical Effects")]
    [SerializeField] ParticleSystem levelUpPartical;
    [SerializeField] ParticleSystem levelDownPartical;

    private GameObject currentCharacterModel;

    private void Start()
    {
        DestroyOldCharacterModel();
        InstantiateNewCharacterModelInPlayerObject();
    }

    void Update()
    {
        if(GameManagement.Instance.GameState == GameManagement.GameStates.START)
            HandleForwardMovement();
    }

    public void OnNewLevel(DiceSettings settings, LevelChangeType levelChangeType)
    {
        ShowParticalEffectMakeItChildDestroyAfterOneSecond(levelChangeType);

        SetCharacterSettings(settings);

        DestroyOldCharacterModel();

        InstantiateNewCharacterModelInPlayerObject();
    }

    private void HandleForwardMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x + sideMovementSensitivity * inputSettings.InputDrag.x, 0, forwardMovementSpeed);
    }
    private void SetCharacterSettings(DiceSettings settings)
    {
        diceSetting = settings;
    }

    private void InstantiateNewCharacterModelInPlayerObject()
    {
        currentCharacterModel =
            Instantiate(diceSetting.DiceModelPrefab, transform.position, transform.rotation);

        currentCharacterModel.transform.parent = transform;
    }

    private void ShowParticalEffectMakeItChildDestroyAfterOneSecond(LevelChangeType levelChangeType)
    {
        if (levelChangeType == LevelChangeType.LEVELUP)
        {
            levelUpPartical.Play();
        }
        else
        {
            levelDownPartical.Play();
        }
    }

    private void DestroyOldCharacterModel()
    {
        if (currentCharacterModel != null)
        {
            Destroy(currentCharacterModel);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Money"))
        {
            StartCoroutine(WaitForCollectMoney());
        }
    }

    private IEnumerator WaitForCollectMoney()
    {
        yield return new WaitForSeconds(0.4f);

        GameManagement.Instance.EarnMoney(diceSetting.MoneyValue);
    }
}
