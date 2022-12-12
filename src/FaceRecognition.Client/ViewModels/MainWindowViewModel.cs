
using FaceRecognition.Client.Services;

namespace FaceRecognition.Client.ViewModels;

class MainWindowViewModel : ViewModelBase
{
    private readonly IOService _service;
    public MainWindowViewModel(IOService service)
    {
        _service = service;
        Greeting = _service.Open();
    }

    public string Greeting { get; set; } =  "Welcome to Avalonia!";
}
