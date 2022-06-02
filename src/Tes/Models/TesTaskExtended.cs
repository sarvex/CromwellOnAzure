﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Tes.Repository;

namespace Tes.Models
{
    public partial class TesTask : RepositoryItem<TesTask>
    {
        private static readonly Regex CromwellTaskInstanceNameRegex = new("(.*):[^:]*:[^:]*");
        private static readonly Regex CromwellShardRegex = new(".*:([^:]*):[^:]*");
        private static readonly Regex CromwellAttemptRegex = new(".*:([^:]*)");

        /// <summary>
        /// Number of retries attempted
        /// </summary>
        [JsonPropertyName("error_count")]
        [DataMember(Name = "error_count")]
        public int ErrorCount { get; set; }

        /// <summary>
        /// Boolean of whether cancellation was requested
        /// </summary>
        [JsonPropertyName("is_cancel_requested")]
        public bool IsCancelRequested { get; set; }

        /// <summary>
        /// Date + time the task was completed, in RFC 3339 format. This is set by the system, not the client.
        /// </summary>
        /// <value>Date + time the task was completed, in RFC 3339 format. This is set by the system, not the client.</value>
        [JsonPropertyName("end_time")]
        public DateTimeOffset? EndTime { get; set; }

        /// <summary>
        /// Top-most parent workflow ID from the workflow engine
        /// </summary>
        [JsonPropertyName("workflow_id")]
        public string WorkflowId { get; set; }

        /// <summary>
        /// Overall reason of the task failure, populated when task execution ends in EXECUTOR_ERROR or SYSTEM_ERROR, for example "DiskFull".
        /// </summary>
        [IgnoreDataMember]
        public string FailureReason => this.Logs?.LastOrDefault()?.FailureReason;

        /// <summary>
        /// Warning that gets populated if task encounters an issue that needs user's attention.
        /// </summary>
        [IgnoreDataMember]
        public string Warning => this.Logs?.LastOrDefault()?.Warning;

        /// <summary>
        /// Cromwell-specific result code, populated when Batch task execution ends in COMPLETED, containing the exit code of the inner Cromwell script.
        /// </summary>
        [IgnoreDataMember]
        public int? CromwellResultCode => this.Logs?.LastOrDefault()?.CromwellResultCode;

        /// <summary>
        /// Cromwell task description without shard and attempt numbers
        /// </summary>
        [IgnoreDataMember]
        public string CromwellTaskInstanceName => CromwellTaskInstanceNameRegex.Match(this.Description).Groups[1].Value;

        /// <summary>
        /// Cromwell shard number
        /// </summary>
        [IgnoreDataMember]
        public int? CromwellShard => int.TryParse(CromwellShardRegex.Match(this.Description).Groups[1].Value, out var result) ? result : null;

        /// <summary>
        /// Cromwell attempt number
        /// </summary>
        [IgnoreDataMember]
        public int? CromwellAttempt => int.TryParse(CromwellAttemptRegex.Match(this.Description).Groups[1].Value, out var result) ? result : null;
    }
}
