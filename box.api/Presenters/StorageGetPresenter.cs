﻿using box.api.Serialization;
using box.application.Interfaces;
using box.application.Models.Response;
using System.Net;

namespace box.api.Presenters
{
    public sealed class StorageGetPresenter : IOutputPort<StorageGetResponse>
    {
        public Stream ContentResult { get; set; }
        public JsonContentResult ContentResultJson { get; set; }
        public int StatusCode { get; set; }

        public StorageGetPresenter() => ContentResult = new MemoryStream();

        public void Handle(StorageGetResponse response)
        {
            if (response.Success)
            {
                StatusCode = (int)HttpStatusCode.OK;
                ContentResult = new MemoryStream(response.FileStream);
            }
            else
            {
                StatusCode = (int)HttpStatusCode.BadRequest;
                ContentResultJson = new();
                ContentResultJson.Content = JsonSerializer.SerializeObject(response.Errors);
                ContentResultJson.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            
        }
    }
}