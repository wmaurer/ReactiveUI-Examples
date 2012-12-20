namespace ReactiveUI_UserError
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using ReactiveUI;
    using ReactiveUI.Xaml;

    public class MainWindowViewModel : ReactiveObject
    {
        public ReactiveAsyncCommand CalculateTheAnswer { get; set; }

        public MainWindowViewModel()
        {
            CalculateTheAnswer = new ReactiveAsyncCommand();

            CalculateTheAnswer.RegisterAsyncTask(_ => AnswerCalculator())
                .ToProperty(this, x => x.TheAnswer);

            CalculateTheAnswer.ThrownExceptions
                .SelectMany(ex => UserError.Throw(ex.Message))
                .Subscribe(result =>
                {
                    if (result == RecoveryOptionResult.RetryOperation)
                    {
                        _retryCalculate = true;
                        CalculateTheAnswer.Execute(null);
                    }
                    else
                        Application.Current.Shutdown();
                });
        }

        private readonly ObservableAsPropertyHelper<int?> _theAnswer;
        public int? TheAnswer
        {
            get { return _theAnswer.Value; }
        }

        private bool _retryCalculate;
        private Task<int?> AnswerCalculator()
        {
            var task = Task.Factory.StartNew(() =>
            {
                if (!_retryCalculate)
                    throw new ApplicationException("Unable to calculate answer, because I don't know what the question is");
                return (int?)42;
            });

            return task;
        }
    }
}
