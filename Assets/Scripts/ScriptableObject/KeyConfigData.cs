using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyConfigData", menuName = "KeyConfig/KeyConfigData", order = 1)]
public class KeyConfigData : ScriptableObject
{
    public KeyCode UP;
    public KeyCode DOWN;
    public KeyCode LEFT;
    public KeyCode RIGHT;
    public KeyCode CameraUP;
    public KeyCode CameraDOWN;
    public KeyCode CameraLEFT;
    public KeyCode CameraRIGHT;
    public KeyCode CameraZoomIn;
    public KeyCode CameraZoomOut;
    public KeyCode CameraAimRotate;
    public KeyCode CameraReset;
    public KeyCode Attack1;
    public KeyCode Attack2;
    public KeyCode UnlockAttackDirection;
    public KeyCode Jump;
    public KeyCode Run;
    public KeyCode Pause;
}
