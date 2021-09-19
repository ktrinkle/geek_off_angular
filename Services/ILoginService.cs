using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeekOff.Models;

namespace GeekOff.Services
{
    public interface ILoginService
    {
        Task<string> Login(string emailAddr);
    }
}