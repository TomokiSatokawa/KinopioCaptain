namespace InGame.Gimmick
{
    /// <summary>
    /// ゲームオブジェクトのインタラクション操作を定義するインターフェース
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 使用可能かどうかを取得する
        /// </summary>
        public bool CanInteract();

        /// <summary>
        /// 使用する
        /// </summary>
        public void Interact();
    }
}
