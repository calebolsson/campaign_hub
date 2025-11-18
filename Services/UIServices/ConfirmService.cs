namespace campaign_hub.Services.UIServices.ConfirmService
{
    public class ConfirmService
    {
        private TaskCompletionSource<bool>? _tcs;
        //public event Action<(string Title, string Message)>? OnShow;
        public event Action<string>? OnShow;
        //public Task<bool> Confirm(string message, string title = "Confirm")
        //{
        //    _tcs = new TaskCompletionSource<bool>();
        //    OnShow?.Invoke((title, message));
        //    return _tcs.Task;
        //}
        public Task<bool> Confirm(string message)
        {
            _tcs = new TaskCompletionSource<bool>();
            OnShow?.Invoke(message);
            return _tcs.Task;
        }
        public void Confirmed()
        {
            _tcs?.SetResult(true);
        }
        public void Cancelled()
        {
            _tcs?.SetResult(false);
        }
    }
}
