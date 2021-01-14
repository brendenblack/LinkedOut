using System;
using System.Collections.Generic;
using System.Text;

namespace LinkedOut.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        bool IsAuthenticated { get; }

        string FirstName { get; }

        string Email { get; }


    }
}
