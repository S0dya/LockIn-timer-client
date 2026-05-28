using System;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Http;

namespace App.Backend.Services
{
    public abstract class BaseService
    {
        protected async UniTask<Result<T>> Execute<T>(UniTask<T> task, string error)
        {
            try
            {
                var response = await task;
                
                return Result<T>.Success(response);
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, e.ToString());
                
                return Result<T>.Fail(error);
            }
        }
    }
}