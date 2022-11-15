namespace Pearl
{
    public interface IElement<T, F>
    {
        F ID { get; set; }

        bool Equals(F anotherID);

        bool Equals(T anotherContent);

        void SetContent(T content);

    }

}