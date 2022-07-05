namespace Common
{
    public interface IConnector
    {
        Port A { get; }
        Port B { get; }
        bool InCollition { get; set; }

        bool IsEmpty(Port port);
        void ReceiveCollition(Port port);
        void ReceiveData(Data data, Port p);
        void CleanConnector();
    }
}