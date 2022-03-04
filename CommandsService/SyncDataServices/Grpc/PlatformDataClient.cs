using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using CommandsService.Models;
using PlatformService;
using Grpc.Net.Client;
using AutoMapper;
using System;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
        
        public IEnumerable<Platform> ReturnALlPlatforms()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration.GetValue<string>("GrpcPlatform")}");

            var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("GrpcPlatform"));
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var response = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(response.Platform);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not call GRPC server {ex.Message}");
                return null;
            }
        }
    }
}