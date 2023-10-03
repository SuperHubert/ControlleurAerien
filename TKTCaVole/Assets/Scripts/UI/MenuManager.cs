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
    [SerializeField] private Button returnToMenuButton;

    private static bool gameLaunched;
    
    public static bool skipMenu;
    public static int lastPlayedLevel;
    
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
        
        if(skipMenu) ShowLevels();
    }

    private void TryLaunchGame()
    {
        if(gameLaunched) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            gameLaunched = true;
        
            BootMenu();
        }
        
        if(Gamepad.current == null) return;

        if (Gamepad.current.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic))
        {
            gameLaunched = true;
        
            BootMenu();
        }
    }    
    
    private void ShowLevels()
    {
        menuTr.DOLocalMoveX(-1920, 0.75f);
        levelsTr.DOLocalMoveX(0, 0.75f);
    }

    private void ShowMenu()
    {
        menuTr.DOLocalMoveX(0, 0.75f);
        levelsTr.DOLocalMoveX(1920, 0.75f);
    }
}