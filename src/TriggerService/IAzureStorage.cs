// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace TriggerService
{
    public interface IAzureStorage
    {
        string AccountName { get; }
        string AccountAuthority { get; }
        Task<byte[]> DownloadBlockBlobAsync(string blobUrl);
        Task<string> UploadFileTextAsync(string content, string container, string blobName);

        /// <summary>
        /// Downloads the blob's contents as a string.
        /// </summary>
        /// <param name="container">Blob Container name</param>
        /// <param name="blobName">Blob name</param>
        /// <returns>A <see cref="Task"/> object of type string that represents the asynchronous operation.</returns>
        Task<string> DownloadBlobTextAsync(string container, string blobName);

        /// <summary>
        /// Deletes the blob if it already exists.
        /// </summary>
        /// <param name="container">Blob Container name</param>
        /// <param name="blobName">Blob name</param>
        /// <returns>A <see cref="Task"/> object that represents the asynchronous operation.</returns>
        Task DeleteBlobIfExistsAsync(string container, string blobName);

        /// <summary>
        /// Returns all workflow trigger files for the given state, except readme files
        /// </summary>
        /// <param name="state">Workflow state to query for</param>
        /// <returns>List of <see cref="TriggerFile" /></returns>
        Task<IEnumerable<TriggerFile>> GetWorkflowsByStateAsync(WorkflowState state);

        Task<bool> IsAvailableAsync();
        Task<byte[]> DownloadFileUsingHttpClientAsync(string url);
    }
}
