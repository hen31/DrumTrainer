using MahApps.Metro.SimpleChildWindow;
using System.Threading.Tasks;

namespace DrumTrainer.ViewModels
{
    public interface IView
    {
        Task ShowChildWindowAsync(ChildWindow childWindow);
        Task<T> ShowChildWindowAsync<T>(ChildWindow childWindow);
        
    }
}