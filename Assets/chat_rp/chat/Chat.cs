using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chat : TrackedObject
{
    public List<ChatEntry> liEntries = new List<ChatEntry>();

    // events when entries are added/removed
    public event Action<ChatEntry> eventOnEntryAdded;
    public event Action<ChatEntry> eventOnEntryRemoved;

    public void AddEntry(ChatEntry _chatEntry)
    {
        liEntries.Add(_chatEntry);
        eventOnEntryAdded?.Invoke(_chatEntry);
        Debug.Log("Chat entry added: " + _chatEntry.guid);
    }
}

/// <summary>
/// An entry in the chat. Can contain multiple messages, of which one is selected.
/// </summary>
public class ChatEntry : TrackedObject
{
    public Guid guidByChar = default;
    public List<Message> liMessageAlternatives { get; private set; } = new List<Message>();
    public Guid guidMessageSelected { get; private set; } = default;

    public void AddMessage(Message _message, bool _bSelectNewMessage = true)
    {
        liMessageAlternatives.Add(_message);
        if (liMessageAlternatives.Count == 1 || _bSelectNewMessage)
        {
            guidMessageSelected = _message.guid;
        }
    }

    public Message messageSelected
    {
        get
        {
            foreach (Message message in liMessageAlternatives)
            {
                if (message.guid == guidMessageSelected)
                {
                    return message;
                }
            }
            return null;
        }
        set
        {
            guidMessageSelected = value.guid;
        }
    }

    public RPChar rpcharBy
    {
        get
        {
            return RPChar.GetByGuid(guidByChar);
        }
    }
}

public class Message : TrackedObject
{
    public string strText = "";
    public Guid guidChat = default;
    public InfoVerbose infoVerbose = new InfoVerbose();
    public DateTime dateSent = DateTime.UtcNow;

    public class InfoVerbose
    {
        public string strPrompt = "";
        public string strApi = "";
        public string strModel = "";
        public DateTime dateGenerationStarted = default;
        public DateTime dateGenerationEnded = default;
        // settings (maybe also other generation parameters like backend model...)
    }
}
