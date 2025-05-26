using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ArchipelagoEverhood.Util;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoLogin
    {
        private readonly string _lastLoginPath;
        private readonly Texture2D _yesTexture;
        private readonly Texture2D _yesTextureHighlighted;
        private readonly Texture2D _noTexture;
        private readonly Texture2D _noTextureHighlighted;
        private readonly Texture2D _backgroundTexture;
        private readonly string _versionNumber;
        private readonly string _trueVersion;

        private GUIStyle? _windowStyle;
        private GUIStyle? _buttonYesStyle;
        private GUIStyle? _buttonNoStyle;
        private GUIStyle? _labelStyle;
        private GUIStyle? _labelCenterStyle;
        private GUIStyle? _textFieldStyle;
        private GUIStyle? _toggleStyle;
        private readonly GUILayoutOption[] _defaultInputHeight = { GUILayout.Height(25f) };

        private bool _showLoginScreen;
#if DEBUG
        private string _ipAddress = "localhost:38281";
#else
        private string _ipAddress = "archipelago.gg:38281";
#endif
        private string _username = "";
        private string _password = "";
        private bool _showPassword;
        private string? _error;

        private VersionCheckState _checkVersionState;
        private string? _newVersionValue;
        private bool _waiting;

        private Transform? _mainMenuObject;

        public ArchipelagoLogin(string versionNumber, string trueVersion)
        {
            _lastLoginPath = Path.Combine(Application.absoluteURL, "UserData/ArchSaves/LastLogin.txt");
            _backgroundTexture = AssetHelpers.LoadTexture("ArchipelagoEverhood.Assets.LoginBackground.png") ?? throw new Exception("Failed to load Login Background.");
            _yesTexture = AssetHelpers.LoadTexture("ArchipelagoEverhood.Assets.ButtonYes.png") ?? throw new Exception("Failed to load Button Yes.");
            _yesTextureHighlighted = AssetHelpers.LoadTexture("ArchipelagoEverhood.Assets.ButtonYesHighlighted.png") ?? throw new Exception("Failed to load Button Yes Highlighted.");
            _noTexture = AssetHelpers.LoadTexture("ArchipelagoEverhood.Assets.ButtonNo.png") ?? throw new Exception("Failed to load  Button No.");
            _noTextureHighlighted = AssetHelpers.LoadTexture("ArchipelagoEverhood.Assets.ButtonNoHighlighted.png") ?? throw new Exception("Failed to load  Button No Highlighted.");
            _versionNumber = versionNumber;
            _trueVersion = trueVersion;
            MelonEvents.OnGUI.Subscribe(DrawArchipelagoScreen);
        }

        private void OnArchipelagoClick()
        {
            if (_mainMenuObject == null)
                return;

            _mainMenuObject.gameObject.SetActive(false);
            Cursor.visible = true;

            _showLoginScreen = true;
            if (File.Exists(_lastLoginPath))
            {
                using (var file = File.OpenRead(_lastLoginPath))
                {
                    using (var sr = new StreamReader(file))
                    {
                        _ipAddress = sr.ReadLine() ?? "";
                        _username = sr.ReadLine() ?? "";
                    }
                }
            }
        }

        public void HideArchipelago()
        {
            if (_mainMenuObject == null)
                throw new Exception("Woah, menu object should not be null here.");

            _waiting = false;
            _showLoginScreen = false;
            Cursor.visible = false;
            _mainMenuObject.gameObject.SetActive(true);
            EditUndertitle();
        }

        private void AttemptLogin()
        {
            _waiting = true;
            AttemptLoginAsync().ConfigureAwait(false);
        }
        
        private async Task AttemptLoginAsync()
        {
            try
            {
                var ipAddress = _ipAddress.Trim();
                if (ipAddress.StartsWith("/connect"))
                    ipAddress = ipAddress.Remove(0, 8).Trim();

                var reason = await Globals.SessionHandler.TryFreshLogin(ipAddress, _username.Trim(), _password.Trim());
                if (reason != null)
                {
                    _error = reason;
                    _waiting = false;
                    return;
                }

                var baseDirectory = Path.GetDirectoryName(_lastLoginPath);
                if (baseDirectory != null)
                {
                    if (!Directory.Exists(baseDirectory))
                        Directory.CreateDirectory(baseDirectory);

                    var sb = new StringBuilder();
                    sb.AppendLine(_ipAddress);
                    sb.AppendLine(_username);
                    await File.WriteAllTextAsync(_lastLoginPath, sb.ToString());
                }

                Globals.SessionHandler.StartSession();
                HideArchipelago();
            }
            catch (Exception e)
            {
                StopWaiting(e);
            }
        }

        private void Disconnect()
        {
            _waiting = true;
            Globals.SessionHandler.Disconnect().ConfigureAwait(false);
        }

        public void StopWaiting(Exception? e)
        {
            if (e != null)
                Globals.Logging.Error("Login", e);
            
            _error = e?.Message;
            _waiting = false;
        }

#region Draw Login GUI

        private void DrawArchipelagoScreen()
        {
            try
            {
                //Can only call this in GUI
                if (_windowStyle == null)
                    SetupStyles(); //Todo: Add textures

                //DrawForfeitRelease();

                if (!_showLoginScreen)
                {
                    //if (!_backedUpFile)
                    //    BackupSaveData();
                    return;
                }
                
                
                if (Globals.SessionHandler.LoggedIn)
                    GUI.ModalWindow(0, new Rect(Screen.width / 2.0f - 175, Screen.height / 2.0f - 90, 350, 180), (GUI.WindowFunction)DrawDisconnectWindow, "Disconnect from Archipelago", _windowStyle);
                else
                    GUI.ModalWindow(0, new Rect(Screen.width / 2.0f - 250, Screen.height / 2.0f - 215, 500, 430), (GUI.WindowFunction)DrawMainWindow, "Connect to an Archipelago Server", _windowStyle);
                
                GUI.enabled = true;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("DrawArchLogin", e);
                MelonEvents.OnGUI.Unsubscribe(DrawArchipelagoScreen);
            }
        }

        private void DrawMainWindow(int windowID)
        {
            if (_waiting)
                GUI.enabled = false;
            
            GUILayout.Label("", _labelStyle, GUILayout.Height(40f));

            GUILayout.Label("IP Address And Port:", _labelStyle);
            _ipAddress = GUILayout.TextField(_ipAddress, _textFieldStyle, _defaultInputHeight);

            GUILayout.Label("Username:", _labelStyle);
            _username = GUILayout.TextField(_username, _textFieldStyle, _defaultInputHeight);

            GUILayout.Label("Password:", _labelStyle);
            if (_showPassword)
                _password = GUILayout.TextField(_password, _textFieldStyle, _defaultInputHeight);
            else
                _password = GUILayout.PasswordField(_password, '*', _textFieldStyle, _defaultInputHeight);
            _showPassword = GUILayout.Toggle(_showPassword, "Show Password", _toggleStyle);


            GUILayout.Label("Version: " + _versionNumber, _labelStyle);

            GUILayout.Label(_error ?? "", _labelCenterStyle, GUILayout.Height(40f));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Back Out", _buttonNoStyle, GUILayout.Height(40f)))
                HideArchipelago();

            if (GUILayout.Button("Log In", _buttonYesStyle, GUILayout.Height(40f)))
                AttemptLogin();

            GUILayout.EndHorizontal();
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            switch (_checkVersionState)
            {
                case VersionCheckState.NotChecked:
                {
                    if (GUILayout.Button("Check for new version", GUILayout.Height(30f), GUILayout.Width(300f)))
                    {
                        _checkVersionState = VersionCheckState.Checking;
                        CheckForNewVersion().ConfigureAwait(false);
                    }

                    break;
                }
                case VersionCheckState.Checking:
                    GUILayout.Label("Checking for update. Please wait...", _labelCenterStyle);
                    break;
                case VersionCheckState.NewVersion:
                    GUILayout.Label($"New Version Available: {_newVersionValue}", _labelCenterStyle);
                    break;
                case VersionCheckState.UpToDate:
                    GUILayout.Label("Up to date!", _labelCenterStyle);
                    break;
                case VersionCheckState.Errored:
                    GUILayout.Label($"An error has occured: {_newVersionValue}", _labelCenterStyle);
                    break;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
        private void DrawDisconnectWindow(int windowID)
        {
            if (_waiting)
                GUI.enabled = false;
            
            GUILayout.Label("", _labelStyle, GUILayout.Height(10f));
            
            GUILayout.Label("Are you sure you want to disconnect from Archipelago?", _labelCenterStyle, GUILayout.Height(40f));
            GUILayout.Label(!string.IsNullOrEmpty(_error) ? _error : "", _labelCenterStyle, GUILayout.Height(40f));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Back Out", _buttonNoStyle, GUILayout.Height(40f)))
                HideArchipelago();

            if (GUILayout.Button("Disconnect", _buttonYesStyle, GUILayout.Height(40f)))
                Disconnect();

            GUILayout.EndHorizontal();
        }

        private void SetupStyles()
        {
            var mainState = new GUIStyleState
            {
                background = _backgroundTexture,
                textColor = new Color(1, 1, 1, 1)
            };

            _windowStyle = new GUIStyle(GUI.skin.window)
            {
                fontSize = 24,
                normal = mainState,
                hover = mainState,
                active = mainState
            };

            var yesState = new GUIStyleState
            {
                background = _yesTexture,
                textColor = Color.white
            };
            var yesStateHighlighted = new GUIStyleState
            {
                background = _yesTextureHighlighted,
                textColor = Color.white
            };

            _buttonYesStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 20,
                normal = yesState,
                hover = yesStateHighlighted,
                active = yesStateHighlighted
            };

            var noState = new GUIStyleState
            {
                background = _noTexture,
                textColor = Color.white
            };
            var noStateHighlighted = new GUIStyleState
            {
                background = _noTextureHighlighted,
                textColor = Color.white
            };

            _buttonNoStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 20,
                normal = noState,
                hover = noStateHighlighted,
                active = noStateHighlighted
            };

            _labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16
            };
            _labelCenterStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                alignment = TextAnchor.UpperCenter
            };
            _textFieldStyle = new GUIStyle(GUI.skin.textField)
            {
                fontSize = 16
            };
            _toggleStyle = new GUIStyle(GUI.skin.toggle)
            {
                fontSize = 16
            };
        }

#endregion

#region Menu Edits

        public void MainMenuLoaded(int buildIndex)
        {
            Globals.Logging.Msg($"Main Menu Build Index: {buildIndex}");
            var scene = SceneManager.GetSceneByBuildIndex(buildIndex);
            if (scene.buildIndex != buildIndex)
                throw new Exception("Failed to edit Main Menu: Main Menu wasn't what we expected?");

            if (!EverhoodHelpers.TryGetGameObjectWithName("Root PC", scene.GetRootGameObjects(), out var baseObj))
                throw new Exception("Failed to edit Main Menu: Could not find 'Root PC'.");

            if (!EverhoodHelpers.TryGetChildWithName("Canvas", baseObj, out var canvas))
                throw new Exception("Failed to edit Main Menu: Could not find 'Canvas'.");

            if (!EverhoodHelpers.TryGetChildWithName("The Home - Rect", canvas, out var mainMenu))
                throw new Exception("Failed to edit Main Menu: Could not find 'The Home - Rect'.");

            CreateMainMenuButton(mainMenu);
            _mainMenuObject = mainMenu;
        }

        public void EditUndertitle()
        {
            var underTitleList = new List<string>()
            {
                "Islands of Possibilities",
                "BK Mode Activated"
            };
            
            var scene = SceneManager.GetSceneByName("MenuRoot");
            if (!EverhoodHelpers.TryGetGameObjectWithName("Base", scene.GetRootGameObjects(), out var baseObj))
                throw new Exception("Failed to edit Main Menu: Could not find 'Base'.");

            if (!EverhoodHelpers.TryGetChildWithName("Canvas Base", baseObj, out var canvas))
                throw new Exception("Failed to edit Main Menu: Could not find 'Canvas Base'.");

            if (!EverhoodHelpers.TryGetChildWithName("Logo - Rect", canvas, out var logo))
                throw new Exception("Failed to edit Main Menu: Could not find 'Logo - Rect'.");

            if (!EverhoodHelpers.TryGetChildWithName("Undertitles", logo, out var undertitles))
                throw new Exception("Failed to edit Main Menu: Could not find 'Undertitles'.");

            foreach (Transform child in undertitles)
                child.gameObject.SetActive(false);

            var first = undertitles.GetChild(0);
            first.GetComponent<TextMeshProUGUI>().text = underTitleList[Random.Range(0, underTitleList.Count)];
            first.gameObject.SetActive(true);
        }

        private void CreateMainMenuButton(Transform mainMenu)
        {
            var backgrounds = mainMenu.GetChild(0);
            var third = backgrounds.GetChild(2);
            var thirdImage = third.GetComponent<Image>();
            var fourthImage = backgrounds.GetChild(3).GetComponent<Image>();

            var newImage = GameObject.Instantiate(third, backgrounds);
            newImage.GetComponent<Image>().color = Color.Lerp(thirdImage.color, fourthImage.color, 0.5f);
            newImage.SetSiblingIndex(3);
            for (int i = 3; i < backgrounds.childCount; i++)
                backgrounds.GetChild(i).GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 35);

            var actualMenu = mainMenu.GetChild(1);
            var archipelagoButton = GameObject.Instantiate(actualMenu.GetChild(2), actualMenu);
            GameObject.Destroy(archipelagoButton.GetComponent<TextMeshProLocalizable>());
            var textVisual = archipelagoButton.GetComponent<TextMeshProUGUI>();
            textVisual.text = "Archipelago";
            archipelagoButton.SetSiblingIndex(3);
            for (int i = 3; i < actualMenu.childCount - 1; i++)
                actualMenu.GetChild(i).GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, 35);

            var button = archipelagoButton.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnArchipelagoClick);
        }

#endregion

#region Version Check

        //Todo: Use Proper API
        private async Task CheckForNewVersion()
        {
            _waiting = true;
            try
            {
                //This is a very dumb, and likely to not work forever method to check updates.
                //Todo: Move to HTTPClient.
                var request = WebRequest.CreateHttp("https://github.com/DeamonHunter/ArchipelagoMuseDash/releases/latest");
                var response = await request.GetResponseAsync();

                var resolvedUrl = response.ResponseUri.ToString();
                response.Close();

                Globals.Logging.Log("GithubCheck", resolvedUrl);
                if (resolvedUrl.EndsWith("/"))
                    resolvedUrl = resolvedUrl.Substring(0, resolvedUrl.Length - 1);

                var lastIndex = resolvedUrl.LastIndexOf('/');
                if (lastIndex > 0)
                {
                    var version = resolvedUrl.Substring(lastIndex + 1);
                    _checkVersionState = version == "v" + _trueVersion ? VersionCheckState.UpToDate : VersionCheckState.NewVersion;
                    if (_checkVersionState == VersionCheckState.NewVersion)
                        _newVersionValue = version;
                }

                response.Close();
            }
            catch (Exception e)
            {
                Globals.Logging.Error("GithubCheck", e);
                _checkVersionState = VersionCheckState.Errored;
                _newVersionValue = e.Message;
            }
            finally
            {
                _waiting = false;
            }
        }

        private enum VersionCheckState
        {
            NotChecked,
            Checking,
            UpToDate,
            NewVersion,
            Errored
        }

#endregion
    }
}