using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [FormerlySerializedAs("rotateSpeed")] [SerializeField] private Vector3 cameraRotateSpeed;

    [Header("GameObjects")]
    [SerializeField] private GameObject pressKeyGo;
    [SerializeField] private GameObject restOfMenu;
    
    [Header("Transforms")]
    [SerializeField] private Transform menuCam;
    [SerializeField] private Transform menuTr;
    [SerializeField] private Transform levelsTr;
    [SerializeField] private Transform levelsParent;
    
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

    private static bool gameLaunched;
    
    public static bool skipMenu;
    public static int lastPlayedLevel;

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
        menuCam.Rotate(cameraRotateSpeed);

        TryLaunchGame();
    }

    private void BootMenu()
    {
        pressKeyGo.SetActive(!gameLaunched);
        restOfMenu.SetActive(gameLaunched);
        
        if(!gameLaunched) return;
        
        playButton.onClick.AddListener(ShowLevels);
        returnToMenuButton.onClick.AddListener(ShowMenu);
        exitButton.onClick.AddListener(Application.Quit);
        settingsButton.onClick.AddListener(OpenSettings);
        
        if (skipMenu)
        {
            ShowLevels();
            return;
        }
        
        RevealMenuButtonAnimation();
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
        
        var sequence = DOTween.Sequence();
        sequence.Append(buttonTransforms[0].tr.DOSizeDelta(buttonTransforms[0].sizeDelta, buttonRevealDuration));
        sequence.Append(buttonTransforms[1].tr.DOSizeDelta(buttonTransforms[1].sizeDelta, buttonRevealDuration));
        sequence.Append(buttonTransforms[2].tr.DOSizeDelta(buttonTransforms[2].sizeDelta, buttonRevealDuration));
        sequence.Append(buttonTransforms[3].tr.DOSizeDelta(buttonTransforms[3].sizeDelta, buttonRevealDuration));
        sequence.AppendCallback(playButton.Select);
        
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
        menuTr.DOLocalMoveX(-1920, 0.75f).SetUpdate(true);;
        levelsTr.DOLocalMoveX(0, 0.75f).SetUpdate(true);;
        
        uiLevelManager.ScrollToLastLevel();
    }

    private void ShowMenu()
    {
        menuTr.DOLocalMoveX(0, 0.75f).SetUpdate(true);;
        levelsTr.DOLocalMoveX(1920, 0.75f).SetUpdate(true);
        
        playButton.Select();
    }

    private void OpenSettings()
    {
        settingsManager.Open(settingsButton);
    }
}