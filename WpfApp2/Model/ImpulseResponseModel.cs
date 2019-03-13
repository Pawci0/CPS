using Lib.Task3.FilterImpulseResponses;
using SciChart.Data.Model;

namespace WpfApp2.Model
{
    public class ImpulseResponseModel : BindableObject
    {
        private IImpulseResponse _impulseResponse;
        private string _name;

        public IImpulseResponse ImpulseResponse
        {
            get => _impulseResponse;
            set
            {
                _impulseResponse = value;
                OnPropertyChanged("Signal");
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
