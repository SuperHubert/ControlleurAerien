using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [FormerlySerializedAs("rotateSpeed")] [SerializeField] private Vector3 cameraRotateSpeed;

    [Header("Camera Angles")]
    
    [SerializeField] private Vector3 levelsCamPos;
    [SerializeField] private Quaternion levelCamRot;
    [SerializeField] private Vector3 creditsCamPos;
    [SerializeField] private Quaternion creditsCamRot;
    private Vector3 camPosCache;
    private Quaternion camRotCache;
    private bool rotateCam;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject pressKeyGo;
    [SerializeField] private GameObject restOfMenu;

    [Header("Components")]
    [SerializeField] private Graphic titleGraphic;
    [SerializeField] private TextMeshProUGUI pressAnyKeyText;
    
    [Header("Transforms")]
    [SerializeField] private Transform menuCam;
    [SerializeField] private Transform menuTr;
    [SerializeField] private Transform levelsTr;
    
    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button returnToMenuButton;
    private List<(RectTransform tr,Vector2 sizeDelta)> buttonTransforms = new ();
    [SerializeField] private float buttonRevealDuration = 0.25f;

    [Header("Other")]
    [SerializeField] private UILevelManager uiLevelManager;
    [SerializeField] private SettingsManager settingsManager;
    [SerializeField] private UISettingsSo uiSettings;

    private Sequence glowSequence;
    
    private static bool gameLaunched;
    
    public static bool skipMenu;

    private void Awake()
    {
        uiSettings.SetInstance();
    }

    private void Start()
    {
        BootMenu();
    }

    private void Update()
    {
        menuCam.Rotate(cameraRotateSpeed * (rotateCam ? 1 : -1));

        TryLaunchGame();
    }

    private void BootMenu()
    {
        pressKeyGo.SetActive(!gameLaunched);
        restOfMenu.SetActive(gameLaunched);

        if (!gameLaunched)
        {
            StartPressAnyKeyGlow();
            return;
        }

        glowSequence?.Kill();

        playButton.onClick.AddListener(ShowLevels);
        returnToMenuButton.onClick.AddListener(ShowMenu);
        exitButton.onClick.AddListener(Application.Quit);
        settingsButton.onClick.AddListener(OpenSettings);

        rotateCam = true;
        
        if (skipMenu)
        {
            ShowLevels();
            return;
        }
        
        RevealMenuButtonAnimation();
    }

    private void StartPressAnyKeyGlow()
    {
        glowSequence = DOTween.Sequence();

        glowSequence.AppendInterval(2f);
        glowSequence.Append(pressAnyKeyText.DOFade(0, 1));
        glowSequence.AppendInterval(1f);
        glowSequence.Append(pressAnyKeyText.DOFade(1, 1));
        glowSequence.SetLoops(-1);

        glowSequence.Play();
    }
    
    private void RevealMenuButtonAnimation()
    {
        buttonTransforms.Clear();

        var tr = playButton.GetComponent<RectTransform>();
        buttonTransforms.Add((tr,tr.sizeDelta));
        tr = settingsButton.GetComponent<RectTransform>();
        buttonTransforms.Add((tr,tr.sizeDelta));
        tr = creditsButton.GetComponent<RectTransform>();
        buttonTransforms.Add((tr,tr.sizeDelta));
        tr = exitButton.GetComponent<RectTransform>();
        buttonTransforms.Add((tr,tr.sizeDelta));

        foreach (var couple in buttonTransforms)
        {
            var size = couple.sizeDelta;
            size.x = 0;
            couple.tr.sizeDelta = size;
        }

        var clearWhite = Color.white;
        clearWhite.a = 0;
        titleGraphic.color = clearWhite;
        
        var sequence = DOTween.Sequence();
        sequence.Append(titleGraphic.DOFade(1, 1f));
        sequence.AppendCallback(playButton.Select);
        sequence.Append(buttonTransforms[0].tr.DOSizeDelta(buttonTransforms[0].sizeDelta, buttonRevealDuration));
        sequence.Append(buttonTransforms[1].tr.DOSizeDelta(buttonTransforms[1].sizeDelta, buttonRevealDuration));
        sequence.Append(buttonTransforms[2].tr.DOSizeDelta(buttonTransforms[2].sizeDelta, buttonRevealDuration));
        sequence.Append(buttonTransforms[3].tr.DOSizeDelta(buttonTransforms[3].sizeDelta, buttonRevealDuration));
        
        sequence.Play();
    }

    private void TryLaunchGame()
    {
        if(gameLaunched) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            gameLaunched = true;
        
            BootMenu();
        }
        
        if(Gamepad.current == null) return;

        if (Gamepad.current.allControls.Any(x => x is ButtonControl && x.IsPressed() && !x.synthetic))
        {
            gameLaunched = true;
        
            BootMenu();
        }
    }    
    
    private void ShowLevels()
    {
        rotateCam = false;

        camPosCache = menuCam.position;
        camRotCache = menuCam.rotation;
        
        menuCam.DOMove(levelsCamPos, 0.75f);
        menuCam.DORotate(levelCamRot.eulerAngles, 0.75f);
        
        menuTr.DOLocalMoveX(-1920, 0.75f).SetUpdate(true);;
        levelsTr.DOLocalMoveX(0, 0.75f).SetUpdate(true);;
        
        uiLevelManager.UpdateLineRenderer();
        uiLevelManager.ScrollToLastLevel();
    }

    private void ShowMenu()
    {
        rotateCam = true;

        menuCam.DOMove(camPosCache, 0.75f);
        menuCam.DORotate(camRotCache.eulerAngles, 0.75f);
        
        menuTr.DOLocalMoveX(0, 0.75f).SetUpdate(true);;
        levelsTr.DOLocalMoveX(1920, 0.75f).SetUpdate(true);
        
        playButton.Select();
    }

    private void OpenSettings()
    {
        settingsManager.Open(settingsButton);
    }
}