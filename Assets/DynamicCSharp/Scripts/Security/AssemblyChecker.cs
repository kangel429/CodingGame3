﻿using System;
using System.Collections.Generic;
using System.IO;
using Mono.Cecil;

namespace DynamicCSharp.Security
{
    /// <summary>
    /// Represents an error generated as a result of a restriction check.
    /// </summary>
    public struct AssemblySecurityError
    {
        // Public
        /// <summary>
        /// The assembly that the restricted module is defined in.
        /// </summary>
        public string assemblyName;     // The name of the assembly that breaches the security policy
        /// <summary>
        /// The name of the restricted module.
        /// </summary>
        public string moduleName;       // The name of the module that breaches the security policy
        /// <summary>
        /// The error message generated by the restriction check.
        /// </summary>
        public string securityMessage;  // The message for the security restriction
        /// <summary>
        /// The type name of the restriction checker.
        /// </summary>
        public string securityType;     // The type of security restriction

        // Methods
        /// <summary>
        /// Prints the error as a formatted string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Security Check Failed ({0}) : [{1}, {2}] : {3}", securityType, assemblyName, moduleName, securityMessage);
        }
    }

    internal sealed class AssemblyChecker
    {
        // Private
        private AssemblySecurityError[] errors = new AssemblySecurityError[0];

        // Properties
        public AssemblySecurityError[] Errors
        {
            get { return errors; }
        }

        public bool HasErrors
        {
            get { return errors.Length > 0; }
        }

        public int ErrorCount
        {
            get { return errors.Length; }
        }

        // Methods
        public void SecurityCheckAssembly(byte[] assemblyData)
        {
            // Clear any previous errors
            ClearErrors();

            AssemblyDefinition defenition = null;

            // Create a memory stream
            using (MemoryStream stream = new MemoryStream(assemblyData))
            {
                // Read the assembly defenition
                defenition = AssemblyDefinition.ReadAssembly(stream);
            }

            // Process all modules
            foreach (ModuleDefinition module in defenition.Modules)
            {
                // Check the module
                SecurityCheckModule(defenition.Name, module);
            }
        }

        private void SecurityCheckModule(AssemblyNameDefinition assemblyName, ModuleDefinition module)
        {
            // Get all restrictions
            IEnumerable<Restriction> restrictions = DynamicCSharp.Settings.Restrictions;

            // Check all restrictions
            foreach(Restriction restriction in restrictions)
            {
                // Try to verify the module with the specified restriction
                if(restriction.Verify(module) == false)
                {
                    // Failed to verify the module - create an error
                    CreateError(assemblyName.Name, module.Name, restriction.Message, restriction.GetType().Name);
                }
            }
        }

        private void ClearErrors()
        {
            // Reset the array
            errors = new AssemblySecurityError[0];
        }

        private void CreateError(string assemblyName, string moduleName, string message, string type)
        {
            AssemblySecurityError error = new AssemblySecurityError
            {
                assemblyName = assemblyName,
                moduleName = moduleName,
                securityMessage = message,
                securityType = type,
            };

            // Add an element to the array
            Array.Resize(ref errors, errors.Length + 1);

            // Add to back
            errors[errors.Length - 1] = error;
        }
    }
}