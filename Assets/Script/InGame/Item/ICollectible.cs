namespace InGame.Item
{
    public interface ICollectible
    {
        public string Name { get; }

        public void PullItem();
    }
}