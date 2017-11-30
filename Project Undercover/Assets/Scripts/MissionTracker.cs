﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionTracker : Photon.PunBehaviour
{
    const string COMPLETED_TEXT = "(COMPLETED) ";
    const string MISSION_LOG_TEXT = "Mission Log: _";

    const string GUARD_MISSION_STATEMENT =
        "Greetings, Overseer,\n\n" +
        "You are tasked with ensuring the security of this peace summit. We know that " +
        "various foreign forces are determined to undermine our progress here, but you " +
        "must make sure they do not succeed. Several enemies of the state will most " +
        "likely be infiltrating the party tonight, and it is your task to watch for " +
        "suspicious behavior and arrest these agents.\n\n" +
        "The security of our great nation rests in your hands. Good luck.";

    const string SPY_MISSION_STATEMENT =
        "Greetings, Agent,\n\n" +
        "You are tasked with infiltrating the party of Tel Nikiri, the tyrant dictator of " +
        "the nation Rhaedoria. Once there, you must perform several covert operations to " +
        "destabilize the political climate enough to bring freedom to this oppressed country.\n\n" +
        "As always, should you be caught or killed, the Secretary will disavow any knowledge " +
        "of your actions. Good luck.";

    const int MAX_GUARD_POINTS = 5;

    const float GUARD_NOTIFICATION_DELAY = 5.0f;
    const float GUARD_NOTIFICATION_TIME = 10.0f;
    const float MISSION_NOTIFICATION_TIME = 20.0f;

    private static MissionTracker mSingleton;

    public Text spyMissionLogText;
    public GameObject guardNotificationPanel;
    public Text guardNotificationText;

    private IDictionary<string, Mission> mMissionLog; // map interaction to mission
    private int mCompletedMissions;
    private int mGuardPoints;
    private int mGuardIncorrectGuesses;

    private class Mission
    {
        private string mAlertText;      // text shown to overseers after mission completes
        private string mDescription;    // description of the mission for agents

        public string AlertText { get { return mAlertText; } }

        public bool Completed { get; set; }

        public string Description { get { return mDescription; } }

        public Mission(string description, string alertText)
        {
            mAlertText = alertText;
            mDescription = description;
            Completed = false;
        }
    }

    /**
     * The Singleton instance of the MissionTracker.
     */
    public static MissionTracker Singleton { get { return mSingleton; } }

    public static bool IsGuard { get; set; }

    /**
     * Guard caught a spy.
     */
    public void CaughtAgent(int agentId)
    {
        photonView.RPC("CaughtAgentRPC", PhotonTargets.All, agentId);
    }

    /**
     * Guard caught a partygoer and must be punished.
     */
    public void CaughtIncorrect()
    {
        photonView.RPC("CaughtIncorrectRPC", PhotonTargets.All);
    }

    /**
    * Complete a mission, maybe.
    */
    public void CompleteMission(string interactionName)
    {
        // if the interactionName is invalid, cry I guess
        if (!mMissionLog.ContainsKey(interactionName))
        {
            Debug.LogError("Invalid interactionName passed to MissionTracker: " + interactionName);
            return;
        }

        // grab the mission - if it's already complete, do nothing
        Mission m = mMissionLog[interactionName];
        if (m.Completed)
        {
            return;
        }

        // mark it as completed, then update the score and UI
        photonView.RPC("CompleteMissionRPC", PhotonTargets.All, interactionName);
    }

    /**
     * Initialize the MissionTracker with all missions.
     */
    void Start()
    {
        // set the singleton
        if (mSingleton)
        {
            Debug.Log("Multiple MissionTrackers in scene - please limit to one.");
        }
        mSingleton = this;

        // populate missions and set initial values
        mMissionLog = new Dictionary<string, Mission>();
        mCompletedMissions = 0;
        mGuardPoints = 0;
        mGuardIncorrectGuesses = 0;

        /*mMissionLog["statue_swap"] = new Mission(
            "Replace the marked statue with a bugged replica.",
            "We're picking up some electromagnetic interference. " +
            "Enemy forces have planted listening devices somewhere on the premises!"
        );*/

        mMissionLog["HideMessage"] = new Mission(
            "Plant critical intelligence in the designated book.",
            "One of the staff saw someone suspicious over by the bookcases. " +
            "The enemy moves among us."
        );

        /*mMissionLog["bathroom_kill"] = new Mission(
            "Assassinate Knight-Captain Brystol in the bathroom.",
            "Our men found Knight-Captain Brystol dead in the bathroom. " +
            "The enemy must be stopped!"
        );*/

        /*mMissionLog["npc_dance"] = new Mission(
            "Dance with Duchess Castra to distract her momentarily.",
            "The peace talks progress poorly. Duchess Castra keeps getting distracted on the dance floor. " +
            "Could this be a strategy of our enemies?"
        );*/

        mMissionLog["TellSecret"] = new Mission(
            "Trade intelligence with another spy at the party.",
            "One of our staff members spotted two individuals exchanging a secret. " +
            "Enemy infiltrators abound."
        );

        /*mMissionLog["spike_punch"] = new Mission(
            "spike_punch",
            "Lower the inhibitions of those in attendance by spiking the punch.",
            "Several of our guests have become extremely intoxicated. Our catering staff insist " +
            "that the punch was non-alcoholic, so we have determined that this must be the plot of " +
            "enemy agents."
        );*/

        UpdateMissionLog();
        StartCoroutine(DisplayMission());
    }

    /**
     * Update the Mission Log UI element.
     */
    private void UpdateMissionLog()
    {
        string text = MISSION_LOG_TEXT;
        foreach (KeyValuePair<string, Mission> pair in mMissionLog)
        {
            text += "\n\n";
            if (pair.Value.Completed)
            {
                text += COMPLETED_TEXT;
            }
            text += pair.Value.Description;
        }
        spyMissionLogText.text = text;
    }

    #region coroutines
    IEnumerator DisplayMission()
    {
        // pause for two seconds
        yield return new WaitForSeconds(1.0f);

        // set the mission text and render
        if (IsGuard)
        {
            guardNotificationText.text = GUARD_MISSION_STATEMENT;
        }
        else
        {
            guardNotificationText.text = SPY_MISSION_STATEMENT;
        }
        guardNotificationPanel.SetActive(true);

        // hide the panel after a certain amount of time
        yield return new WaitForSeconds(MISSION_NOTIFICATION_TIME);
        guardNotificationPanel.SetActive(false);
    }

    IEnumerator DisplayNotification(string text)
    {
        // only display for guards...
        if (!IsGuard)
        {
            yield break;
        }

        // wait before displaying notification
        yield return new WaitForSeconds(GUARD_NOTIFICATION_DELAY);

        // set the text and show the panel
        guardNotificationText.text = text;
        guardNotificationPanel.SetActive(true);

        // hide the panel after a certain amount of time
        yield return new WaitForSeconds(GUARD_NOTIFICATION_TIME);
        guardNotificationPanel.SetActive(false);

        yield return null;
    }
    #endregion

    #region rpc
    [PunRPC]
    void CompleteMissionRPC(string interactionName)
    {
        Mission m = mMissionLog[interactionName];
        m.Completed = true;
        mCompletedMissions++;

        float score = mCompletedMissions + mGuardIncorrectGuesses;
        ScorePanelController.Singleton.UpdateSpyScore(score / mMissionLog.Count);

        StartCoroutine(DisplayNotification(m.AlertText));
        UpdateMissionLog();
    }

    [PunRPC]
    void CaughtAgentRPC(int spyId)
    {
        mGuardPoints++;
        ScorePanelController.Singleton.UpdateGuardScore((float) mGuardPoints / MAX_GUARD_POINTS);

        // Get Random NPC
        var npcs = GameManager.ActiveManager.GetNpcs();
        int randInt = (int)(UnityEngine.Random.value * npcs.Count);
        if (PhotonNetwork.isMasterClient)
            GameManager.ActiveManager.photonView.RPC("ReplaceNPCWithSpyRPC", PhotonTargets.All, spyId, npcs[randInt].photonView.viewID);
    }

    [PunRPC]
    void CaughtIncorrectRPC()
    {
        mGuardIncorrectGuesses++;
        float score = mCompletedMissions + mGuardIncorrectGuesses;
        ScorePanelController.Singleton.UpdateSpyScore(score / mMissionLog.Count);
    }
    #endregion
}
