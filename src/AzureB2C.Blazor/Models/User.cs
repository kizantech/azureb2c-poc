using CommunityToolkit.Mvvm.ComponentModel;

namespace AzureB2C.Blazor.Models
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
    }
}
