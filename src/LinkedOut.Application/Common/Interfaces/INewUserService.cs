using System.Threading.Tasks;

namespace LinkedOut.Application.Common.Interfaces
{
    public interface INewUserService
    {
        Task InitializeNewUser(string userid);
    }
}
