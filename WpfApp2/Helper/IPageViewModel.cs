using Lib;

namespace WpfApp2.Helper
{
    public interface IPageViewModel
    {
        PageEnum NameOfPage { get; }
        ChartDetailsEnum ChartDetailName { get; set; }
        string Title { get; set; }
        RealSignal Signal { get; set; }
    }
}