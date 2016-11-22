using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void execute();
}

public interface IConversation<T> where T : ICommand
{
    void exec(T command);
    void undo();
    void redo();
}

public interface ICompensableCommand : ICommand
{
    void compensate();
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="C">Type de commande</typeparam>
/// <typeparam name="S">Type d'etat stocké</typeparam>
public abstract class AbstractConversation<C, S> : IConversation<C> where C : ICommand
{
    protected Stack<S> undoStack, redoStack;

    public AbstractConversation()
    {
        undoStack = new Stack<S>();
        redoStack = new Stack<S>();
    }

    public abstract void exec(C command);

    public abstract void redo();

    public abstract void undo();
}

public class CompensationConversation : AbstractConversation<ICompensableCommand, ICompensableCommand>
{
    public Action<ICompensableCommand> CommandExecuted;
    public Action<ICompensableCommand> CommandUndone;
    public Action<ICompensableCommand> CommandRedone;

    public Action<bool> CanUndo;
    public Action<bool> CanRedo;

    public override void exec(ICompensableCommand command)
    {
        command.execute();

        if(CommandExecuted!=null)
            CommandExecuted(command);

        undoStack.Push(command);
        redoStack.Clear();

        CheckStacksStates();
    }

    public override void redo()
    {
        if (redoStack.Count > 0)
        {
            var latestCommand = redoStack.Pop();
            latestCommand.execute();

            if (CommandRedone != null)
                CommandRedone(latestCommand);

            undoStack.Push(latestCommand);
        }

        CheckStacksStates();
    }

    public override void undo()
    {
        if (undoStack.Count > 0)
        {
            var latestCommand = undoStack.Pop();
            latestCommand.compensate();

            if (CommandUndone != null)
                CommandUndone(latestCommand);

            redoStack.Push(latestCommand);
        }

        CheckStacksStates();
    }

    void CheckStacksStates()
    {
        if(CanUndo != null)
            CanUndo(undoStack.Count > 0);

        if (CanRedo != null)
            CanRedo(redoStack.Count > 0);
    }
}