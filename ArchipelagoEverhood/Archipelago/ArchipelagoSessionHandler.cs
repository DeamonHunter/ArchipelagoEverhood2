using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using ArchipelagoEverhood.Data;
using UnityEngine;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoSessionHandler
    {
        public bool LoggedIn { get; private set; }
        public ArchipelagoLogicHandler? LogicHandler;
        public ArchipelagoItemHandler? ItemHandler;

        private ArchipelagoSession? _currentSession;
        public int Slot { get; private set; }
        private int _team;
        private Dictionary<string, object>? _slotData;
        private bool _activateUpdate;

        public async Task<string?> TryFreshLogin(string ipAddress, string username, string password)
        {
            if (_currentSession != null)
                throw new Exception("Already Connected!");

            var session = ArchipelagoSessionFactory.CreateSession(ipAddress);
            session.Socket.ErrorReceived += (exception, message) => { Globals.Logging.Log("[ArchError]", $"{message} : {exception}"); };

            LoginResult? loginResult;
            try
            {
                await session.ConnectAsync();
                loginResult = await session.LoginAsync("Everhood 2", username, ItemsHandlingFlags.AllItems, null, null, null, password);
            }
            catch (TaskCanceledException e)
            {
                loginResult = new LoginFailure("Timed out. Please check connection info.");
                Globals.Logging.Error("Login", e);
            }

            if (!loginResult.Successful)
            {
                var failed = (LoginFailure)loginResult;
                //Todo: When does multiple errors show up? And should we handle that case here.
                return failed.Errors[0];
            }

            var successful = (LoginSuccessful)loginResult;

            session.MessageLog.OnMessageReceived += Globals.Logging.LogMessage;

            Slot = successful.Slot;
            _team = successful.Team;
            _slotData = successful.SlotData;
            _currentSession = session;

            return null;
        }

        public void StartSession()
        {
            FindObjects();
            LoggedIn = true;

            LogicHandler = new ArchipelagoLogicHandler(_currentSession!);
            ItemHandler = new ArchipelagoItemHandler(_currentSession!.ConnectionInfo.Slot, _currentSession!.ConnectionInfo.Team);

            SoulColor soulColor;
            if (_slotData!.TryGetValue("SoulColor", out var colorObj))
                soulColor = (SoulColor)((long)colorObj + 1);
            else
                soulColor = SoulColor.None;

            if (_slotData.TryGetValue("DoorKeys", out var doorKeys))
                Globals.EverhoodDoors.DoorRandoEnabled = (bool)doorKeys;

            long powerGemAmount = 3;
            if (_slotData.TryGetValue("DragonGems", out var powerGemObj))
                powerGemAmount = (long)powerGemObj;

            Globals.EverhoodOverrides.ArchipelagoConnected(_currentSession!.RoomState.Seed, soulColor, (int)powerGemAmount);
        }

        private void FindObjects()
        {
            Globals.ServicesRoot = GameObject.FindObjectsByType<ServicesRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();
            Globals.TopdownRoot = GameObject.FindObjectsByType<Main_TopdownRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();
            Globals.GameplayRoot = GameObject.FindObjectsByType<Main_GameplayRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();
            Globals.SceneManagerRoot = GameObject.FindObjectsByType<SceneManagerRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();
            
            //Cache Victory objects so its easier to check.
            Globals.BattleVictoryResult = (BattleVictoryResult)(typeof(Main_GameplayRoot).GetField("battleVictoryResult", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(Globals.GameplayRoot));
            Globals.VictoryScreenCanvas = (Canvas)(typeof(BattleVictoryResult).GetField("victoryCanvas", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(Globals.BattleVictoryResult));
        }

        public void SaveFileLoaded()
        {
            if (_currentSession == null)
                throw new Exception("Not Connected!");

            try
            {
                Globals.EverhoodBattles.CompleteChecksLoadedFromSave(_currentSession.Locations);
                Globals.EverhoodChests.CompleteChecksLoadedFromSave(_currentSession.Locations);
                LogicHandler!.SetAcceptingItems(true);
                _activateUpdate = true;
            }
            catch (Exception e)
            {
                Globals.Logging.Error("Loading", e);
            }
        }

        public void Update()
        {
            if (!_activateUpdate || !LoggedIn)
                return;

            LogicHandler!.Update();
            ItemHandler!.Update();
        }

        public void QuitToMenu()
        {
            if (_currentSession == null)
                return;

            LogicHandler!.SetAcceptingItems(false);
            _activateUpdate = false;
        }

        public void SendCompletion() => _currentSession!.SetGoalAchieved();

        public async Task Disconnect(bool error = false)
        {
            try
            {
                if (_currentSession == null)
                    throw new Exception("Trying to disconnect from a non-existent connection?");

                if (_currentSession.Socket != null)
                    await _currentSession.Socket.DisconnectAsync();
                Globals.EverhoodOverrides?.ArchipelagoDisconnected();
                LoggedIn = false;
                LogicHandler = null;
                ItemHandler = null;
                _currentSession = null;
                if (!error)
                    Globals.LoginHandler.StopWaiting(null);
            }
            catch (Exception e)
            {
                Globals.LoginHandler.StopWaiting(e);
            }
        }
    }
}