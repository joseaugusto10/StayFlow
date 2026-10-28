using Mono.Cecil;
using Mono.Cecil.Cil;
using NetArchTest.Rules;
using System.Runtime.CompilerServices;
using static StayFlow.ArchitectureTests.Assemblies;

namespace StayFlow.ArchitectureTests;

public class ConventionTests
{
	[Fact]
	public void Domain_Never_Throws_Exceptions()
	{
		using var module = ModuleDefinition.ReadModule(Assemblies.Domain.Location);

		var offenders = module.GetTypes()
			.Where(type => type.CustomAttributes.Any(a => a.AttributeType.Name == nameof(CompilerGeneratedAttribute)) == false)
			.SelectMany(type => type.Methods)
			.Where(method => method.HasBody && method.Body.Instructions.Any(i => i.OpCode == OpCodes.Throw))
			.Select(method => method.FullName)
			.ToList();

		offenders.ShouldBeEmpty();
	}

	[Fact]
	public void Every_UseCase_Has_Its_Own_Interface()
	{
		var useCases = Assemblies.Application.GetTypes()
			.Where(type => type is { IsClass: true, IsAbstract: false } && type.Name.EndsWith("UseCase"));

		foreach (var useCase in useCases)
			useCase.GetInterfaces().ShouldContain(i => i.Name == $"I{useCase.Name}", $"{useCase.Name} has no I{useCase.Name}");
	}

	[Fact]
	public void Repository_Contracts_Live_In_Domain_As_Interfaces()
	{
		var result = Types.InAssembly(Assemblies.Domain)
			.That().ResideInNamespace($"{DOMAIN}.Repositories")
			.And().HaveNameEndingWith("Repository")
			.Should().BeInterfaces()
			.GetResult();

		result.IsSuccessful.ShouldBeTrue(LayerDependencyTests.Failing(result));
	}

	[Fact]
	public void Infrastructure_Implementations_Are_Hidden()
	{
		var result = Types.InAssembly(Assemblies.Infrastructure)
			.That().HaveNameEndingWith("Repository")
			.Or().HaveNameEndingWith("Hasher")
			.Or().HaveNameEndingWith("TokenHandler")
			.Should().NotBePublic()
			.GetResult();

		result.IsSuccessful.ShouldBeTrue(LayerDependencyTests.Failing(result));
	}
}
