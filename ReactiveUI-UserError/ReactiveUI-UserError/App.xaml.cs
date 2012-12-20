namespace ReactiveUI_UserError
{
    using ReactiveUI;

    public partial class App
    {
        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            base.OnStartup(e);
            RxApp.GetFieldNameForPropertyNameFunc = x =>
            {
                var arr = x.ToCharArray();
                arr[0] = char.ToLower(arr[0]);
                return '_' + new string(arr);
            };
        }
    }
}
