using Castle.DynamicProxy;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using System;

namespace Karma.Aspects
{
    public class MethodInterceptor : IInterceptor
    {
        private readonly ILogger _logger;
        public MethodInterceptor(ILogger<MethodInterceptor> logger)
        {
            _logger = logger;
        }
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();

                _logger.LogInformation($"Method {invocation.Method.Name} " +
                    $"called with these parameters: {JsonConvert.SerializeObject(invocation.Arguments)}" +
                    $"returned this response: {JsonConvert.SerializeObject(invocation.ReturnValue)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in method {invocation.Method} : {JsonConvert.SerializeObject(ex)}");
                throw;
            }
        }
    }
}
