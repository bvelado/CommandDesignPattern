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
            if (instance == null)
            {
                var go = new GameObject("Command Stream");
                instance = go.AddComponent<CommandStream>();
            }
            return instance;
        }
    }

    private Stack<ICommand> _undoStream;
    [SerializeField]
    private int _maxLength;

    private Stack<ICommand> _redoStream;
    
    public event Action<bool> CanRedo;
    public event Action<bool> CanUndo;

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

        commandViewLookup = new Dictionary<ICommand, GameObject>();
    }

    void Start()
    {
        _undoStream = new Stack<ICommand>();
        _redoStream = new Stack<ICommand>();
    }

    public void Push(ICommand command)
    {
        if (_undoStream.Count > _maxLength)
            _undoStream.Pop();
        
        command.execute();
        AddCommandView(command);
        _undoStream.Push(command);

        CheckStreams();
    }

    public void Undo()
    {
        if(_undoStream.Count > 0)
        {
            RemoveCommandView(_undoStream.Peek());
            _redoStream.Push(_undoStream.Peek());
            _undoStream.Pop().undo();
        }
        CheckStreams();
    }

    public void Redo()
    {
        if (_redoStream.Count > 0)
        {
            AddCommandView(_redoStream.Peek());
            _undoStream.Push(_redoStream.Peek());
            _redoStream.Pop().execute();
        }
        CheckStreams();
    }

    void CheckStreams()
    {
        if(CanUndo!=null)
            CanUndo(_undoStream.Count > 0);
        if(CanRedo!=null)
            CanRedo(_redoStream.Count > 0);
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
}
