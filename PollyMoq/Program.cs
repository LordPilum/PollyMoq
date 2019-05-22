using Autofac;

namespace PollyMoq
{
    public class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<PollyModule>();
            builder.RegisterType<Application>().As<IStartable>().SingleInstance();

            builder.Build();
        }
    }
}
