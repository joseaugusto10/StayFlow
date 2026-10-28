using NetArchTest.Rules;
using static StayFlow.ArchitectureTests.Assemblies;

namespace StayFlow.ArchitectureTests;

public class LayerDependencyTests
{
	[Fact]
	public void Domain_Depends_On_Nothing()
	{
		var result = Types.InAssembly(Assemblies.Domain)
			.ShouldNot()
			.HaveDependencyOnAny(APPLICATION, INFRASTRUCTURE, API, COMMUNICATION, EXCEPTION,
				"Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore", "FluentValidation")
			.GetResult();

		result.IsSuccessful.ShouldBeTrue(Failing(result));
	}

	[Fact]
	public void Application_Does_Not_Depend_On_Infrastructure_Or_Api()
	{
		var result = Types.InAssembly(Assemblies.Application)
			.ShouldNot()
			.HaveDependencyOnAny(INFRASTRUCTURE, API, "Microsoft.EntityFrameworkCore", "Microsoft.AspNetCore")
			.GetResult();

		result.IsSuccessful.ShouldBeTrue(Failing(result));
	}

	[Fact]
	public void Infrastructure_Only_Knows_The_Domain()
	{
		var result = Types.InAssembly(Assemblies.Infrastructure)
			.ShouldNot()
			.HaveDependencyOnAny(APPLICATION, API, COMMUNICATION, EXCEPTION)
			.GetResult();

		result.IsSuccessful.ShouldBeTrue(Failing(result));
	}

	[Fact]
	public void Controllers_Do_Not_Touch_Repositories_Or_DbContext()
	{
		var result = Types.InAssembly(Assemblies.Api)
			.That().HaveNameEndingWith("Controller")
			.ShouldNot()
			.HaveDependencyOnAny($"{DOMAIN}.Repositories", INFRASTRUCTURE, "Microsoft.EntityFrameworkCore")
			.GetResult();

		result.IsSuccessful.ShouldBeTrue(Failing(result));
	}

	internal static string Failing(TestResult result) =>
		"Violations: " + string.Join(", ", result.FailingTypeNames ?? []);
}
