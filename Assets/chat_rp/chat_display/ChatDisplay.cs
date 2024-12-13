using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatDisplay : MonoBehaviour
{
    public Chat m_chat = new Chat();

    public List<ChatEntryDisplay> liEntryDisplay = new List<ChatEntryDisplay>();

    // references
    [Header("References")]
    public ChatEntryDisplay m_entryDisplayPrefab;
    public Transform m_transEntryParent;
    public TMP_InputField m_inputFieldUser;
    public Button m_buttonSendMessage;

    private void Start()
    {
        m_chat.eventOnEntryAdded += OnEntryAdded;
        m_chat.eventOnEntryRemoved += OnEntryRemoved;

        m_buttonSendMessage.onClick.AddListener(OnButtonSendMessageClicked);
    }

    private void OnButtonSendMessageClicked()
    {
        if (m_inputFieldUser.text.Length > 0)
        {
            Message message = new Message();
            message.strText = m_inputFieldUser.text;
            ChatEntry chatEntry = new ChatEntry();
            chatEntry.AddMessage(message);

            m_chat.AddEntry(chatEntry);
            m_inputFieldUser.text = "";
        }
    }

    private void SpawnChatEntry(ChatEntry _chatEntry)
    {
        ChatEntryDisplay entryDisplay = Instantiate(m_entryDisplayPrefab, m_transEntryParent);
        entryDisplay.SetEntry(_chatEntry);
        liEntryDisplay.Add(entryDisplay);

        StartCoroutine(ieUpdateRects(entryDisplay));
    }

    private IEnumerator ieUpdateRects(ChatEntryDisplay _entryDisplay)
    {
        yield return new WaitForSeconds(0.2f);
        _entryDisplay.GetComponent<RectTransform>().ForceUpdateRectTransforms();
    }

    private void OnEntryAdded(ChatEntry _chatEntry)
    {
        Debug.Log("Spawning chat entry: " + _chatEntry.guid);
        SpawnChatEntry(_chatEntry);
    }

    private void OnEntryRemoved(ChatEntry _chatEntry)
    {
        liEntryDisplay.RemoveAll((ChatEntryDisplay _entryDisplay) => _entryDisplay.m_chatEntry == _chatEntry);
    }
}
