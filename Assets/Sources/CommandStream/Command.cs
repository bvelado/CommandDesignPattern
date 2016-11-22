using UnityEngine;

public interface ICommand
{
    void execute();
    void undo();
}
