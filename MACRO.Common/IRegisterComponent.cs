using Microsoft.Practices.Unity;

namespace MACRO.Common
{
    public interface IRegisterComponent
    {
        void RegisterType<TFrom, TTo>(bool withInterception = false) where TTo : TFrom;

        void RegisterTypeWithControlledLifeTime<T>(bool withInterception = false);

        void RegisterType<TFrom, TTo>(params InjectionMember[] members) where TTo : TFrom;

        void RegisterTypeWithControlledLifeTime<T>();

        void RegisterInstance<T>(T instance);
    }
}
