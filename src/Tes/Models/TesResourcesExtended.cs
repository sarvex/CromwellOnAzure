// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Tes.Models
{
    /// <summary>
    /// Defines additional backend parameters supported by this TES service.
    /// </summary>
    public partial class TesResources
    {
        /// <summary>
        /// Gets a list of supported backend parameters.
        /// </summary>
        public static readonly List<string> SupportedBackendParameters = typeof(TesResources).GetProperties()
            .Where(p => p.GetCustomAttribute<SupportedBackendParameterAttribute>(false) != null)
            .Select(p => p.Name)
            .ToList();

        /// <summary>
        /// Gets a list of unsupported backend parameters in the current request.
        /// </summary>
        public List<string> UnsupportedBackendParameters => this.BackendParameters.Keys
            .Where(k => SupportedBackendParameters.Contains(k, StringComparer.OrdinalIgnoreCase))
            .ToList();

        /// <summary>
        /// Azure VM Size. If provided, it overrides other resource requirements that indirectly specify the VM size (cpu, memory etc.)
        /// </summary>
        [SupportedBackendParameter]
        public string VmSize => GetBackendParameterValue();

        private string GetBackendParameterValue([CallerMemberName] string key = null)
        {
            return this.BackendParameters.FirstOrDefault(p => p.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value;
        }

        [AttributeUsage(AttributeTargets.Property)]
        private class SupportedBackendParameterAttribute : Attribute
        {
        }
    }
}
