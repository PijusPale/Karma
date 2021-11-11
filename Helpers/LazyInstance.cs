using System;
using Microsoft.Extensions.DependencyInjection;

namespace Karma.Helpers
{
    public class LazyInstance<T> : Lazy<T>
    {
        public LazyInstance(IServiceProvider serviceProvider) :
            base(() => serviceProvider.GetRequiredService<T>())
        {

        }
    }
}