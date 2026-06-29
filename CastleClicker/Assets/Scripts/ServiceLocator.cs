using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public static void Register<T>(T service)
    {
        services[typeof(T)] = service;
    }

    public static void Unregister<T>()
    {
        services.Remove(typeof(T));
    }

    public static T Resolve<T>()
    {
        if (services.TryGetValue(typeof(T), out var service))
        {
            return (T)service;
        }
        throw new Exception($"Service of type {typeof(T)} not registered.");
    }

    public static bool TryResolve<T>(out T service)
    {
        if (services.TryGetValue(typeof(T), out var s))
        {
            service = (T)s;
            return true;
        }
        service = default;
        return false;
    }

    public static void Clear()
    {
        services.Clear();
    }
}
