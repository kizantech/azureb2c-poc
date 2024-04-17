using CommunityToolkit.Mvvm.ComponentModel;
using Finbuckle.MultiTenant;
using System.ComponentModel.DataAnnotations;

namespace BlazorAppPoc.Models
{
    [MultiTenant]
    public partial class Todo : ObservableObject
    {
        [ObservableProperty]
        [Key]
        private string _tenantId;
        [ObservableProperty]
        private string _title;
        [ObservableProperty]
        private string _description;
    }
}
