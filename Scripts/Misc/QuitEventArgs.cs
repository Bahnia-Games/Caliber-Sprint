namespace Assets.Git.Scripts.Misc
{
    public class QuitEventArgs : System.EventArgs
    {
        public enum QuitReason
        {
            request,
            error,
            force
        }

        public QuitReason quitReason;
        public string Requester;
    }
}
