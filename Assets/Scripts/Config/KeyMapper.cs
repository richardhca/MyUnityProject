using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GameCore.KeyConfig
{
    public class KeyMapper : MonoBehaviour
    {
        private readonly Dictionary<string, KeyCode> keyDefault = new Dictionary<string, KeyCode>
        {
            { "UP", KeyCode.Keypad5 }, { "DOWN", KeyCode.Keypad2 }, { "LEFT", KeyCode.Keypad1 }, { "RIGHT", KeyCode.Keypad3 },
            { "CameraUP", KeyCode.Keypad7 }, { "CameraDOWN", KeyCode.Keypad9 }, { "CameraLEFT", KeyCode.Keypad4 }, { "CameraRIGHT", KeyCode.Keypad6 },
            { "CameraZoomIn", KeyCode.Keypad8 }, { "CameraZoomOut", KeyCode.Keypad0 }, { "CameraAimRotate", KeyCode.A }, { "CameraReset", KeyCode.Z },
            { "Attack1", KeyCode.F }, { "Attack2", KeyCode.G }, { "UnlockAttackDirection", KeyCode.Space },
            { "Jump", KeyCode.D }, { "Run", KeyCode.S }, { "Pause", KeyCode.Escape }
        };

        [SerializeField] KeyConfigData keyConfigData;
        [SerializeField] GameObject scrollableContent;
        [SerializeField] Transform keyTable;
        [SerializeField] GameObject enterKey;
        [SerializeField] GameObject mainMenu;

        private KeyCode UP = KeyCode.UpArrow;
        private KeyCode DOWN = KeyCode.DownArrow;
        private KeyCode SPACE = KeyCode.Space;
        private KeyCode F1 = KeyCode.F1;
        private KeyCode ESC = KeyCode.Escape;

        private Dictionary<string, KeyCode> keyConfig;

        private int currentSelectedIndex;
        private int newSelectedIndex;

        private int currentTopDisplayIndex;
        private int currentBottomDisplayIndex;

        void Awake()
        {
            keyConfig = new Dictionary<string, KeyCode>();
            var material = transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontMaterial;
            for (int i = 0; i < keyDefault.Count; i++)
            {
                keyConfig.Add(keyDefault.Keys.ElementAt(i), keyDefault.Values.ElementAt(i));

                GameObject keyCat = new GameObject(keyDefault.Keys.ElementAt(i));
                keyCat.transform.SetParent(keyTable);
                var keyCatTransform = keyCat.AddComponent<RectTransform>();
                keyCatTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 50);
                var horizontalLayoutGroup = keyCat.AddComponent<HorizontalLayoutGroup>();
                horizontalLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                var focusHighlight = keyCat.AddComponent<Image>();
                focusHighlight.color = new Color(255, 255, 255, 0);
                GameObject keyName = new GameObject("Key");
                keyName.AddComponent<RectTransform>();
                keyName.transform.SetParent(keyCat.transform);
                GameObject keyCode = new GameObject("KeyCode");
                keyCode.AddComponent<RectTransform>();
                keyCode.transform.SetParent(keyCat.transform);

                GameObject nameText = new GameObject("Text");
                nameText.transform.SetParent(keyName.transform);
                var nameRect = nameText.AddComponent<RectTransform>();
                nameRect.anchorMin = Vector2.zero;
                nameRect.anchorMax = Vector2.one;
                var nametxt = nameText.AddComponent<TextMeshProUGUI>();
                nametxt.text = keyDefault.Keys.ElementAt(i);
                nametxt.alignment = TextAlignmentOptions.Center;
                nametxt.fontMaterial = material;

                GameObject codeText = new GameObject("Text");
                codeText.transform.SetParent(keyCode.transform);
                var codeRect = codeText.AddComponent<RectTransform>();
                codeRect.anchorMin = Vector2.zero;
                codeRect.anchorMax = Vector2.one;
                var codetxt = codeText.AddComponent<TextMeshProUGUI>();
                codetxt.text = keyDefault.Values.ElementAt(i).ToString();
                codetxt.alignment = TextAlignmentOptions.Center;
                codetxt.fontMaterial = material;
            }
            mapToKeyConfigData();
        }

        void Start()
        {
            enterKey.SetActive(false);
            mainMenu.SetActive(false);
            currentSelectedIndex = 0;
            newSelectedIndex = 0;
            selectNewIndex(true);
            scrollableContent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;
            currentTopDisplayIndex = 0;
            currentBottomDisplayIndex = 8;
        }

        void Update()
        {
            if (enterKey.activeSelf)
            {
                if (Input.anyKeyDown)
                {
                    foreach (KeyCode downKey in Enum.GetValues(typeof(KeyCode)))
                    {
                        if (Input.GetKeyDown(downKey))
                        {
                            SetKey(keyTable.GetChild(currentSelectedIndex).name, downKey);
                            enterKey.SetActive(false);
                        }
                    }
                }
                return;
            }

            if (Input.GetKeyDown(UP))
            {
                newSelectedIndex = Math.Max(currentSelectedIndex - 1, 0);
                if (newSelectedIndex < currentTopDisplayIndex)
                {
                    scrollableContent.GetComponent<ScrollRect>().verticalNormalizedPosition = Mathf.Min(scrollableContent.GetComponent<ScrollRect>().verticalNormalizedPosition + keyTable.GetChild(newSelectedIndex).GetComponent<RectTransform>().sizeDelta.y / keyTable.GetComponent<RectTransform>().sizeDelta.y, 1.0f);
                    currentTopDisplayIndex--;
                    currentBottomDisplayIndex--;
                }
                selectNewIndex(false);
            }

            if (Input.GetKeyDown(DOWN))
            {
                newSelectedIndex = Math.Min(currentSelectedIndex + 1, keyTable.childCount - 1);
                if (newSelectedIndex > currentBottomDisplayIndex)
                {
                    scrollableContent.GetComponent<ScrollRect>().verticalNormalizedPosition = Mathf.Max(scrollableContent.GetComponent<ScrollRect>().verticalNormalizedPosition - keyTable.GetChild(newSelectedIndex).GetComponent<RectTransform>().sizeDelta.y / keyTable.GetComponent<RectTransform>().sizeDelta.y, 0.0f);
                    currentTopDisplayIndex++;
                    currentBottomDisplayIndex++;
                }
                selectNewIndex(false);
            }

            if (Input.GetKeyDown(SPACE))
            {
                enterKey.SetActive(true);
            }

            if (Input.GetKeyDown(F1))
            {
                SetDefault();
            }

            if (Input.GetKeyDown(ESC))
            {
                mapToKeyConfigData();
                mainMenu.SetActive(true);
                scrollableContent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1.0f;
                gameObject.SetActive(false);
            }
        }

        private void selectNewIndex(bool initialize)
        {
            if (currentSelectedIndex != newSelectedIndex || initialize)
            {
                if (!initialize)
                {
                    var prev = keyTable.GetChild(currentSelectedIndex).GetComponent<Image>();
                    prev.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                }
                var curr = keyTable.GetChild(newSelectedIndex).GetComponent<Image>();
                curr.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                currentSelectedIndex = newSelectedIndex;
            }
        }

        public void SetKey(string key, KeyCode value)
        {
            for (int i = 0; i < keyConfig.Count; i++)
            {
                if (keyConfig.Values.ElementAt(i) == value)
                {
                    keyConfig[keyConfig.Keys.ElementAt(i)] = keyConfig[key];
                    var item = keyTable.Find(keyConfig.Keys.ElementAt(i));
                    if (item != null)
                        item.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = keyConfig[key].ToString();
                    break;
                }
            }
            keyConfig[key] = value;
            keyTable.GetChild(currentSelectedIndex).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
        }

        public void SetDefault()
        {
            for (int i = 0; i < keyDefault.Count; i++)
            {
                keyConfig[keyDefault.Keys.ElementAt(i)] = keyDefault.Values.ElementAt(i);
                keyTable.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = keyDefault.Values.ElementAt(i).ToString();
            }
        }

        private void mapToKeyConfigData()
        {
            keyConfigData.UP = keyConfig["UP"];
            keyConfigData.DOWN = keyConfig["DOWN"];
            keyConfigData.LEFT = keyConfig["LEFT"];
            keyConfigData.RIGHT = keyConfig["RIGHT"];
            keyConfigData.CameraUP = keyConfig["CameraUP"];
            keyConfigData.CameraDOWN = keyConfig["CameraDOWN"];
            keyConfigData.CameraLEFT = keyConfig["CameraLEFT"];
            keyConfigData.CameraRIGHT = keyConfig["CameraRIGHT"];
            keyConfigData.CameraZoomIn = keyConfig["CameraZoomIn"];
            keyConfigData.CameraZoomOut = keyConfig["CameraZoomOut"];
            keyConfigData.CameraAimRotate = keyConfig["CameraAimRotate"];
            keyConfigData.CameraReset = keyConfig["CameraReset"];
            keyConfigData.Attack1 = keyConfig["Attack1"];
            keyConfigData.Attack2 = keyConfig["Attack2"];
            keyConfigData.UnlockAttackDirection = keyConfig["UnlockAttackDirection"];
            keyConfigData.Jump = keyConfig["Jump"];
            keyConfigData.Run = keyConfig["Run"];
            keyConfigData.Pause = keyConfig["Pause"];
        }
    }
}