namespace InGame.Item
{
    public interface ICollectible
    {
        public string Name { get; }

        /// <summary>
        /// ƒvƒŒƒCƒ„[‚ªG‚ê‚½
        /// </summary>
        public void OnPlayerTouch();
    }
}