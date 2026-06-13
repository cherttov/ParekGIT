using ParekGIT.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParekGIT.Data.Interfaces
{
    public interface ISettingsStore
    {
        Task<UserSettings> GetUserSettingsAsync();
        Task SaveUserSettingsAsync(UserSettings settings);
    }
}
