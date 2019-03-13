using Lib.Task3.WindowFunctions;
using SciChart.Data.Model;

namespace WpfApp2.Model
{
    public class WindowFunctionModel : BindableObject
    {
        private IWindowFunction _windowFunction;
        private string _name;

        public IWindowFunction WindowFunction
        {
            get => _windowFunction;
            set
            {
                _windowFunction = value;
                OnPropertyChanged("WindowFunction");
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}
