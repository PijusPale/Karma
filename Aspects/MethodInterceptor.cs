using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

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

                _logger.LogInformation($"{invocation.Method.Name} " +
                    $"called with parameters: {JsonSerializer.Serialize(invocation.Arguments)} " +
                    $"returned this response: {JsonSerializer.Serialize(invocation.ReturnValue)}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{invocation.Method} : {JsonSerializer.Serialize(ex)}");
                throw;
            }
        }
    }
}
