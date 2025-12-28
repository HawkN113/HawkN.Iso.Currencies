using HawkN.Iso.Currencies.Builders;
using HawkN.Iso.Currencies.Models;

namespace HawkN.Iso.Currencies.Tests.Builders
{
    public class CurrencyQueryBuilderTests
    {
        private static IReadOnlyList<HawkN.Iso.Currencies.Models.Currency> GetSampleCurrencies() => new List<HawkN.Iso.Currencies.Models.Currency>
        {
            new("USD", "US Dollar", "United States", "840", false, null, CurrencyType.Fiat),
            new("EUR", "Euro", "Eurozone", "978", false, null, CurrencyType.Fiat),
            new("XAU", "Gold (ounce)", null, "959", false, null, CurrencyType.PreciousMetal),
            new("XAG", "Silver (ounce)", null, "961", false, null, CurrencyType.PreciousMetal),
            new("XDR", "IMF Special Drawing Rights", null, "960", false, null, CurrencyType.SpecialUnit),
            new("XXX", "No Currency", null, "999", false, null, CurrencyType.SpecialReserve),
        };

        [Fact]
        public void Build_Should_Filter_By_CurrencyType()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.PreciousMetal)
                .Build();

            // Assert
            Assert.All(result, c => Assert.Equal(CurrencyType.PreciousMetal, c.CurrencyType));
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Build_Should_Filter_By_With_Codes()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.Fiat)
                .With(x => x.Codes("USD"))
                .Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].Code);
        }

        [Fact]
        public void Build_Should_Filter_By_Without_Codes()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.Fiat)
                .Without(x => x.Codes("EUR"))
                .Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].Code);
        }

        [Fact]
        public void Build_Should_Filter_By_With_Names()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.Fiat)
                .With(x => x.Names("Euro"))
                .Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("EUR", result[0].Code);
        }

        [Fact]
        public void Build_Should_Filter_By_With_NumericCodes()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.Fiat)
                .With(x => x.NumericCodes(840))
                .Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("USD", result[0].Code);
        }

        [Fact]
        public void Build_Should_Filter_By_Where_Predicate()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.PreciousMetal)
                .Where(c => c.Code == "XAU")
                .Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("XAU", result[0].Code);
        }

        [Fact]
        public void Build_Should_Apply_Multiple_Filters()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.Fiat)
                .With(x => x.Codes("USD", "EUR"))
                .Without(x => x.Codes("USD"))
                .Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("EUR", result[0].Code);
        }

        [Fact]
        public void Type_Should_Throw_On_Duplicate()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            builder.Includes.Type(CurrencyType.Fiat);

            // Assert
            Assert.Throws<InvalidOperationException>(() => builder.Includes.Type(CurrencyType.Fiat));
        }

        [Fact]
        public void Build_Should_Return_Empty_If_No_Match()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.Fiat)
                .With(x => x.Codes("ZZZ"))
                .Build();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Build_Should_Handle_WithoutNumericCodes()
        {
            // Arrange
            var builder = new CurrencyQueryBuilder(GetSampleCurrencies());

            // Act
            var result = builder
                .Includes.Type(CurrencyType.PreciousMetal)
                .Without(x => x.NumericCodes(959))
                .Build();

            // Assert
            Assert.Single(result);
            Assert.Equal("XAG", result[0].Code);
        }
    }
}
