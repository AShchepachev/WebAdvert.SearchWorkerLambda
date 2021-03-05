using System;
using AdvertApi.Models.Messages;

namespace WebAdvert.SearchWorkerLambda
{
    internal static class MappingHelper
    {
        public static AdvertType Map(AdvertConfirmedMessage message)
        {
            return new AdvertType
            {
                Id = message.Id,
                Title = message.Title,
                CreationDateTime = DateTime.UtcNow
            };
        }
    }
}