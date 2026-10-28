using System.Reflection;

namespace StayFlow.ArchitectureTests;

internal static class Assemblies
{
	public static readonly Assembly Domain = typeof(StayFlow.Domain.Entities.EntityBase).Assembly;
	public static readonly Assembly Application = typeof(StayFlow.Application.DependencyInjectionExtension).Assembly;
	public static readonly Assembly Infrastructure = typeof(StayFlow.Infrastructure.DependencyInjectionExtension).Assembly;
	public static readonly Assembly Api = typeof(StayFlow.Api.Filters.ExceptionFilter).Assembly;

	public const string DOMAIN = "StayFlow.Domain";
	public const string APPLICATION = "StayFlow.Application";
	public const string INFRASTRUCTURE = "StayFlow.Infrastructure";
	public const string API = "StayFlow.Api";
	public const string COMMUNICATION = "StayFlow.Communication";
	public const string EXCEPTION = "StayFlow.Exception";
}
