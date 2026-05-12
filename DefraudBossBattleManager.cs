using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DefraudBossBattleManager : MonoBehaviour
{
    public GameObject playerHealthTransaction;
    public GameObject defraudHealthTransaction;
    public GameObject fleeceHealthTransaction;

    public GameObject idiotSlapPanel;
    public GameObject unarmedAttackPanel;
    public GameObject unarmedDefendPanel;


    [Header("Effect Textures")]

    // This system is for one-turn moves only!!
    public GameObject effectPanel;
    public RawImage effectImage;

    // Player moves
    public Texture ultramarineMixTex;
    public Texture sonicSkeletonicTex;
    public Texture achooboomTex;
    public Texture treejuvenatumTex;
    public Texture cardioColaTex;
    public Texture pyroplasmaleTex;
    

    // Dr. deFraud moves
    public Texture napalmNectarTex;
    public ParticleSystem napalmParticles;

    // Fleece deFraud moves
    public Texture bladeTex;
    public Texture f22Tex;

    public GameObject sonicSkeletonicEffect;
    public ParticleSystem achooboomEffect;
    public GameObject elephantitanEffect;
    public GameObject tungstenskinEffect;
    public GameObject deepsleepreaperEffect;
    public GameObject staticSpookerEffect;
    public GameObject tongueTwisterEffect;
    public GameObject drillbacillusEffect;
    public GameObject spookerGhosts;

    public GameObject handPrefab;
    public RectTransform canvasRect;

    public float spawnDuration = 2.4f;
    public float spawnRate = 0.02f;
    public float fallSpeed = 300f;

    public AudioSource phaseOneTheme;
    public AudioSource phaseTwoTheme;
    public AudioSource dangerArises;
    public AudioSource phaseThreeTheme;
    public AudioSource activateMachine;
    public AudioSource pauseTheme;
    public float activateLoopStart = 4.8f;
    public float activateLoopEnd = 106f;
    public AudioSource fleeceDeath;
    public AudioSource defraudDeath;
    public float oneLoopStart = 23.5f;
    public float oneLoopEnd = 96.5f;
    public float twoLoopStart = 25.8f;
    public float twoLoopEnd = 138f;
    public float threeLoopStart = 22f;
    public float threeLoopEnd = 187.2f;
    public bool oneIntroPlayed = false;
    public bool twoIntroPlayed = false;
    public bool threeIntroPlayed = false;
    public bool activateIntroPlayed;

    public AudioSource playerGameOver;

    [Header("Player Move Sounds")]

    public AudioSource unarmedAttackSound;
    public AudioSource unarmedDefendSound;

    // Attack potions
    public AudioSource ultramarineMixSound;
    public AudioSource sonicSkeletonicSound;
    public AudioSource achooboomSound;

    // Defend potions
    public AudioSource elephantitanSound;
    public AudioSource tungstenskinSound;
    public AudioSource deepsleepreaperSound;

    // Heal potions
    public AudioSource treejuvenatumSound;
    public AudioSource cardioColaSound;
    public AudioSource pyroplasmaleSound;

    [Header("Defraud Move Sounds")]

    public AudioSource idiotSlap;

    // Moves
    public AudioSource slapRainSound;
    public AudioSource staticSpookerSound;
    public AudioSource unixQuickfixSound;
    public AudioSource drillbacillusSound;
    public AudioSource napalmNectarSound;
    public AudioSource pastBlastSound;

    [Header("Fleece Move Sounds")]

    public AudioSource catSound;
    public AudioSource bladeSound;
    public AudioSource lradSound;
    public AudioSource liquidNitrogenSound;
    public AudioSource f22Sound;


    public AudioSource machineChargingSound;
    public AudioSource machineReadySound;
    public AudioSource machineBlastSound;

    public AudioSource correctly;
    public AudioSource incorrectly;
    public AudioSource chaching;
    public AudioSource liquotholShieldSound;
    public AudioSource enterDestructionatron;



    public GameObject stunner;
    public GameObject frozenPanel;


    public GameObject[] deFraudFaces;
    public GameObject[] fleeceFaces;
    public GameObject[] liquotholFaces;

    int defraudFaceIndex;
    int fleeceFaceIndex;


    public GameObject[] defraudIIPanels;
    public GameObject[] defraudIIIPanels;
    public GameObject[] defraudFinalePanels;
    public GameObject[] defraudDeathPanels;
    public GameObject[] enemyWinPanels;
    public GameObject healButton, attackButton, defendButton, shopButton;
    public GameObject healPanel, attackPanel, defendPanel, shopPanel;
    public GameObject defSlider, playSlider, flceSlider;
    public GameObject cashPanel;
    public TextMeshProUGUI playerCashText;
    public TextMeshProUGUI elephantitanStatus, tungstenskinStatus, deepsleepreaperStatus;
    public TextMeshProUGUI treejuvenatumStatus, cardioColaStatus, pyroplasmaleStatus;
    public TextMeshProUGUI ultramarineMixStatus, sonicSkeletonicStatus, achooboomStatus;
    public TextMeshProUGUI narrator;
    public TextMeshProUGUI playerHealthText, defraudHealthText, fleeceHealthText;
    public TextMeshProUGUI desCountdown;
    public GameObject narrationPanel;
    public int playerCash, damage;
    public bool playerTurn;
    int playerHP = 600, deFraudHP = 6000, fleeceHP = 4000;
    int currentDefense = 0;
    bool defenseActive = false;
    public Slider playerSlider;
    public Slider deFraudSlider;
    public Slider fleeceSlider;
    int defraudMoveSelect;
    int fleeceMoveSelect;
    GameObject bonusQuestionPanel;
    public GameObject bonusOfferPanel, correctAnswerPanel, incorrectAnswerPanel;
    public GameObject destructionatronBeam;
    public GameObject pressSpacePanel;
    public GameObject desEffect;

    private int bonusCooldownTurns = 0;
    private const int BONUS_COOLDOWN = 2;
    private const int BONUS_REWARD = 450;

    public GameObject pauseMenu;
    public GameObject destructionatron;
    public GameObject desOnButton;
    public GameObject pauseButton;


    public int uQnumTurnGap;
    int currentActivePanel;
    int activeFleeceDeathPanel;
    int activeDefraudFinalePanel;
    int activeDefraudDeathPanel;


    public bool phaseOne;
    public bool phaseTwo;
    public bool phaseThree;
    public bool ending;

    public bool shieldChallengeActive;
    float shieldTimer;
    public int numSpaces;
    public GameObject liquotholShield;
    public GameObject fleeceProtection;

    public bool breakOneHappened;
    public bool breakTwoHappened;

    // Fleece move variables

    public bool canF22;
    public int catNumTurnGap;
    public int lNNumTurnGap;
    public int ldNumTurnGap;

    bool gameOverPlayed;

    public Animator defraudAnim;
    public Animator fleeceAnim;
    public Animator liquotholAnim;
    public Animator transition;
    public Animator congrats;
    public Animator transformAnim;
    public Animator fleeceWeaponsAnim;
    public Animator playerPotionsAnim;
    public Animator reaper;
    public Animator defraudPotionsAnim;
    public Animator unixQuickfixHeal;
    public Animator catHeal;

    public GameObject[] fleeceWeaponIcons;

    // Defraud Potion Move Helpers

    [System.Serializable]
    public class StaticSpookerStuff
    {
        public bool staticSpooked;
        public int numTurnGap;
    }

    [System.Serializable]
    public class TongueTwisterStuff
    {
        public int numTurnGap;
        public bool tongueTwisted;
        public int timesTwisted;
    }

    [System.Serializable]
    public class DrillBacillusStuff
    {
        public int numTurnsInf;
        public bool infected;
        public int numTurnGap;
    }

    [System.Serializable]
    public class LiquidNitrogenStuff
    {
        public bool frozen;
        public int numTurnsFrozen;
    }

    [System.Serializable]
    public class LRADStuff
    {
        public int timesDeafened;
        public bool deafened;
    }


    public enum PotionType
    {
        Elephantitan,
        Tungstenskin,
        Deepsleepreaper,
        Treejuvenatum,
        CardioCola,
        Pyroplasmale,
        UltramarineMix,
        SonicSkeletonic,
        Achooboom
    }

    public enum DefraudPotions 
    {
        UnixQuickfix,
        StaticSpooker,
        SlapRain,
        Drillbacillus,
        TongueTwister,
        NapalmNectar
    }

    public enum FleeceMoves
    {
        Cat,
        F22,
        LiquidNitrogen,
        Blade,
        LRAD
    }

    public enum TurnState
    {
        Player,
        Enemy,
        Busy
    }

    private TurnState turnState = TurnState.Player;

    [System.Serializable]
    public class PotionData
    {
        public string displayName;
        public int heal;
        public int attack;
        public float defense;
        public int cost;
        public int maxHold;
    }

    [System.Serializable]
    public class EnemyData
    {
        public string displayName;
        public int heal;
        public int attack;
    }

    public Dictionary<PotionType, PotionData> potionDB = new Dictionary<PotionType, PotionData>();
    public Dictionary<DefraudPotions, EnemyData> defraudPotionDB = new Dictionary<DefraudPotions, EnemyData>();
    public Dictionary<FleeceMoves, EnemyData> fleeceMoveDB = new Dictionary<FleeceMoves, EnemyData>();
    public GameObject[] moneyBackQuestions;
    public List<GameObject> correctlyAnsweredQuestions = new List<GameObject>();

    private PotionData potionToUse;
    private Dictionary<PotionType, int> inventory;
    private DrillBacillusStuff dbDetails = new DrillBacillusStuff();
    private TongueTwisterStuff ttDetails = new TongueTwisterStuff();
    private StaticSpookerStuff ssDetails = new StaticSpookerStuff();
    private LiquidNitrogenStuff lnDetails = new LiquidNitrogenStuff();
    private LRADStuff ldDetails = new LRADStuff();
    // Start is called before the first frame update
    void Start()
    {
        
        healButton.SetActive(false);
        attackButton.SetActive(false);
        defendButton.SetActive(false);
        shopButton.SetActive(false);
        cashPanel.SetActive(false);
        healPanel.SetActive(false);
        attackPanel.SetActive(false);
        defendPanel.SetActive(false);
        shopPanel.SetActive(false);
        flceSlider.SetActive(false);
        defSlider.SetActive(false);
        playSlider.SetActive(false);
        playerCash = 1000;
        bonusOfferPanel.SetActive(false);
        narrationPanel.SetActive(false);
        bonusOfferPanel.SetActive(false);
        correctAnswerPanel.SetActive(false);
        incorrectAnswerPanel.SetActive(false);
        destructionatron.SetActive(false);
        correctlyAnsweredQuestions.Clear();
        destructionatronBeam.SetActive(false);
        tongueTwisterEffect.SetActive(false);
        spookerGhosts.SetActive(false);
        gameOverPlayed = false;

        dbDetails.infected = false;
        dbDetails.numTurnsInf = 0;
        dbDetails.numTurnGap = 0;
        ssDetails.numTurnGap = 0;
        ttDetails.numTurnGap = 0;
        ttDetails.timesTwisted = 0;
        ldDetails.timesDeafened = 0;
        lnDetails.frozen = false;
        lnDetails.numTurnsFrozen = 0;
        ldNumTurnGap = 0;
        lNNumTurnGap = 0;
        catNumTurnGap = 0;
        ssDetails.staticSpooked = false;
        ttDetails.tongueTwisted = false;
        uQnumTurnGap = 0;
        currentActivePanel = 0;
        activeFleeceDeathPanel = 0;
        activeDefraudFinalePanel = 0;
        activeDefraudDeathPanel = 0;

        numSpaces = 0;
        ending = false;
        shieldTimer = 30f;
        shieldChallengeActive = false;
        desEffect.SetActive(false);

        phaseOne = true;
        phaseTwo = false;
        phaseThree = false;
        breakOneHappened = false;
        breakTwoHappened = false;

        desCountdown.enabled = false;
        pressSpacePanel.SetActive(false);
        fleeceProtection.SetActive(false);
        liquotholShield.SetActive(false);

        frozenPanel.SetActive(false);
        stunner.SetActive(false);
        pauseButton.SetActive(false);

        phaseTwoTheme.time = 12.5f;
;
        foreach (GameObject q in moneyBackQuestions)
        {
            q.SetActive(false);
        }

        foreach (GameObject face in fleeceFaces)
        {
            face.SetActive(false);
        }
        foreach (GameObject panel in defraudIIPanels)
        {
            panel.SetActive(false);
        }
        foreach (GameObject panel in defraudIIIPanels)
        {
            panel.SetActive(false);
        }
        foreach (GameObject panel in defraudFinalePanels)
        {
            panel.SetActive(false);
        }
        foreach (GameObject panel in defraudDeathPanels)
        {
            panel.SetActive(false);
        }
        foreach (GameObject f in liquotholFaces)
        {
            f.SetActive(false);
        }
        foreach (GameObject w in fleeceWeaponIcons)
        {
            w.SetActive(false);
        }
        liquotholFaces[0].SetActive(true);
        InitializePotionDB();
        InitializeDefraudDB();
        InitializeFleeceDB();
        InitializeInventory();
    }

    void InitializePotionDB()
    {
        potionDB = new Dictionary<PotionType, PotionData>()
        {
            { PotionType.UltramarineMix, new PotionData { displayName = "Ultramarine Mix", attack = 600, heal = 0, defense = 0, cost = 100, maxHold = 3 } },
            { PotionType.SonicSkeletonic, new PotionData { displayName = "Sonic Skeletonic", attack = 900, heal = 0, defense = 0, cost = 220, maxHold = 3 } },
            { PotionType.Achooboom, new PotionData { displayName = "Achooboom", attack = 1200, heal = 0, defense = 0, cost = 380, maxHold=3 } },

            { PotionType.Treejuvenatum, new PotionData { displayName = "Treejuvenatum", heal = 200, attack=0, defense=0, cost = 90, maxHold=3 } },
            { PotionType.CardioCola, new PotionData { displayName = "Cardio Cola", heal = 400, attack=0, defense=0, cost = 180, maxHold=3 } },
            { PotionType.Pyroplasmale, new PotionData { displayName = "Pyroplasmale", heal = 600, attack=0, defense = 0, cost= 330, maxHold=3 } },

            { PotionType.Elephantitan, new PotionData { displayName = "Elephantitan", defense = 0.5f, attack=0, heal=0, cost = 80, maxHold=3 } },
            { PotionType.Tungstenskin, new PotionData { displayName = "Tungstenskin", defense = 0.65f, attack=0, heal=0, cost = 150, maxHold=3 } },
            { PotionType.Deepsleepreaper, new PotionData { displayName = "Deepsleepreaper", defense = 0.8f, attack=0, heal=0, cost = 240, maxHold=3 } }
        };
    }

    void InitializeDefraudDB()
    {
        defraudPotionDB = new Dictionary<DefraudPotions, EnemyData>()
        {
            { DefraudPotions.SlapRain, new EnemyData { displayName = "Slap Rain", heal = 0, attack = 70 } },
            { DefraudPotions.TongueTwister, new EnemyData { displayName = "Tongue Twister", heal = 0, attack = 55 } },
            { DefraudPotions.StaticSpooker, new EnemyData { displayName = "Static Spooker", heal = 0, attack = 40 } },
            { DefraudPotions.UnixQuickfix, new EnemyData { displayName = "Unix Quickfix", heal = 350, attack = 0 } },
            { DefraudPotions.Drillbacillus, new EnemyData { displayName = "Drillbacillus", heal = 0, attack = 45 } },
            { DefraudPotions.NapalmNectar, new EnemyData { displayName = "Napalm Nectar", heal = 0, attack = 165 } },

        };
    }
    void InitializeFleeceDB()
    {
        fleeceMoveDB = new Dictionary<FleeceMoves, EnemyData>()
        {
            { FleeceMoves.Cat, new EnemyData { displayName = "Cat", heal = 500, attack = 0} },
            { FleeceMoves.Blade, new EnemyData { displayName = "Blade", heal = 0, attack = 65} },
            { FleeceMoves.LRAD, new EnemyData { displayName = "L.R.A.D", heal = 0, attack = 50} },
            { FleeceMoves.LiquidNitrogen, new EnemyData { displayName = "Liquid Nitrogen", heal = 0, attack = 20} },
            { FleeceMoves.F22, new EnemyData { displayName = "F-22", heal = 0, attack = 120} }
        };
    }
    void InitializeInventory()
    {
        inventory = new Dictionary<PotionType, int>();
        foreach (PotionType t in System.Enum.GetValues(typeof(PotionType)))
            inventory[t] = 0;
    }

    // Update is called once per frame
    void Update()
    {
        playerCashText.text = string.Format("CASH: $" + playerCash);
        elephantitanStatus.text = string.Format("ELEPHANTITAN (" + inventory[PotionType.Elephantitan] + ")");
        tungstenskinStatus.text = string.Format("TUNGSTENSKIN (" + inventory[PotionType.Tungstenskin] + ")");
        deepsleepreaperStatus.text = string.Format("DEEPSLEEPREAPER (" + inventory[PotionType.Deepsleepreaper] + ")");
        ultramarineMixStatus.text = string.Format("ULTRAMARINE MIX (" + inventory[PotionType.UltramarineMix] + ")");
        sonicSkeletonicStatus.text = string.Format("SONIC SKELETONIC (" + inventory[PotionType.SonicSkeletonic] + ")");
        achooboomStatus.text = string.Format("ACHOOBOOM (" + inventory[PotionType.Achooboom] + ")");
        treejuvenatumStatus.text = string.Format("TREEJUVENATUM (" + inventory[PotionType.Treejuvenatum] + ")");
        cardioColaStatus.text = string.Format("CARDIO COLA (" + inventory[PotionType.CardioCola] + ")");
        pyroplasmaleStatus.text = string.Format("PYROPLASMALE (" + inventory[PotionType.Pyroplasmale] + ")");

        playerSlider.value = playerHP;
        deFraudSlider.value = deFraudHP;
        fleeceSlider.value = fleeceHP;

        playerHealthText.text = string.Format(playerHP + "/600");
        defraudHealthText.text = string.Format(deFraudHP + "/6000");
        fleeceHealthText.text = string.Format(fleeceHP + "/4000");

        if (!oneIntroPlayed && phaseOneTheme.time >= oneLoopStart)
        {
            oneIntroPlayed = true;
            Debug.Log("Switched Intro Played variable!");
        }

        if (oneIntroPlayed && phaseOneTheme.time >= oneLoopEnd)
        {
            phaseOneTheme.timeSamples =
                (int)(oneLoopStart * phaseOneTheme.clip.frequency);

            if (!phaseOneTheme.isPlaying)
            {
                phaseOneTheme.Play();
            }
        }

        if (!twoIntroPlayed && phaseTwoTheme.time >= twoLoopStart)
        {
            twoIntroPlayed = true;
            Debug.Log("Switched Intro Played variable!");
        }

        if (twoIntroPlayed && phaseTwoTheme.time >= twoLoopEnd)
        {
            phaseTwoTheme.timeSamples =
                (int)(twoLoopStart * phaseTwoTheme.clip.frequency);

            if (!phaseTwoTheme.isPlaying)
            {
                phaseTwoTheme.Play();
            }
        }

        if (!threeIntroPlayed && phaseThreeTheme.time >= threeLoopStart)
        {
            threeIntroPlayed = true;
            Debug.Log("Switched Intro Played variable!");
        }

        if (threeIntroPlayed && phaseThreeTheme.time >= threeLoopEnd)
        {
            phaseThreeTheme.timeSamples =
                (int)(threeLoopStart * phaseThreeTheme.clip.frequency);

            if (!phaseThreeTheme.isPlaying)
            {
                phaseThreeTheme.Play();
            }
        }

        if (!activateIntroPlayed && activateMachine.time >= activateLoopStart)
        {
            threeIntroPlayed = true;
            Debug.Log("Switched Intro Played variable!");
        }

        if (activateIntroPlayed && activateMachine.time >= activateLoopEnd)
        {
            activateMachine.timeSamples =
                (int)(activateLoopStart * activateMachine.clip.frequency);

            if (!activateMachine.isPlaying)
            {
                activateMachine.Play();
            }
        }

        if (shieldChallengeActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                numSpaces++;
            }
        }

        if (playerHP <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    // The first active face in the array is defraudFaces[4]!!

    public void SetupFirstPlayerTurn()
    {

        defSlider.SetActive(true);
        playSlider.SetActive(true);
        healButton.SetActive(true);
        attackButton.SetActive(true);
        defendButton.SetActive(true);
        shopButton.SetActive(true);
        cashPanel.SetActive(true);
        pauseButton.SetActive(true);
    }
    
    public void ReturnToMoves()
    {
        healButton.SetActive(true);
        attackButton.SetActive(true);
        defendButton.SetActive(true);
        shopButton.SetActive(true);
        healPanel.SetActive(false);
        attackPanel.SetActive(false);
        defendPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void HealSelected()
    {
        healButton.SetActive(false);
        attackButton.SetActive(false);
        defendButton.SetActive(false);
        shopButton.SetActive(false);
        healPanel.SetActive(true);
    }

    public void DefendSelected()
    {
        healButton.SetActive(false);
        attackButton.SetActive(false);
        defendButton.SetActive(false);
        shopButton.SetActive(false);
        defendPanel.SetActive(true);
    }

    public void AttackSelected()
    {
        healButton.SetActive(false);
        attackButton.SetActive(false);
        defendButton.SetActive(false);
        shopButton.SetActive(false);
        attackPanel.SetActive(true);
    }

    public void ShopSelected()
    {
        healButton.SetActive(false);
        attackButton.SetActive(false);
        defendButton.SetActive(false);
        shopButton.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void BuyPotion(PotionType type)
    {
        PotionData p = potionDB[type];

        if (playerCash < p.cost) return;
        if (inventory[type] >= p.maxHold) return;

        inventory[type] ++;
        playerCash -= p.cost;
        chaching.Play();
    }

    public void BuyUltramarineMix(Button button)
    {
        BuyPotion(PotionType.UltramarineMix);
        if (inventory[PotionType.UltramarineMix] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuySonicSkeletonic(Button button)
    {
        BuyPotion(PotionType.SonicSkeletonic);
        if (inventory[PotionType.SonicSkeletonic] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuyAchooboom(Button button)
    {
        BuyPotion(PotionType.Achooboom);
        if (inventory[PotionType.Achooboom] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuyTreejuvenatum(Button button)
    {
        BuyPotion(PotionType.Treejuvenatum);
        if (inventory[PotionType.Treejuvenatum] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuyCardioCola(Button button)
    {
        BuyPotion(PotionType.CardioCola);
        if (inventory[PotionType.CardioCola] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuyDeepsleepreaper(Button button)
    {
        BuyPotion(PotionType.Deepsleepreaper);
        if (inventory[PotionType.Deepsleepreaper] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuyElephantitan(Button button)
    {
        BuyPotion(PotionType.Elephantitan);
        if (inventory[PotionType.Elephantitan] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuyTungstenskin(Button button)
    {
        BuyPotion(PotionType.Tungstenskin);
        if (inventory[PotionType.Tungstenskin] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }
    public void BuyPyroplasmale(Button button)
    {
        BuyPotion(PotionType.Pyroplasmale);
        if (inventory[PotionType.Pyroplasmale] >= 3)
        {
            Color c = button.GetComponent<Image>().color;
            c.a = 0.1f;
            button.GetComponent<Image>().color = c;
        }
    }

    public void UsePotion(PotionType type)
    {
        if (turnState != TurnState.Player) return;

        if (inventory[type] <= 0) return;

        PotionData p = potionDB[type];
        potionToUse = p;
        inventory[type]--;

        PotionUseOutline();
    }

    public void PotionUseOutline()
    {
        StartCoroutine(PotionUseNarration());
    }
    private IEnumerator PotionUseNarration()
    {
        turnState = TurnState.Busy;


        healButton.SetActive(false);
        healPanel.SetActive(false);
        attackButton.SetActive(false);
        attackPanel.SetActive(false);
        defendButton.SetActive(false);
        defendPanel.SetActive(false);
        cashPanel.SetActive(false);
        pauseButton.SetActive(false);

        narrator.text = string.Format("You use " + potionToUse.displayName + "!");
        yield return new WaitForSeconds(0.5f);
        narrationPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationPanel.SetActive(false);

        // Potion Animations here!!!

        switch (potionToUse.displayName)
        {
            case "Ultramarine Mix":

                effectImage.texture = ultramarineMixTex;
                playerPotionsAnim.Play("UseUltramarineMix", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                effectImage.color = new Color(1, 1, 1, 0.5f);
                ultramarineMixSound.Play();
                yield return new WaitForSeconds(1f);
                effectImage.color = new Color(0, 0, 0, 0);
                break;
            case "Sonic Skeletonic":

                effectImage.texture = sonicSkeletonicTex;
                playerPotionsAnim.Play("UseSonicSkeletonic", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                effectImage.color = new Color(1, 1, 1, 0.5f);
                sonicSkeletonicEffect.SetActive(true);
                sonicSkeletonicSound.Play();
                yield return new WaitForSeconds(1f);
                effectImage.color = new Color(0, 0, 0, 0);
                sonicSkeletonicEffect.SetActive(false);
                break;
            case "Achooboom":

                effectImage.texture = achooboomTex;
                playerPotionsAnim.Play("UseAchooboom", 0, 0f);
                yield return new WaitForSeconds(0.65f);
                achooboomSound.Play();
                yield return new WaitForSeconds(0.1f);
                effectImage.color = new Color(1, 1, 1, 0.5f);
                achooboomEffect.Play();
                yield return new WaitForSeconds(0.9f);
                effectImage.color = new Color(0, 0, 0, 0);
                break;
            case "Treejuvenatum":

                effectImage.texture = treejuvenatumTex;
                playerPotionsAnim.Play("UseTreejuvenatum", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                treejuvenatumSound.Play();
                effectImage.color = new Color(1, 1, 1, 0.5f);
                yield return new WaitForSeconds(2f);
                effectImage.color = new Color(0, 0, 0, 0);
                break;
            case "Cardio Cola":

                effectImage.texture = cardioColaTex;
                playerPotionsAnim.Play("UseCardioCola", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                effectImage.color = new Color(1, 1, 1, 0.5f);
                cardioColaSound.Play();
                yield return new WaitForSeconds(4f);
                effectImage.color = new Color(0, 0, 0, 0);
                break;
            case "Pyroplasmale":

                effectImage.texture = pyroplasmaleTex;
                playerPotionsAnim.Play("UsePyroplasmale", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                effectImage.color = new Color(1, 1, 1, 0.5f);
                pyroplasmaleSound.Play();
                yield return new WaitForSeconds(1f);
                effectImage.color = new Color(0, 0, 0, 0);
                break;
            case "Elephantitan":

                playerPotionsAnim.Play("UseElephantitan", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                elephantitanEffect.SetActive(true);
                elephantitanSound.Play();
                yield return new WaitForSeconds(2f);
                break;
            case "Tungstenskin":

                playerPotionsAnim.Play("UseTungstenskin", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                tungstenskinEffect.SetActive(true);
                tungstenskinSound.Play();
                yield return new WaitForSeconds(2f);
                break;
            case "Deepsleepreaper":

                playerPotionsAnim.Play("UseDeepsleepreaper", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                deepsleepreaperEffect.SetActive(true);
                deepsleepreaperSound.Play();
                reaper.Play("ReapersComing");
                yield return new WaitForSeconds(3f);
                break;

            default:

                Debug.Log("Dylan messed up LOL");
                break;

        }

        if (potionToUse.heal > 0)
        {
            HealPlayer(potionToUse.heal);
            yield return new WaitForSeconds(0.5f);
        }
            

        if (potionToUse.attack > 0)
        {
            DamageEnemy(potionToUse.attack);
            if (damage > 0)
            {
                if (phaseTwo)
                {
                    fleeceFaces[2].SetActive(false);
                    fleeceFaceIndex = Random.Range(7, 9);
                    fleeceFaces[fleeceFaceIndex].SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    fleeceFaces[fleeceFaceIndex].SetActive(false);
                    fleeceFaces[2].SetActive(true);
                }
                else
                {
                    deFraudFaces[4].SetActive(false);
                    defraudFaceIndex = Random.Range(9, 15);
                    deFraudFaces[defraudFaceIndex].SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    deFraudFaces[defraudFaceIndex].SetActive(false);
                    deFraudFaces[4].SetActive(true);
                }
            }
        }
            

        if (potionToUse.defense > 0)
            ApplyDefense(potionToUse.defense);

        yield return new WaitForSeconds(0.5f);

        if (deFraudHP < 3001 && fleeceHP > 0)
        {
            phaseTwo = true;
            phaseOne = false;
        }
        if (fleeceHP == 0 && deFraudHP > 1)
        {
            phaseThree = true;
            phaseTwo = false;
        }
        if (deFraudHP == 1)
        {
            phaseThree = false;
            ending = true;
        }

        if (ending)
        {
            StartCoroutine(DefraudsLastStand());
        }
        else if (phaseTwo && !breakOneHappened)
        {
            StartCoroutine(DefraudStopsFighting());
        }
        else if (phaseThree && !breakTwoHappened)
        {
            StartCoroutine(DeathOfFleece());
        }
        else if (phaseTwo && fleeceHP > 0)
        {
            StartCoroutine(FleeceTurn());
        }
        else
        {
            StartCoroutine(DrDefraudTurn());
        }     
    }
    public void UseUnarmedAtack()
    {
        StartCoroutine(UnarmedAttackSequence());
    }
    private IEnumerator UnarmedAttackSequence()
    {
        turnState = TurnState.Busy;


        healButton.SetActive(false);
        healPanel.SetActive(false);
        attackButton.SetActive(false);
        attackPanel.SetActive(false);
        defendButton.SetActive(false);
        defendPanel.SetActive(false);
        cashPanel.SetActive(false);
        pauseButton.SetActive(false);

        narrator.text = string.Format("You attack unarmed!");
        yield return new WaitForSeconds(0.5f);
        narrationPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationPanel.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        unarmedAttackPanel.SetActive(true);
        unarmedAttackSound.Play();
        yield return new WaitForSeconds(0.3f);
        unarmedAttackPanel.SetActive(false);
        DamageEnemy(150);
        if (damage > 0)
        {
            if (phaseTwo)
            {
                fleeceFaces[2].SetActive(false);
                fleeceFaceIndex = Random.Range(7, 9);
                fleeceFaces[fleeceFaceIndex].SetActive(true);
                yield return new WaitForSeconds(0.5f);
                fleeceFaces[fleeceFaceIndex].SetActive(false);
                fleeceFaces[2].SetActive(true);
            }
            else
            {
                deFraudFaces[4].SetActive(false);
                defraudFaceIndex = Random.Range(9, 15);
                deFraudFaces[defraudFaceIndex].SetActive(true);
                yield return new WaitForSeconds(0.5f);
                deFraudFaces[defraudFaceIndex].SetActive(false);
                deFraudFaces[4].SetActive(true);
            }
        }           
        yield return new WaitForSeconds(0.5f);

        if (deFraudHP < 3001 && fleeceHP > 0)
        {
            phaseTwo = true;
            phaseOne = false;
        }
        if (fleeceHP == 0 && deFraudHP > 1)
        {
            phaseThree = true;
            phaseTwo = false;
        }
        if (deFraudHP == 1)
        {
            phaseThree = false;
            ending = true;
        }

        if (ending)
        {
            StartCoroutine(DefraudsLastStand());
        }
        else if (phaseTwo && !breakOneHappened)
        {
            StartCoroutine(DefraudStopsFighting());
        }
        else if (phaseThree && !breakTwoHappened)
        {
            StartCoroutine(DeathOfFleece());
        }
        else if (phaseTwo && fleeceHP > 0)
        {
            StartCoroutine(FleeceTurn());
        }
        else
        {
            StartCoroutine(DrDefraudTurn());
        }
    }
    public void UseUnarmedDefense()
    {
        StartCoroutine(UnarmedDefenseSequence());
    }
    private IEnumerator UnarmedDefenseSequence()
    {
        turnState = TurnState.Busy;


        healButton.SetActive(false);
        healPanel.SetActive(false);
        attackButton.SetActive(false);
        attackPanel.SetActive(false);
        defendButton.SetActive(false);
        defendPanel.SetActive(false);
        cashPanel.SetActive(false);
        pauseButton.SetActive(false);
        potionToUse = null;

        narrator.text = string.Format("You defend unarmed!");
        yield return new WaitForSeconds(0.5f);
        narrationPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationPanel.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        unarmedDefendPanel.SetActive(true);
        unarmedDefendSound.Play();
        ApplyDefense(0.33f);
        yield return new WaitForSeconds(0.5f);
        if (phaseTwo && fleeceHP > 0)
        {
            StartCoroutine(FleeceTurn());
        }
        else
        {
            StartCoroutine(DrDefraudTurn());
        }
          
    }

    public void UseUltramarineMix(Button button)
    {
        UsePotion(PotionType.UltramarineMix);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UseSonicSkeletonic(Button button)
    {
        UsePotion(PotionType.SonicSkeletonic);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UseAchooboom(Button button)
    {
        UsePotion(PotionType.Achooboom);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UseTreejuvenatum(Button button)
    {
        UsePotion(PotionType.Treejuvenatum);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UseCardioCola(Button button)
    {
        UsePotion(PotionType.CardioCola);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UsePyroplasmale(Button button)
    {
        UsePotion(PotionType.Pyroplasmale);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UseElephantitan(Button button)
    {
        UsePotion(PotionType.Elephantitan);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UseTungstenskin(Button button)
    {
        UsePotion(PotionType.Tungstenskin);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }
    public void UseDeepsleepreaper(Button button)
    {
        UsePotion(PotionType.Deepsleepreaper);
        Color c = button.GetComponent<Image>().color;
        c.a = 1f;
        button.GetComponent<Image>().color = c;
    }

    void HealPlayer(int amount)
    {
        if (dbDetails.infected)
        {
            dbDetails.infected = false;
            drillbacillusEffect.SetActive(false);
            dbDetails.numTurnsInf = 0;
            dbDetails.numTurnGap = 0;
        }
        if (ttDetails.tongueTwisted)
        {
            ttDetails.timesTwisted = 0;
            ttDetails.numTurnGap = 0;
            tongueTwisterEffect.SetActive(false);
            ttDetails.tongueTwisted = false;
        }
        if (ldDetails.deafened)
        {
            ldDetails.timesDeafened = 0;
            ldNumTurnGap = 0;
            ldDetails.deafened = false;
            stunner.SetActive(false);
        }
        playerHP = Mathf.Min(playerHP + amount, 600);
        playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("+" + amount);
        playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 251, 0, 0);
        playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
    }

    void DamageEnemy(int dmg)
    {
        if (ttDetails.tongueTwisted)
        {
            for (int i = 0; i < ttDetails.timesTwisted; i++)
            {
                dmg = (int)(dmg * 0.75f);
            }

            int missProb = Random.Range(0, 20);
            if (missProb < 8) dmg = 0;
        }

        if (phaseTwo && fleeceHP > 0)
        {
            if (ldDetails.deafened)
            {
                for (int i = 0; i < ldDetails.timesDeafened; i++)
                {
                    dmg = (int)(dmg * 0.75f);
                }

                int missProb = Random.Range(0, 20);
                if (missProb < 4) dmg = 0;
            }
            fleeceHP = Mathf.Max(fleeceHP - dmg, 0);
            if (dmg == 0)
            {
                fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("MISS");
                fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(120, 120, 120, 0);
            }
            else
            {
                fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + dmg);
                fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
            }
            
            fleeceHealthTransaction.GetComponent<Animator>().Play("FleeceHealthTransaction", 0, 0f);
        }
        else
        {
            deFraudHP = Mathf.Max(deFraudHP - dmg, 1);

            if (dmg == 0)
            {
                defraudHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("MISS");
                defraudHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(120, 120, 120, 0);
            }
            else
            {
                defraudHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + dmg);
                defraudHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
            }               
            defraudHealthTransaction.GetComponent<Animator>().Play("DefraudHealthTransaction", 0, 0f);
        }

        damage = dmg;
        
    }

    void ApplyDefense(float amount)
    {
        currentDefense = (int)Mathf.Max(currentDefense, amount);
        defenseActive = true;
    }
    
    private IEnumerator DrDefraudTurn()
    {
        turnState = TurnState.Enemy;

        yield return new WaitForSeconds(0.75f);

        DefraudPotions move = SelectDefraudMove();
        EnemyData data = defraudPotionDB[move];

        deFraudFaces[4].SetActive(false);
        do
        {
            defraudFaceIndex = Random.Range(0, 9);
        }
        while (defraudFaceIndex == 4);
        deFraudFaces[defraudFaceIndex].SetActive(true);
        narrator.text = "Dr. deFraud uses " + data.displayName + "!";
        narrationPanel.SetActive(true);
        if (uQnumTurnGap > 0) uQnumTurnGap--;
        if (ssDetails.numTurnGap > 0) ssDetails.numTurnGap--;
        if (dbDetails.numTurnGap > 0) dbDetails.numTurnGap--;
        if (dbDetails.numTurnsInf == 0)
        {
            drillbacillusEffect.SetActive(false);
            dbDetails.infected = false;
        }

        if (move == DefraudPotions.UnixQuickfix) uQnumTurnGap = 5;
        if (move == DefraudPotions.StaticSpooker) ssDetails.numTurnGap = 2;
        if (move == DefraudPotions.Drillbacillus) dbDetails.numTurnGap = 6;
        if (ssDetails.staticSpooked)
        {
            ssDetails.staticSpooked = false;
            staticSpookerEffect.SetActive(false);
            spookerGhosts.SetActive(false);
        }
        yield return new WaitForSeconds(3f);
        narrationPanel.SetActive(false);

        // deFraud's potion animations go here!!

        switch (data.displayName)
        {

            case "Slap Rain":

                defraudPotionsAnim.Play("UseSlapRain", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                StartCoroutine(SpawnRain());
                slapRainSound.Play();
                yield return new WaitForSeconds(3.5f);
                break;
            case "Tongue Twister":

                defraudPotionsAnim.Play("UseTongueTwister", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                break;
            case "Static Spooker":

                defraudPotionsAnim.Play("UseStaticSpooker", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                if (!defenseActive) staticSpookerSound.Play();
                break;
            case "Unix Quickfix":

                defraudPotionsAnim.Play("UseUnixQuickfix", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                unixQuickfixHeal.Play("UnixQuickfixHeal", 0, 0f);
                yield return new WaitForSeconds(0.5f);
                unixQuickfixSound.Play();
                yield return new WaitForSeconds(1f);
                break;
            case "Drillbacillus":

                defraudPotionsAnim.Play("UseDrillbacillus", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                if (!defenseActive) drillbacillusSound.Play();
                break;
            case "Napalm Nectar":

                effectImage.texture = napalmNectarTex;
                defraudPotionsAnim.Play("UseNapalmNectar", 0, 0f);
                yield return new WaitForSeconds(0.75f);
                effectImage.color = new Color(1, 1, 1, 0.5f);
                napalmParticles.Play();
                napalmNectarSound.Play();
                yield return new WaitForSeconds(5f);
                effectImage.color = new Color(0, 0, 0, 0);
                break;

            default:

                Debug.Log("Dylan messed up yet again!!");
                break;
        }

        if (data.heal > 0)
        {
            deFraudHP = Mathf.Min(deFraudHP + data.heal, 6000);
            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("+" + data.heal);
            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(169, 71, 255, 0);
            defraudHealthTransaction.GetComponent<Animator>().Play("DefraudHealthTransaction", 0, 0f);
            yield return new WaitForSeconds(0.5f);
        }
            

        if (data.attack > 0)
        {
            int finalDamage = data.attack;

            if (defenseActive)
            {
                defenseActive = false;

                if (potionToUse == null) {

                    finalDamage = (int)(finalDamage - (finalDamage * 0.33f));
                    playerHP = Mathf.Max(playerHP - finalDamage, 0);
                    playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                    playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                    playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                }
                else
                {
                    int hitProb = Random.Range(0, 10);
                    finalDamage = (int)(finalDamage - (finalDamage * potionToUse.defense));

                    currentDefense = 0;
                    if (hitProb < 5)
                    {
                        if (potionToUse.displayName == "Elephantitan")
                        {
                            deFraudHP = (int)Mathf.Max(deFraudHP - data.attack, 1);
                            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + data.attack);
                            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            defraudHealthTransaction.GetComponent<Animator>().Play("DefraudHealthTransaction", 0, 0f);
                            deFraudFaces[defraudFaceIndex].SetActive(false);
                            defraudFaceIndex = Random.Range(9, 15);
                            deFraudFaces[defraudFaceIndex].SetActive(true);
                            playerHP = Mathf.Max(playerHP - finalDamage, 0);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                            yield return new WaitForSeconds(0.5f);
                            deFraudFaces[defraudFaceIndex].SetActive(false);
                            deFraudFaces[4].SetActive(true);
                        }
                        else if (potionToUse.displayName == "Tungstenskin")
                        {
                            deFraudHP = (int)Mathf.Max(deFraudHP - (data.attack * 1.5f), 1);
                            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + (int)(data.attack * 1.5f));
                            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            defraudHealthTransaction.GetComponent<Animator>().Play("DefraudHealthTransaction", 0, 0f);
                            deFraudFaces[defraudFaceIndex].SetActive(false);
                            defraudFaceIndex = Random.Range(9, 15);
                            deFraudFaces[defraudFaceIndex].SetActive(true);
                            playerHP = Mathf.Max(playerHP - finalDamage, 0);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                            yield return new WaitForSeconds(0.5f);
                            deFraudFaces[defraudFaceIndex].SetActive(false);
                            deFraudFaces[4].SetActive(true);
                        }
                        else
                        {
                            deFraudHP = (int)Mathf.Max(deFraudHP - (data.attack * 2f), 1);
                            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + (int)(data.attack * 2f));
                            defraudHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            defraudHealthTransaction.GetComponent<Animator>().Play("DefraudHealthTransaction", 0, 0f);
                            deFraudFaces[defraudFaceIndex].SetActive(false);
                            defraudFaceIndex = Random.Range(9, 15);
                            deFraudFaces[defraudFaceIndex].SetActive(true);
                            playerHP = Mathf.Max(playerHP - finalDamage, 0);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                            yield return new WaitForSeconds(0.5f);
                            deFraudFaces[defraudFaceIndex].SetActive(false);
                            deFraudFaces[4].SetActive(true);
                        }
                        
                    }
                    else
                    {
                        playerHP = Mathf.Max(playerHP - finalDamage, 0);
                        playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                        playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                        playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                        yield return new WaitForSeconds(0.5f);
                    }
                    yield return new WaitForSeconds(0.3f);

                    if (elephantitanEffect.activeInHierarchy) elephantitanEffect.SetActive(false);
                    if (tungstenskinEffect.activeInHierarchy) tungstenskinEffect.SetActive(false);
                    if (deepsleepreaperEffect.activeInHierarchy)
                    {
                        deepsleepreaperEffect.SetActive(false);
                        reaper.Play("ReapersLeaving");
                    }

                    yield return new WaitForSeconds(0.5f);

                    if (deFraudHP < 3001)
                    {
                        phaseTwo = true;
                        phaseOne = false;
                    }

                    if (phaseTwo && !breakOneHappened)
                    {
                        StartCoroutine(DefraudStopsFighting());
                        yield break;
                    }

                    if (deFraudHP == 1)
                    {
                        phaseThree = false;
                        ending = true;
                    }

                    if (ending)
                    {
                        StartCoroutine(DefraudsLastStand());
                        yield break;
                    }
                }
            }
            else
            {
                if (data.displayName == "Static Spooker")
                {
                    ssDetails.staticSpooked = true;
                    staticSpookerEffect.SetActive(true);
                    spookerGhosts.SetActive(true);
                    ssDetails.numTurnGap = 2;
                }
                else if (data.displayName == "Unix Quickfix")
                {
                    uQnumTurnGap = 5;
                }
                else if (data.displayName == "Tongue Twister")
                {
                    ttDetails.tongueTwisted = true;
                    tongueTwisterEffect.SetActive(true);
                    ttDetails.timesTwisted++;
                }
                else if (data.displayName == "Drillbacillus")
                {
                    dbDetails.infected = true;
                    drillbacillusEffect.SetActive(true);
                    dbDetails.numTurnsInf = 2;
                }
                else
                {

                }

                playerHP = Mathf.Max(playerHP - finalDamage, 0);
                playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
            }
        }

        yield return new WaitForSeconds(0.75f);
        deFraudFaces[defraudFaceIndex].SetActive(false);
        deFraudFaces[4].SetActive(true);

        yield return new WaitForSeconds(0.4f);
        if (unarmedDefendPanel.activeInHierarchy) unarmedDefendPanel.SetActive(false);
        if (elephantitanEffect.activeInHierarchy) elephantitanEffect.SetActive(false);
        if (tungstenskinEffect.activeInHierarchy) tungstenskinEffect.SetActive(false);
        if (deepsleepreaperEffect.activeInHierarchy)
        {
            deepsleepreaperEffect.SetActive(false);
            reaper.Play("ReapersLeaving");
        }

        yield return new WaitForSeconds(0.5f);

        if (dbDetails.infected)
        {
            if (dbDetails.numTurnsInf > 0)
            {
                yield return new WaitForSeconds(2f);
                playerHP -= 25;
                playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-25");
                playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                dbDetails.numTurnsInf--;
            }
            else
            {
                yield return new WaitForSeconds(2f);
                dbDetails.infected = false;
                dbDetails.numTurnGap = 0;
                dbDetails.numTurnsInf = 0;
            }
        }

        // Handle the bonus question logic
        if (bonusCooldownTurns > 0)
        {
            bonusCooldownTurns--;

            ReturnControlToPlayer();
            yield break;
        }

        bool offerBonus = false;

        if (playerCash < 276)
        {
            offerBonus = true;
        }
        else
        {          
            offerBonus = (Random.Range(0, 10) == 6);
        }

        if (offerBonus)
        {
            bonusOfferPanel.SetActive(true);
        }
        else
        {
            ReturnControlToPlayer();
        }
    }

    private DefraudPotions SelectDefraudMove()
    {
        int roll = Random.Range(0, 100);
        Debug.Log(roll);
        bool canUseUnixQuickfix = uQnumTurnGap == 0;
        bool canUseStaticSpooker = ssDetails.numTurnGap == 0;
        bool canDrillbacillus = dbDetails.numTurnGap == 0;

        if (phaseOne)
        {
        
            if (!canUseStaticSpooker && !canUseUnixQuickfix)
            {
                
                if (roll < 75) return DefraudPotions.SlapRain;
                return DefraudPotions.TongueTwister;
            }

            if (!canUseStaticSpooker)
            {
                if (roll < 65) return DefraudPotions.SlapRain;
                if (roll < 90) return DefraudPotions.TongueTwister;
                return DefraudPotions.UnixQuickfix;
            }

            if (!canUseUnixQuickfix)
            {
                if (roll < 65) return DefraudPotions.SlapRain;
                if (roll < 90) return DefraudPotions.TongueTwister;
                return DefraudPotions.StaticSpooker;
            }

            // All available
            if (roll < 45) return DefraudPotions.SlapRain;
            if (roll < 75) return DefraudPotions.TongueTwister;
            if (roll < 90) return DefraudPotions.StaticSpooker;
            return DefraudPotions.UnixQuickfix;
        }
        else
        {
            if (!canUseStaticSpooker && !canUseUnixQuickfix && !canDrillbacillus)
            {

                if (roll < 60) return DefraudPotions.SlapRain;
                if (roll < 92) return DefraudPotions.TongueTwister;
                return DefraudPotions.NapalmNectar;
            }

            if (!canUseStaticSpooker && !canUseUnixQuickfix)
            {
                if (roll < 60) return DefraudPotions.SlapRain;
                if (roll < 85) return DefraudPotions.TongueTwister;
                if (roll < 95) return DefraudPotions.Drillbacillus;
                return DefraudPotions.NapalmNectar;
            }

            if (!canUseUnixQuickfix && !canDrillbacillus)
            {
                if (roll < 55) return DefraudPotions.SlapRain;
                if (roll < 75) return DefraudPotions.TongueTwister;
                if (roll < 90) return DefraudPotions.StaticSpooker;
                return DefraudPotions.NapalmNectar;
            }
            if (!canUseStaticSpooker && !canDrillbacillus)
            {
                if (roll < 64) return DefraudPotions.SlapRain;
                if (roll < 84) return DefraudPotions.TongueTwister;
                if (roll < 94) return DefraudPotions.UnixQuickfix;
                return DefraudPotions.NapalmNectar;
            }
            if (!canDrillbacillus)
            {
                if (roll < 45) return DefraudPotions.SlapRain;
                if (roll < 75) return DefraudPotions.TongueTwister;
                if (roll < 87) return DefraudPotions.StaticSpooker;
                if (roll < 95) return DefraudPotions.UnixQuickfix;
                return DefraudPotions.NapalmNectar;
            }
            if (!canUseStaticSpooker)
            {
                if (roll < 50) return DefraudPotions.SlapRain;
                if (roll < 72) return DefraudPotions.TongueTwister;
                if (roll < 87) return DefraudPotions.Drillbacillus;
                if (roll < 95) return DefraudPotions.UnixQuickfix;
                return DefraudPotions.NapalmNectar;
            }
            if (!canUseUnixQuickfix)
            {
                if (roll < 40) return DefraudPotions.SlapRain;
                if (roll < 70) return DefraudPotions.TongueTwister;
                if (roll < 85) return DefraudPotions.StaticSpooker;
                if (roll < 95) return DefraudPotions.Drillbacillus;
                return DefraudPotions.NapalmNectar;
            }
            if (roll < 35) return DefraudPotions.SlapRain;
            if (roll < 50) return DefraudPotions.TongueTwister;
            if (roll < 70) return DefraudPotions.StaticSpooker;
            if (roll < 85) return DefraudPotions.Drillbacillus;
            if (roll < 95) return DefraudPotions.UnixQuickfix;
            return DefraudPotions.NapalmNectar;
        }
    }

    private FleeceMoves SelectFleeceMove()
    {
        int roll = Random.Range(0, 100);
        Debug.Log(roll);

        bool canUseLN = lNNumTurnGap == 0;
        bool canUseLD = ldNumTurnGap == 0;
        bool canUseCat = catNumTurnGap == 0;

        if (lNNumTurnGap == 5) canF22 = true;
        // First scenario: All three of these are cooling down

        if (!canUseLN && !canUseLD && !canUseCat)
        {
            if (!canF22) return FleeceMoves.Blade;
            else
            {
                if (roll < 65) return FleeceMoves.Blade;
                return FleeceMoves.F22;
            }
        }
        // Second scenario: Two of these are cooling down
        else if (!canUseLN && !canUseCat)
        {
            if (!canF22)
            {
                if (roll < 65) return FleeceMoves.Blade;
                return FleeceMoves.LRAD;
            }
            else
            {
                if (roll < 45) return FleeceMoves.Blade;
                if (roll < 75) return FleeceMoves.LRAD;
                return FleeceMoves.F22;
            }
        }
        else if (!canUseLN && !canUseLD)
        {
            if (!canF22)
            {
                if (roll < 85) return FleeceMoves.Blade;
                return FleeceMoves.Cat;
            }
            else
            {
                if (roll < 55) return FleeceMoves.Blade;
                if (roll < 80) return FleeceMoves.F22;
                return FleeceMoves.Cat;
            }
        }
        else if (!canUseLD && !canUseCat)
        {
            if (roll < 50) return FleeceMoves.Blade;
            if (roll < 80) return FleeceMoves.F22;
            return FleeceMoves.LiquidNitrogen;
        }
        // Third scenario: One of these is cooling down
        else if (!canUseLN)
        {
            if (!canF22)
            {
                if (roll < 55) return FleeceMoves.Blade;
                if (roll < 85) return FleeceMoves.LRAD;
                return FleeceMoves.Cat;
            }
            else
            {
                if (roll < 40) return FleeceMoves.Blade;
                if (roll < 65) return FleeceMoves.LRAD;
                if (roll < 90) return FleeceMoves.F22;
                return FleeceMoves.Cat;
            }
        }
        else if (!canUseCat)
        {
            if (roll < 40) return FleeceMoves.Blade;
            if (roll < 65) return FleeceMoves.LRAD;
            if (roll < 85) return FleeceMoves.F22;
            return FleeceMoves.LiquidNitrogen;
        }
        else if (!canUseLD)
        {
            if (roll < 45) return FleeceMoves.Blade;
            if (roll < 70) return FleeceMoves.F22;
            if (roll < 90) return FleeceMoves.LiquidNitrogen;
            return FleeceMoves.Cat;
        }
        // Fourth scenario: NO cooldowns!!!
        else
        {
            if (roll < 40) return FleeceMoves.Blade;
            if (roll < 65) return FleeceMoves.LRAD;
            if (roll < 85) return FleeceMoves.F22;
            if (roll < 95) return FleeceMoves.LiquidNitrogen;
            return FleeceMoves.Cat;
        }
        
    }

    public void ReturnControlToPlayer()
    {
        defenseActive = false;
        if (playerHP <= 0 || deFraudHP <= 0)
        {
            return;
        }

        turnState = TurnState.Player;

        if (ssDetails.staticSpooked)
        {
            StartCoroutine(DrDefraudTurn());
            return;
        }

        if (lnDetails.frozen)
        {
            StartCoroutine(FleeceTurn());
            return;
        }

        healButton.SetActive(true);
        attackButton.SetActive(true);
        defendButton.SetActive(true);
        shopButton.SetActive(true);
        cashPanel.SetActive(true);
        pauseButton.SetActive(true);
    }

    public void AcceptBonusQuestion()
    {
        bonusCooldownTurns = BONUS_COOLDOWN;
        StartCoroutine(BonusQuestionSequence());
    }

    private IEnumerator BonusQuestionSequence()
    {
        bonusOfferPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        BonusQuestionSelected();
    }

    public void BonusQuestionSelected()
    {
        int idx = 0;

        do
        {
            idx = Random.Range(0, moneyBackQuestions.Length);
            bonusQuestionPanel = moneyBackQuestions[idx];

        } while (correctlyAnsweredQuestions.Contains(bonusQuestionPanel));

        bonusQuestionPanel.SetActive(true);
    }
    public void NoBonusQuestion()
    {
        StartCoroutine(NoBonusSequence());
    }
    private IEnumerator NoBonusSequence()
    {
        bonusOfferPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        ReturnControlToPlayer();
    }

    public void BonusQuestionCorrect()
    {
        StartCoroutine(CorrectQuestionReward());
    }
    private IEnumerator CorrectQuestionReward()
    {
        bonusQuestionPanel.SetActive(false);
        yield return new WaitForSeconds(2f);
        deFraudFaces[4].SetActive(false);
        deFraudFaces[13].SetActive(true);
        if (phaseTwo)
        {
            fleeceFaces[2].SetActive(false);
            fleeceFaces[8].SetActive(true);
        }
        correctly.Play();
        correctAnswerPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        correctAnswerPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        playerCash += BONUS_REWARD;
        playerCash = Mathf.Min(playerCash, 1000);

        yield return new WaitForSeconds(1f);
        correctlyAnsweredQuestions.Add(bonusQuestionPanel);
        if (correctlyAnsweredQuestions.Count == moneyBackQuestions.Length)
        {
            correctlyAnsweredQuestions.Clear();
        }
        deFraudFaces[13].SetActive(false);
        deFraudFaces[4].SetActive(true);
        if (phaseTwo)
        {
            fleeceFaces[2].SetActive(true);
            fleeceFaces[8].SetActive(false);
        }
        ReturnControlToPlayer();
    }


    public void BonusQuestionIncorrect()
    {
        StartCoroutine(IdiotSlap());
    }
    private IEnumerator IdiotSlap()
    {
        bonusQuestionPanel.SetActive(false);
        yield return new WaitForSeconds(2f);
        deFraudFaces[4].SetActive(false);
        deFraudFaces[6].SetActive(true);
        if (phaseTwo)
        {
            fleeceFaces[2].SetActive(false);
            fleeceFaces[6].SetActive(true);
        }
        incorrectly.Play();
        incorrectAnswerPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        incorrectAnswerPanel.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        idiotSlapPanel.SetActive(true);
        idiotSlap.Play();
        yield return new WaitForSeconds(0.4f);
        idiotSlapPanel.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        playerHP = Mathf.Max(playerHP - 25, 0);
        playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-25");
        playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
        playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
        yield return new WaitForSeconds(1f);
        deFraudFaces[6].SetActive(false);
        deFraudFaces[4].SetActive(true);
        if (phaseTwo)
        {
            fleeceFaces[2].SetActive(true);
            fleeceFaces[6].SetActive(false);
        }
        ReturnControlToPlayer();
    }

    private IEnumerator DefraudStopsFighting()
    {
        phaseOneTheme.Stop();
        defSlider.SetActive(false);
        playSlider.SetActive(false);
        cashPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        activateMachine.Play();
        if (dbDetails.infected)
        {
            dbDetails.infected = false;
            drillbacillusEffect.SetActive(false);
            dbDetails.numTurnsInf = 0;
            dbDetails.numTurnGap = 0;
        }
        if (ttDetails.tongueTwisted)
        {
            ttDetails.timesTwisted = 0;
            ttDetails.numTurnGap = 0;
            tongueTwisterEffect.SetActive(false);
            ttDetails.tongueTwisted = false;
        }
        if (ldDetails.deafened)
        {
            ldDetails.timesDeafened = 0;
            ldNumTurnGap = 0;
            ldDetails.deafened = false;
        }
        deFraudFaces[4].SetActive(false);
        deFraudFaces[12].SetActive(true);
        defraudAnim.Play("DefraudMidFightMoveForward");
        yield return new WaitForSeconds(2f);
        defraudIIPanels[currentActivePanel].SetActive(true);
        breakOneHappened = true;
    }

    public void NextMidDefraudPanel()
    {
        if (currentActivePanel == 2)
        {
            StartCoroutine(ActivateMachine());
        }
        else if (currentActivePanel == 5)
        {
            if (activateMachine.isPlaying) activateMachine.Stop();
            phaseTwoTheme.Play();
            StartCoroutine(SummonFleece());
        }
        else if (currentActivePanel == defraudIIPanels.Length - 1)
        {
            StartCoroutine(FleecesFightPlaces());
        }
        else
        {
            StartCoroutine(DisplayNextMidPanel());
        }

    }

    private IEnumerator DisplayNextMidPanel()
    {
        defraudIIPanels[currentActivePanel].SetActive(false);
        if (currentActivePanel == 0)
        {
            deFraudFaces[12].SetActive(false);
            deFraudFaces[10].SetActive(true);
        }
        else if (currentActivePanel < 3)
        {
            deFraudFaces[13].SetActive(false);
            deFraudFaces[10].SetActive(true);
        }
        else if (currentActivePanel == 6)
        {
            fleeceFaces[2].SetActive(true);
            fleeceFaces[0].SetActive(false);
        }
        else 
        {
            for (int i = 0; i < 9; i++)
            {
                deFraudFaces[i].SetActive(false);
            }
            deFraudFaces[5].SetActive(true);
        }
        currentActivePanel++;
        yield return new WaitForSeconds(0.5f);
        if (currentActivePanel == 2)
        {
            deFraudFaces[10].SetActive(false);
            deFraudFaces[0].SetActive(true);
        }
        else if (currentActivePanel == 5)
        {
            deFraudFaces[5].SetActive(false);
            deFraudFaces[6].SetActive(true);
        }
        else if (currentActivePanel == 7)
        {
            deFraudFaces[4].SetActive(false);
            deFraudFaces[0].SetActive(true);
        }
        else if (currentActivePanel >= 0 && currentActivePanel <= 1)
        {
            deFraudFaces[10].SetActive(false);
            deFraudFaces[13].SetActive(true);
        }  
        else
        {
            deFraudFaces[5].SetActive(false);
            deFraudFaces[0].SetActive(true);
        }

        if (currentActivePanel == defraudIIPanels.Length - 1)
        {
            fleeceFaces[2].SetActive(false);
            fleeceFaces[5].SetActive(true);
        }
        defraudIIPanels[currentActivePanel].SetActive(true);
    }

    private IEnumerator ActivateMachine()
    {
        
        defraudIIPanels[currentActivePanel].SetActive(false);
        deFraudFaces[0].SetActive(false);
        deFraudFaces[1].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        destructionatron.SetActive(true);
        Vector3 startPos = destructionatron.transform.localPosition;
        Vector3 targetPos = new Vector3(
            startPos.x,
            startPos.y,
            0.049f
        );

        float duration = 9f;
        float elapsed = 0f;
        enterDestructionatron.Play();
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            destructionatron.transform.localPosition =
                Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }
        desOnButton.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(2f);
        currentActivePanel++;
        deFraudFaces[1].SetActive(false);
        deFraudFaces[7].SetActive(true);
        defraudIIPanels[currentActivePanel].SetActive(true);
    }
    private IEnumerator SummonFleece()
    {
        defraudIIPanels[currentActivePanel].SetActive(false);
        yield return new WaitForSeconds(1.5f);
        deFraudFaces[6].SetActive(false);
        deFraudFaces[4].SetActive(true);
        narrationPanel.SetActive(true);
        narrator.text = string.Format("Dr. deFraud uses Past Blast!");
        yield return new WaitForSeconds(5f);
        narrationPanel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        defraudPotionsAnim.Play("UsePastBlast", 0, 0f);
        yield return new WaitForSeconds(1f);
        transformAnim.Play("PastBlastEffect");
        pastBlastSound.Play();
        yield return new WaitForSeconds(1f);
        defraudAnim.Play("MoveAsideDefraud");
        fleeceFaces[2].SetActive(true);
        yield return new WaitForSeconds(4.5f);
        currentActivePanel++;
        fleeceFaces[2].SetActive(false);
        fleeceFaces[0].SetActive(true);
        defraudIIPanels[currentActivePanel].SetActive(true);
    }
    private IEnumerator FleecesFightPlaces()
    {
        defraudIIPanels[currentActivePanel].SetActive(false);
        deFraudFaces[0].SetActive(false);
        deFraudFaces[4].SetActive(true);
        fleeceFaces[5].SetActive(false);
        fleeceFaces[2].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        defraudAnim.Play("DefraudAfterFleeceAppears");
        fleeceAnim.Play("FleeceFightPosition");
        yield return new WaitForSeconds(2.25f);
        fleeceProtection.SetActive(true);
        flceSlider.SetActive(true);
        playSlider.SetActive(true);
        defSlider.SetActive(true);

        healButton.SetActive(true);
        attackButton.SetActive(true);
        defendButton.SetActive(true);
        shopButton.SetActive(true);
        cashPanel.SetActive(true);
        pauseButton.SetActive(true);

        turnState = TurnState.Player;
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    private IEnumerator FleeceTurn()
    {
        turnState = TurnState.Enemy;

        yield return new WaitForSeconds(0.75f);

        FleeceMoves move = SelectFleeceMove();
        EnemyData data = fleeceMoveDB[move];

        fleeceFaces[2].SetActive(false);
        do
        {
            fleeceFaceIndex = Random.Range(0, 7);
        } while (fleeceFaceIndex == 2 || fleeceFaceIndex == 3);
        fleeceFaces[fleeceFaceIndex].SetActive(true);
        narrator.text = "Fleece deFraud uses " + data.displayName + "!";
        narrationPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationPanel.SetActive(false);
        if (lNNumTurnGap > 0) lNNumTurnGap--;
        if (ldNumTurnGap > 0) ldNumTurnGap--;
        if (catNumTurnGap > 0) catNumTurnGap--;
        if (lnDetails.frozen) lnDetails.numTurnsFrozen++;

        if (move == FleeceMoves.Cat)
        {
            catNumTurnGap = 4;
            fleeceWeaponIcons[0].SetActive(true);
            fleeceWeaponsAnim.Play("UseCat", 0, 0f);
            yield return new WaitForSeconds(1f);
            catHeal.Play("CatHeal", 0, 0f);
            catSound.Play();
            yield return new WaitForSeconds(2.5f);
            fleeceWeaponIcons[0].SetActive(false);
        }
        if (move == FleeceMoves.LiquidNitrogen)
        {
            lNNumTurnGap = 6;
            fleeceWeaponIcons[3].SetActive(true);
            fleeceWeaponsAnim.Play("UseLiquidNitrogen", 0, 0f);
            yield return new WaitForSeconds(1f);
            fleeceWeaponIcons[3].SetActive(false);
            
        }
        if (move == FleeceMoves.LRAD)
        {
            ldNumTurnGap = 2;
            fleeceWeaponIcons[2].SetActive(true);
            fleeceWeaponsAnim.Play("UseLRAD", 0, 0f);
            yield return new WaitForSeconds(1f);
            fleeceWeaponIcons[2].SetActive(false);
            
        }
        if (move == FleeceMoves.Blade)
        {
            effectImage.texture = bladeTex;
            fleeceWeaponIcons[1].SetActive(true);
            fleeceWeaponsAnim.Play("UseBlade", 0, 0f);
            yield return new WaitForSeconds(1f);
            effectImage.color = new Color(1, 1, 1, 0.5f);
            fleeceWeaponIcons[1].SetActive(false);
            bladeSound.Play();
            yield return new WaitForSeconds(0.75f);
            effectImage.color = new Color(0, 0, 0, 0);

        }
        if (move == FleeceMoves.F22)
        {
            effectImage.texture = f22Tex;
            fleeceWeaponIcons[4].SetActive(true);
            fleeceWeaponsAnim.Play("UseF22", 0, 0f);
            yield return new WaitForSeconds(0.2f);
            f22Sound.Play();
            yield return new WaitForSeconds(0.8f);
            effectImage.color = new Color(1, 1, 1, 0.5f);
            fleeceWeaponIcons[4].SetActive(false);
            yield return new WaitForSeconds(0.75f);
            effectImage.color = new Color(0, 0, 0, 0);
        }
        if (lnDetails.numTurnsFrozen == 2)
        {
            frozenPanel.SetActive(false);
            lnDetails.frozen = false;
        }

        

        if (data.heal > 0)
        {
            fleeceHP = Mathf.Min(fleeceHP + data.heal, 4000);
            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("+" + data.heal);
            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(86, 0, 255, 0);
            fleeceHealthTransaction.GetComponent<Animator>().Play("FleeceHealthTransaction", 0, 0f);
            yield return new WaitForSeconds(0.5f);
        }
            

        if (data.attack > 0)
        {
            int finalDamage = data.attack;

            if (defenseActive)
            {
                defenseActive = false;
                if (potionToUse == null)
                {
                    finalDamage = (int)(finalDamage - (finalDamage * 0.33f));
                    playerHP = Mathf.Max(playerHP - finalDamage, 0);
                    playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                    playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                    playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                }
                else
                {
                    int hitProb = Random.Range(0, 10);
                    finalDamage = (int)(finalDamage - (finalDamage * potionToUse.defense));

                    currentDefense = 0;
                    if (hitProb < 5)
                    {
                        if (potionToUse.displayName == "Elephantitan")
                        {
                            fleeceHP = (int)Mathf.Max(fleeceHP - data.attack, 0);
                            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + data.attack);
                            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            fleeceHealthTransaction.GetComponent<Animator>().Play("FleeceHealthTransaction", 0, 0f);
                            fleeceFaces[fleeceFaceIndex].SetActive(false);
                            fleeceFaceIndex = Random.Range(7, 9);
                            fleeceFaces[fleeceFaceIndex].SetActive(true);
                            playerHP = Mathf.Max(playerHP - finalDamage, 0);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                            yield return new WaitForSeconds(0.5f);
                            fleeceFaces[fleeceFaceIndex].SetActive(false);
                            fleeceFaces[2].SetActive(true);
                        }
                        else if (potionToUse.displayName == "Tungstenskin")
                        {
                            fleeceHP = (int)Mathf.Max(fleeceHP - (data.attack * 1.5f), 0);
                            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + (int)(data.attack * 1.5f));
                            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            fleeceHealthTransaction.GetComponent<Animator>().Play("FleeceHealthTransaction", 0, 0f);
                            fleeceFaces[fleeceFaceIndex].SetActive(false);
                            fleeceFaceIndex = Random.Range(7, 9);
                            fleeceFaces[fleeceFaceIndex].SetActive(true);
                            playerHP = Mathf.Max(playerHP - finalDamage, 0);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                            yield return new WaitForSeconds(0.5f);
                            fleeceFaces[fleeceFaceIndex].SetActive(false);
                            fleeceFaces[2].SetActive(true);
                        }
                        else
                        {
                            fleeceHP = (int)Mathf.Max(fleeceHP - (data.attack * 2f), 0);
                            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + (int)(data.attack * 2f));
                            fleeceHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            fleeceHealthTransaction.GetComponent<Animator>().Play("FleeceHealthTransaction", 0, 0f);
                            fleeceFaces[fleeceFaceIndex].SetActive(false);
                            fleeceFaceIndex = Random.Range(7, 9);
                            fleeceFaces[fleeceFaceIndex].SetActive(true);
                            playerHP = Mathf.Max(playerHP - finalDamage, 0);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                            playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                            playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                            yield return new WaitForSeconds(0.5f);
                            fleeceFaces[fleeceFaceIndex].SetActive(false);
                            fleeceFaces[2].SetActive(true);
                        }
                    }
                    else
                    {
                        playerHP = Mathf.Max(playerHP - finalDamage, 0);
                        playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                        playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                        playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
                        yield return new WaitForSeconds(0.5f);
                    }

                    yield return new WaitForSeconds(0.3f);

                    if (fleeceHP < 1)
                    {
                        phaseThree = true;
                        phaseTwo = false;
                    }

                    if (phaseThree && !breakTwoHappened)
                    {
                        StartCoroutine(DeathOfFleece());
                        yield break;
                    }
                }

                yield return new WaitForSeconds(0.3f);

                if (elephantitanEffect.activeInHierarchy) elephantitanEffect.SetActive(false);
                if (tungstenskinEffect.activeInHierarchy) tungstenskinEffect.SetActive(false);
                if (deepsleepreaperEffect.activeInHierarchy)
                {
                    deepsleepreaperEffect.SetActive(false);
                    reaper.Play("ReapersLeaving");
                }

                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                if (data.displayName == "Liquid Nitrogen")
                {
                    lnDetails.frozen = true;
                    lnDetails.numTurnsFrozen = 0;
                    lNNumTurnGap = 6;
                    canF22 = false;
                    frozenPanel.SetActive(true);
                    liquidNitrogenSound.Play();
                }
                else if (data.displayName == "Cat")
                {
                    catNumTurnGap = 4;
                }
                else if (data.displayName == "L.R.A.D")
                {
                    ldDetails.deafened = true;
                    ldDetails.timesDeafened++;
                    stunner.SetActive(true);
                    lradSound.Play();

                }
                else
                {

                }

                playerHP = Mathf.Max(playerHP - finalDamage, 0);
                playerHealthTransaction.GetComponent<TextMeshProUGUI>().text = string.Format("-" + finalDamage);
                playerHealthTransaction.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 3, 0);
                playerHealthTransaction.GetComponent<Animator>().Play("PlayerHealthChange", 0, 0f);
            }
        }

        yield return new WaitForSeconds(0.75f);
        fleeceFaces[fleeceFaceIndex].SetActive(false);
        fleeceFaces[2].SetActive(true);

        yield return new WaitForSeconds(0.4f);
        if (unarmedDefendPanel.activeInHierarchy) unarmedDefendPanel.SetActive(false);
        if (elephantitanEffect.activeInHierarchy) elephantitanEffect.SetActive(false);
        if (tungstenskinEffect.activeInHierarchy) tungstenskinEffect.SetActive(false);
        if (deepsleepreaperEffect.activeInHierarchy)
        {
            deepsleepreaperEffect.SetActive(false);
            reaper.Play("ReapersLeaving");
        }

        yield return new WaitForSeconds(0.5f);

        // Handle the bonus question logic
        if (bonusCooldownTurns > 0)
        {
            bonusCooldownTurns--;

            ReturnControlToPlayer();
            yield break;
        }

        bool offerBonus = false;

        if (playerCash < 276)
        {
            offerBonus = true;
        }
        else
        {
            offerBonus = (Random.Range(0, 10) == 6);
        }

        if (offerBonus)
        {
            bonusOfferPanel.SetActive(true);
        }
        else
        {
            ReturnControlToPlayer();
        }
    }

    private IEnumerator DeathOfFleece()
    {
        flceSlider.SetActive(false);
        defSlider.SetActive(false);
        playSlider.SetActive(false);
        cashPanel.SetActive(false);
        pauseButton.SetActive(false);
        breakTwoHappened = true;
        stunner.SetActive(false);
        ldDetails.deafened = false;
        phaseTwoTheme.Stop();
        fleeceDeath.Play();
        ldDetails.timesDeafened = 0;
        yield return new WaitForSeconds(1f);
        fleeceAnim.Play("FleeceDying");
        yield return new WaitForSeconds(2f);
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(true);
        fleeceFaces[2].SetActive(false);
        fleeceFaces[3].SetActive(true);
    }

    public void NextFleeceDeathPanel()
    {
        if (activeFleeceDeathPanel == 1)
        {
            fleeceFaces[3].SetActive(false);
            fleeceFaces[8].SetActive(true);
        }
        if (activeFleeceDeathPanel == 3)
        {
            deFraudFaces[14].SetActive(false);
            deFraudFaces[0].SetActive(true);
        }
        if (activeFleeceDeathPanel == 6)
        {
            deFraudFaces[6].SetActive(false);
            deFraudFaces[1].SetActive(true);
        }
        if (activeFleeceDeathPanel == 2)
        {
            StartCoroutine(GoodbyeFleece());
        }       
        else if (activeFleeceDeathPanel == 4)
        {
            deFraudFaces[0].SetActive(false);
            deFraudFaces[6].SetActive(true);
            
            StartCoroutine(MachineReady());
            StartCoroutine(FinalMusicChange());
        }
        else if (activeFleeceDeathPanel == defraudIIIPanels.Length - 2)
        {
            deFraudFaces[1].SetActive(false);
            deFraudFaces[5].SetActive(true);
            StartCoroutine(DefraudToCenter());
        }
        else if (activeFleeceDeathPanel == defraudIIIPanels.Length - 1)
        {
            deFraudFaces[5].SetActive(false);
            deFraudFaces[4].SetActive(true);
            StartCoroutine(BeginLastPhase());
        }
        else
        {
            StartCoroutine(FleeceDeathSequence());
        }
    }

    private IEnumerator GoodbyeFleece()
    {
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(false);
        activeFleeceDeathPanel++;
        yield return new WaitForSeconds(0.75f);
        if (fleeceAnim.GetCurrentAnimatorStateInfo(0).IsName("FleeceDying"))
        {
            fleeceAnim.Play("TheDeathOfFleece");
        }
        yield return new WaitForSeconds(5.2f);
        fleeceProtection.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        deFraudFaces[4].SetActive(false);
        deFraudFaces[14].SetActive(true);
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(true);
    }
    private IEnumerator MachineReady()
    {
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(false);
        yield return new WaitForSeconds(1f);
        desOnButton.GetComponent<Light>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        destructionatronBeam.SetActive(true);
        machineChargingSound.Play();
        yield return new WaitForSeconds(3f);
        activeFleeceDeathPanel++;
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(true);
    }

    private IEnumerator FinalMusicChange()
    {
        yield return new WaitForSeconds(1f);
        dangerArises.Play();
        yield return new WaitForSeconds(15f);
        phaseThreeTheme.Play();
    }
    private IEnumerator DefraudToCenter()
    {
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(false);
        activeFleeceDeathPanel++;
        yield return new WaitForSeconds(1f);
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(true);
    }
    private IEnumerator FleeceDeathSequence()
    {
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(false);

        if (activeFleeceDeathPanel == 3)
        {
            if (fleeceDeath.isPlaying) fleeceDeath.Stop();
            machineReadySound.Play();
            yield return new WaitForSeconds(5f);
        }
        yield return new WaitForSeconds(0.5f);
        activeFleeceDeathPanel++;
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(true);
    }
    private IEnumerator BeginLastPhase()
    {
        defraudIIIPanels[activeFleeceDeathPanel].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        defraudAnim.Play("DefraudFinalPos");
        yield return new WaitForSeconds(1f);
        turnState = TurnState.Player;
        ldDetails.deafened = false;
        ldDetails.timesDeafened = 0;

        playSlider.SetActive(true);
        defSlider.SetActive(true);

        healButton.SetActive(true);
        attackButton.SetActive(true);
        defendButton.SetActive(true);
        shopButton.SetActive(true);
        cashPanel.SetActive(true);
        pauseButton.SetActive(true);

    }


    public void DefraudFinalePanelTransition()
    {
        if (activeDefraudFinalePanel == 5)
        {
            StartCoroutine(LiquotholsBetrayal());
        }
        else if (activeDefraudFinalePanel == 10)
        {
            StartCoroutine(LiquotholGrantsShield());
        }
        else if (activeDefraudFinalePanel == defraudFinalePanels.Length - 1)
        {
            StartCoroutine(StartCountdown());
        }
        else
        {
            if (activeDefraudFinalePanel == defraudFinalePanels.Length - 2)
            {
                desCountdown.enabled = true;
            }
            StartCoroutine(NextDefraudFinalePanel());
        }
    }

    private IEnumerator DefraudsLastStand()
    {
        cashPanel.SetActive(false);
        playSlider.SetActive(false);
        defSlider.SetActive(false);
        pauseButton.SetActive(false);
        deFraudFaces[4].SetActive(false);
        deFraudFaces[12].SetActive(true);
        tongueTwisterEffect.SetActive(false);
        ttDetails.tongueTwisted = false;
        ttDetails.timesTwisted = 0;
        drillbacillusEffect.SetActive(false);
        dbDetails.infected = false;
        yield return new WaitForSeconds(1.5f);
        defraudAnim.Play("DefraudMoveToBeamPos");
        yield return new WaitForSeconds(3f);
        defraudFinalePanels[activeDefraudFinalePanel].SetActive(true);
    }
    private IEnumerator NextDefraudFinalePanel()
    {
        if (activeDefraudFinalePanel >= 6 && activeDefraudFinalePanel <= 11)
        {
            defraudFinalePanels[activeDefraudFinalePanel].SetActive(false);
            liquotholFaces[1].SetActive(false);
            liquotholFaces[0].SetActive(true);
            yield return new WaitForSeconds(0.5f);
            activeDefraudFinalePanel++;
            defraudFinalePanels[activeDefraudFinalePanel].SetActive(true);
            liquotholFaces[1].SetActive(true);
            liquotholFaces[0].SetActive(false);
        }
        else
        {
            if (activeDefraudFinalePanel == 1)
            {
                deFraudFaces[12].SetActive(false);
                deFraudFaces[0].SetActive(true);
            }
            if (activeDefraudFinalePanel == 2)
            {
                deFraudFaces[0].SetActive(false);
                deFraudFaces[1].SetActive(true);
            }
            if (activeDefraudFinalePanel == 3)
            {
                deFraudFaces[1].SetActive(false);
                deFraudFaces[6].SetActive(true);
            }
            if (activeDefraudFinalePanel == 10)
            {
                deFraudFaces[4].SetActive(false);
                deFraudFaces[6].SetActive(true);
            }
            defraudFinalePanels[activeDefraudFinalePanel].SetActive(false);
            yield return new WaitForSeconds(0.5f);
            activeDefraudFinalePanel++;
            defraudFinalePanels[activeDefraudFinalePanel].SetActive(true);
        }       
    }
    private IEnumerator LiquotholsBetrayal()
    {
        defraudFinalePanels[activeDefraudFinalePanel].SetActive(false);
        yield return new WaitForSeconds(0.75f);
        liquotholAnim.Play("LiquotholAppearance");
        yield return new WaitForSeconds(1f);
        activeDefraudFinalePanel++;
        liquotholFaces[1].SetActive(true);
        liquotholFaces[0].SetActive(false);
        defraudFinalePanels[activeDefraudFinalePanel].SetActive(true);
    }
    private IEnumerator LiquotholGrantsShield()
    {
        defraudFinalePanels[activeDefraudFinalePanel].SetActive(false);
        liquotholFaces[1].SetActive(false);
        liquotholFaces[0].SetActive(true);
        yield return new WaitForSeconds(0.75f);
        liquotholAnim.Play("LiquotholDepart");
        yield return new WaitForSeconds(1.5f);
        deFraudFaces[6].SetActive(false);
        deFraudFaces[4].SetActive(true);
        liquotholShieldSound.Play();
        liquotholShield.SetActive(true);
        activeDefraudFinalePanel++;
        defraudFinalePanels[activeDefraudFinalePanel].SetActive(true);
    }
    private IEnumerator StartCountdown()
    {
        defraudFinalePanels[activeDefraudFinalePanel].SetActive(false);
        yield return new WaitForSeconds(0.25f);
        shieldChallengeActive = true;
        pressSpacePanel.SetActive(true);
        shieldTimer = 30f;
        numSpaces = 0;

        desCountdown.enabled = true;

        while (shieldTimer > 0)
        {
            shieldTimer -= Time.deltaTime;
            desCountdown.text = Mathf.Ceil(shieldTimer).ToString();

            yield return null;
        }

        shieldChallengeActive = false;
        pressSpacePanel.SetActive(false);
        desCountdown.enabled = false;
        CheckShieldResult();
    }
    void CheckShieldResult()
    {
        narrator.text = string.Format("SPACES PRESSED: " + numSpaces);
        if (numSpaces >= 150 && numSpaces <= 200)
        {
            StartCoroutine(ShieldSuccess());
        }
        else
        {
            StartCoroutine(ShieldFailure());
        }
    }

    private IEnumerator ShieldSuccess()
    {
        narrationPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        narrationPanel.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        desEffect.SetActive(true);
        if (machineChargingSound.isPlaying) machineChargingSound.Stop();
        machineBlastSound.Play();
        yield return new WaitForSeconds(2f);
        desEffect.SetActive(false);
        liquotholShield.SetActive(false);
        destructionatronBeam.SetActive(false);
        destructionatron.SetActive(false);
        deFraudHP = 0;
        if (phaseThreeTheme.isPlaying) phaseThreeTheme.Stop();
        yield return new WaitForSeconds(0.5f);
        defraudDeath.Play();
        defraudAnim.Play("DefraudConcludingLoc");
        yield return new WaitForSeconds(4f);
        deFraudFaces[4].SetActive(false);
        deFraudFaces[6].SetActive(false);
        deFraudFaces[9].SetActive(true);
        defraudAnim.Play("DefraudDying");
        yield return new WaitForSeconds(3f);
        defraudDeathPanels[activeDefraudDeathPanel].SetActive(true);
    }
    private IEnumerator ShieldFailure()
    {
        narrationPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        narrationPanel.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        liquotholShield.SetActive(false);
        desEffect.SetActive(true);
        if (machineChargingSound.isPlaying) machineChargingSound.Stop();
        machineBlastSound.Play();
        yield return new WaitForSeconds(2f);
        transition.Play("BossFadeTransition");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("BossLoseScene");
    }

    public void NextDefraudDeathPanel()
    {
        if (activeDefraudDeathPanel < defraudDeathPanels.Length - 1)
        {
            StartCoroutine(DefraudDeathSequence());
        }
        else
        {
            StartCoroutine(HappyEnding());
        }
    }

    private IEnumerator DefraudDeathSequence()
    {
        defraudDeathPanels[activeDefraudDeathPanel].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        activeDefraudDeathPanel++;
        if (activeDefraudDeathPanel == 1)
        {
            deFraudFaces[9].SetActive(false);
            deFraudFaces[10].SetActive(true);
        }
        if (activeDefraudDeathPanel == 2)
        {
            deFraudFaces[10].SetActive(false);
            deFraudFaces[13].SetActive(true);
        }
        if (activeDefraudDeathPanel == 3)
        {
            deFraudFaces[13].SetActive(false);
            deFraudFaces[14].SetActive(true);
        }
        defraudDeathPanels[activeDefraudDeathPanel].SetActive(true);
    }
    private IEnumerator HappyEnding()
    {
        defraudDeathPanels[activeDefraudDeathPanel].SetActive(false);
        yield return new WaitForSeconds(2f);
        defraudAnim.Play("DrDefraudDeath");
        yield return new WaitForSeconds(6f);
        defraudDeath.Stop();
        congrats.Play("CongratsBoss");
        yield return new WaitForSeconds(6.5f);
        transition.Play("BossFadeTransition");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("BossWinScene");
    }

    private IEnumerator GameOver()
    {
        
        if (phaseOneTheme.isPlaying || phaseTwoTheme.isPlaying || phaseThreeTheme.isPlaying)
        {
            phaseOneTheme.Stop();
            phaseTwoTheme.Stop();
            phaseThreeTheme.Stop();
        }
        cashPanel.SetActive(false);
        playSlider.SetActive(false);
        defSlider.SetActive(false);
        flceSlider.SetActive(false);
        pauseButton.SetActive(false);
        if (!gameOverPlayed)
        {
            playerGameOver.Play();
            gameOverPlayed = true;
        } 
        yield return new WaitForSeconds(2f);
        transition.Play("BossFadeTransition");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("BossLoseScene");
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseTheme.Play();
        if (phaseOneTheme.isPlaying) phaseOneTheme.Pause();
        if (phaseTwoTheme.isPlaying) phaseTwoTheme.Pause();
        if (phaseThreeTheme.isPlaying) phaseThreeTheme.Pause();
    }

    IEnumerator SpawnRain()
    {
        float timer = 0f;

        while (timer < spawnDuration)
        {
            SpawnHand();
            yield return new WaitForSeconds(spawnRate);
            timer += spawnRate;
        }
    }

    void SpawnHand()
    {
        GameObject hand = Instantiate(handPrefab, canvasRect);

        RectTransform rect = hand.GetComponent<RectTransform>();

        // random x position across canvas width
        float randomX = Random.Range(0, canvasRect.rect.width);

        // place at top of canvas
        rect.anchoredPosition = new Vector2(
            randomX - canvasRect.rect.width / 2,
            canvasRect.rect.height / 2 + 50
        );

        // add falling behavior
        HandFall fall = hand.AddComponent<HandFall>();
        fall.speed = fallSpeed;
    }
}
