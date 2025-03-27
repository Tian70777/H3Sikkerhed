using SymmetricEncryptionAPI.Services.CryptoServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SymmetricEncryptionApp.Services
{
    public class CryptoServiceFactory
    {
        private readonly Dictionary<string, ICryptoService> _services;

        // The constructor takes all the ICryptoService instances 
        // that have been registered in the DI container (in program,cs
        //  The DI container will automatically inject all the registered implementations of the ICryptoService interface.
        // Then, in the constructor, the code uses each service's Name property to build a dictionary.
        public CryptoServiceFactory(IEnumerable<ICryptoService> services)
        {
            // the ToDictionary(s => s.Name) part creates a dictionary where
            // each key is the value of the service’s Name property, and the value is the corresponding instance. 
            _services = services.ToDictionary(s => s.Name);
        }

        // Get the list of available method names
        public List<string> GetAvailableServices()
        {
            return _services.Keys.ToList();
        }

        // Retrieve the correct service by name
        public ICryptoService GetServiceByName(string name)
        {
            try
            {
                if (_services.TryGetValue(name, out var service))
                {
                    return service;
                }
                throw new KeyNotFoundException($"The given key '{name}' was not present in the dictionary.");
            }
            catch (KeyNotFoundException ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Service not found. Please check the service name.", ex);
            }
        }
    }
}
