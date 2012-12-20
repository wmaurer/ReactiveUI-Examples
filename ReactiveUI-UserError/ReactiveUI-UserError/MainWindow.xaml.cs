namespace ReactiveUI_UserError
{
    using System.Reactive.Linq;
    using System.Windows;

    using ReactiveUI;
    using ReactiveUI.Xaml;

    public partial class MainWindow : IViewFor<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new MainWindowViewModel();
            this.BindCommand(ViewModel, x => x.CalculateTheAnswer);
            this.OneWayBind(ViewModel, x => x.TheAnswer);

            UserError.RegisterHandler(err =>
            {
                var result = MessageBox.Show("An error occurred. Would you like to try again?", "Error", MessageBoxButton.YesNo);
                return Observable.Return(result == MessageBoxResult.Yes ? RecoveryOptionResult.RetryOperation : RecoveryOptionResult.CancelOperation);
            });
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainWindowViewModel)value; }
        }

        public MainWindowViewModel ViewModel { get; set; }
    }
}
