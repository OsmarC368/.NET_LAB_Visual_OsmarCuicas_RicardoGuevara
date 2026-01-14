namespace AppBlazor.Components
{
    public class TokenContainer
    {
        public string token { get; set; } = string.Empty;

        public void asingToken(string newToken)
        {
            token = newToken;
            NotifyStateChanged();
        }

        public event Action? Onchange;

        private void NotifyStateChanged() => Onchange?.Invoke();

        public void Clear()
        {
            token = string.Empty;
            NotifyStateChanged();
        }
    }
}