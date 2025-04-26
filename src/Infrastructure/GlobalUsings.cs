global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;

global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.Extensions.DependencyInjection;

global using OzonParserService.Infrastructure.Persistence;
global using OzonParserService.Application.Publish;
global using OzonParserService.Application.Common.Authentication;
global using OzonParserService.Application.Common.DateTimeProvider;

global using OzonParserService.Infrastructure.Common.Authentication;
global using OzonParserService.Infrastructure.Common.DateTime;

global using ProductDataContract;
global using System.Text;
global using System;
global using MassTransit;

global using Hangfire;
global using PuppeteerSharp;
global using Hangfire.PostgreSql;

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
