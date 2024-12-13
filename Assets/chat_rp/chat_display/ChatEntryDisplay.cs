using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatEntryDisplay : MonoBehaviour
{
    public ChatEntry m_chatEntry;

    // references
    public TMP_Text m_textMessage;
    public Image m_imageChar;

    public void SetEntry(ChatEntry _chatEntry)
    {
        m_chatEntry = _chatEntry;

        if (m_chatEntry.messageSelected != null)
            m_textMessage.text = m_chatEntry.messageSelected.strText;
        if (m_chatEntry.rpcharBy != null)
            m_imageChar.sprite = m_chatEntry.rpcharBy.sprite;
        m_textMessage.ForceMeshUpdate();
    }

}
