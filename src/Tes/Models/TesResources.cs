﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

/*
 * Task Execution Service
 *
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * OpenAPI spec version: 0.3.0
 * 
 * Generated by: https://openapi-generator.tech
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Tes.Models
{
    /// <summary>
    /// Resources describes the resources requested by a task.
    /// </summary>
    [DataContract]
    public partial class TesResources : IEquatable<TesResources>
    {
        public enum SupportedBackendParameters { vm_size, workflow_execution_identity, docker_host_configuration };

        /// <summary>
        /// Requested number of CPUs
        /// </summary>
        /// <value>Requested number of CPUs</value>
        [DataMember(Name = "cpu_cores")]
        public long? CpuCores { get; set; }

        /// <summary>
        /// Is the task allowed to run on preemptible compute instances (e.g. AWS Spot)?
        /// </summary>
        /// <value>Is the task allowed to run on preemptible compute instances (e.g. AWS Spot)?</value>
        [DataMember(Name = "preemptible")]
        public bool? Preemptible { get; set; }

        /// <summary>
        /// Requested RAM required in gigabytes (GB)
        /// </summary>
        /// <value>Requested RAM required in gigabytes (GB)</value>
        [DataMember(Name = "ram_gb")]
        public double? RamGb { get; set; }

        /// <summary>
        /// Requested disk size in gigabytes (GB)
        /// </summary>
        /// <value>Requested disk size in gigabytes (GB)</value>
        [DataMember(Name = "disk_gb")]
        public double? DiskGb { get; set; }

        /// <summary>
        /// Request that the task be run in these compute zones.
        /// </summary>
        /// <value>Request that the task be run in these compute zones.</value>
        [DataMember(Name = "zones")]
        public List<string> Zones { get; set; }

        /// <summary>
        /// Key/value pairs for backend configuration
        /// </summary>
        /// <value> Key/value pairs for backend configuration</value>
        [DataMember(Name = "backend_parameters")]
        public Dictionary<string, string> BackendParameters { get; set; }

        /// <summary>
        /// If set to true, TES shall fail the task if any backend_parameters key/values are unsupported, otherwise, TES will attempt to run the task
        /// </summary>
        /// <value> If set to true, TES shall fail the task if any backend_parameters key/values are unsupported, otherwise, TES will attempt to run the task</value>
        [DataMember(Name = "backend_parameters_strict")]
        public bool? BackendParametersStrict { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
            => new StringBuilder()
                .Append("class TesResources {\n")
                .Append("  CpuCores: ").Append(CpuCores).Append('\n')
                .Append("  Preemptible: ").Append(Preemptible).Append('\n')
                .Append("  RamGb: ").Append(RamGb).Append('\n')
                .Append("  DiskGb: ").Append(DiskGb).Append('\n')
                .Append("  Zones: ")
                .Append(
                    Zones?.Count > 0 ?
                    string.Join(",", Zones) : null)
                .Append('\n')
                .Append("  BackendParameters: ")
                .Append(
                    BackendParameters?.Keys.Count > 0 ?
                    string.Join(",", BackendParameters.Select(kv => $"({kv.Key},{kv.Value})")) : null)
                .Append('\n')
                .Append("  BackendParametersStrict: ").Append(BackendParametersStrict).Append('\n')
                .Append("}\n")
                .ToString();

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
            => obj switch
            {
                var x when x is null => false,
                var x when ReferenceEquals(this, x) => true,
                _ => obj.GetType() == GetType() && Equals((TesResources)obj),
            };

        /// <summary>
        /// Returns true if TesResources instances are equal
        /// </summary>
        /// <param name="other">Instance of TesResources to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(TesResources other)
            => other switch
            {
                var x when x is null => false,
                var x when ReferenceEquals(this, x) => true,
                _ =>
                (
                    CpuCores == other.CpuCores ||
                    CpuCores is not null &&
                    CpuCores.Equals(other.CpuCores)
                ) &&
                (
                    Preemptible == other.Preemptible ||
                    Preemptible is not null &&
                    Preemptible.Equals(other.Preemptible)
                ) &&
                (
                    RamGb == other.RamGb ||
                    RamGb is not null &&
                    RamGb.Equals(other.RamGb)
                ) &&
                (
                    DiskGb == other.DiskGb ||
                    DiskGb is not null &&
                    DiskGb.Equals(other.DiskGb)
                ) &&
                (
                    Zones == other.Zones ||
                    Zones is not null && other.Zones is not null &&
                    Zones.SequenceEqual(other.Zones)
                ) &&
                (
                    BackendParameters == other.BackendParameters ||
                    BackendParameters is not null && other.BackendParameters is not null &&
                    BackendParameters.SequenceEqual(other.BackendParameters)
                ) &&
                (
                    BackendParametersStrict == other?.BackendParametersStrict
                )
            };

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (CpuCores is not null)
                {
                    hashCode = hashCode * 59 + CpuCores.GetHashCode();
                }

                if (Preemptible is not null)
                {
                    hashCode = hashCode * 59 + Preemptible.GetHashCode();
                }

                if (RamGb is not null)
                {
                    hashCode = hashCode * 59 + RamGb.GetHashCode();
                }

                if (DiskGb is not null)
                {
                    hashCode = hashCode * 59 + DiskGb.GetHashCode();
                }

                if (Zones is not null)
                {
                    hashCode = hashCode * 59 + Zones.GetHashCode();
                }

                if (BackendParameters != null)
                {
                    hashCode = hashCode * 59 + BackendParameters.GetHashCode();
                }

                if (BackendParametersStrict != null)
                {
                    hashCode = hashCode * 59 + BackendParametersStrict.GetHashCode();
                }

                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(TesResources left, TesResources right)
            => Equals(left, right);

        public static bool operator !=(TesResources left, TesResources right)
            => !Equals(left, right);

#pragma warning restore 1591
        #endregion Operators
    }
}
