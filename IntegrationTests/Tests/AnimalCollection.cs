using Xunit;

namespace IntegrationTests.Tests;

// q: what is CollectionDefinition
// a: https://xunit.net/docs/shared-context
[CollectionDefinition(nameof(AnimalCollection))]
public class AnimalCollection : ICollectionFixture<AnimalSetupFixture>
{
    
}