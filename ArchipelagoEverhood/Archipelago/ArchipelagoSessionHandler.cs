using System;
using System.Collections.Generic;
using System.Linq;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using UnityEngine;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoSessionHandler
    {
        public bool LoggedIn { get; private set; }
        public ArchipelagoLogicHandler? LogicHandler;

        private ArchipelagoSession? _currentSession;
        public int Slot { get; private set; }
        private int _team;
        private Dictionary<string, object>? _slotData;

        public bool TryFreshLogin(string ipAddress, string username, string password, out string? reason)
        {
            if (_currentSession != null)
                throw new NotImplementedException("Changing sessions is not implemented atm.");

            var session = ArchipelagoSessionFactory.CreateSession(ipAddress);
            session.Socket.ErrorReceived += (exception, message) => { Globals.Logging.Log("[ArchError]", $"{message} : {exception}"); };

            var loginResult = session.TryConnectAndLogin("Everhood2", username, ItemsHandlingFlags.AllItems, password: password);

            if (!loginResult.Successful)
            {
                var failed = (LoginFailure)loginResult;

                //Todo: When does multiple errors show up? And should we handle that case here.
                reason = failed.Errors[0];
                return false;
            }

            var successful = (LoginSuccessful)loginResult;

            Slot = successful.Slot;
            _team = successful.Team;
            _slotData = successful.SlotData;
            _currentSession = session;

            reason = null;
            return true;
        }

        public void StartSession()
        {
            Globals.ServicesRoot = GameObject.FindObjectsByType<ServicesRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None).First();
            LoggedIn = true;

            LogicHandler = new ArchipelagoLogicHandler(_currentSession!);
            Globals.EverhoodOverrides.ArchipelagoConnected();
        }

        public void SaveFileLoaded()
        {
            if (_currentSession == null)
                return;

            Globals.EverhoodBattles.CompleteChecksLoadedFromSave(_currentSession.Locations);
            LogicHandler!.SetAcceptingItems(true);
        }

        public void QuitToMenu()
        {
            if (_currentSession == null)
                return;

            LogicHandler!.SetAcceptingItems(false);
        }
    }
}