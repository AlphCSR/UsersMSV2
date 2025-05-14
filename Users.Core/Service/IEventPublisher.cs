using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Domain.Entities;

namespace UsersMS.Core.Service
{
    public interface IEventPublisher
    {
        Task PublishUserCreatedAsync(User user);
    }
}
