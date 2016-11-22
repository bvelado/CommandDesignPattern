using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TypeReferences;

[Serializable]
public class CommandViewByCommandType
{
    [ClassImplements(typeof(ICommand))]
    public List<ClassTypeReference> CommandType;
    public GameObject CommandView;
}

public class CommandStream : MonoBehaviour {

    private static CommandStream instance;
    public static CommandStream Instance
    {
        get
        {
            return instance;
        }
    }

    protected CompensationConversation Conversation;

    public Action<bool> CanUndo;
    public Action<bool> CanRedo;

    [SerializeField]
    private List<CommandViewByCommandType> commandViews;
    [SerializeField]
    private Transform CommandViewContainer;
    private Dictionary<ICommand, GameObject> commandViewLookup;
    
    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

        Conversation = new CompensationConversation();

        commandViewLookup = new Dictionary<ICommand, GameObject>();

        Conversation.CommandExecuted += AddCommandView;
        Conversation.CommandRedone += AddCommandView;
        Conversation.CommandUndone += RemoveCommandView;

        Conversation.CanUndo += Conversation_CanUndoHandler;
        Conversation.CanRedo += Conversation_CanRedoHandler;
    }
    
    void OnDestroy()
    {
        Conversation.CommandExecuted -= AddCommandView;
        Conversation.CommandRedone -= AddCommandView;
        Conversation.CommandUndone -= RemoveCommandView;

        Conversation.CanUndo -= Conversation_CanUndoHandler;
        Conversation.CanRedo -= Conversation_CanRedoHandler;
    }

    public void Push(ICompensableCommand command)
    {
        Conversation.exec(command);
    }

    public void Undo()
    {
        Conversation.undo();
    }

    public void Redo()
    {
        Conversation.redo();
    }

    void AddCommandView(ICommand command)
    {
        foreach(var commandView in commandViews)
        {
            foreach(var commandType in commandView.CommandType)
            {
                if(commandType.Type == command.GetType())
                {
                    GameObject go = Instantiate(commandView.CommandView, CommandViewContainer, false) as GameObject;
                    go.GetComponent<IFillable>().Fill(command);

                    commandViewLookup.Add(command, go);
                }
            }
        }
    }

    void RemoveCommandView(ICommand command)
    {
        if (commandViewLookup.ContainsKey(command))
        {
            Destroy(commandViewLookup[command]);
            commandViewLookup.Remove(command);
        }
    }

    void Conversation_CanUndoHandler(bool canUndo)
    {
        if(CanUndo != null)
        {
            CanUndo(canUndo);
        }
    }

    void Conversation_CanRedoHandler(bool canRedo)
    {
        if (CanRedo != null)
        {
            CanRedo(canRedo);
        }
    }
}
