using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connectlive.Proxy;
public interface IProxy
{
    Task<T> Get<T>(RequestCommand request);
    Task<T> Post<T>(RequestCommand request);
    Task<T> Delete<T>(RequestCommand request);
    Task<T> Put<T>(RequestCommand request);
}
