using CommunityToolkit.Mvvm.ComponentModel;

namespace BlazorAppPoc.Models
{
    public partial class User : ObservableObject
    {
        [ObservableProperty]
        private string _firstName;
        [ObservableProperty]
        private string _lastName;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _identityprovider;
        [ObservableProperty]
        private List<Todo> _toDos; 
    }
}
