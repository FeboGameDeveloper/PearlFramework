namespace Pearl
{
    public interface IFSM
    {
        void CheckTransitions(bool forceFinishState);

        void ChangeLabel(string newLabel);

        string GetLabel();

        void StartFSM();

        void UpdateVariable<T>(string nameVar, T content);

        T GetVariable<T>(string nameVar);

        T RemoveVariable<T>(string nameVar);
    }


}
