using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SiliconShark.PoQu;
using SubSpec;
using Xunit;
using Xunit.Extensions;

public class QueueNamerSpecs
{
    #region Test specific types

    internal class TypeEndsWithMessage
    {
    }

    internal class TypeOfSufficientLengthSoAsToBeOfSixtyThreeCharacters
    {
    }

    internal class TypeWithShortName
    {
    }

    #endregion

    #region PropertyData

    public static IEnumerable<object[]> MessageTypesForTesting
    {
        get
        {
            yield return new object[] {new TypeEndsWithMessage(), "TypeEndsWithMessage"};
            yield return new object[] {new TypeWithShortName(), "TypeWithShortName"};
            yield return
                new object[]
                    {
                        new TypeOfSufficientLengthSoAsToBeOfSixtyThreeCharacters(),
                        "TypeOfSufficientLengthSoAsToBeOfSixtyThreeCharacters"
                    };
            yield return new object[] {typeof (TypeEndsWithMessage), "TypeEndsWithMessage (passed as Type)"};
            yield return new object[] {typeof (TypeWithShortName), "TypeWithShortName (passed as Type)"};
            yield return
                new object[]
                    {
                        typeof (TypeOfSufficientLengthSoAsToBeOfSixtyThreeCharacters),
                        "TypeOfSufficientLengthSoAsToBeOfSixtyThreeCharacters (passed as Type)"
                    };
        }
    }

    #endregion

    [Thesis]
    [PropertyData("MessageTypesForTesting")]
    public void NamingOfDifferentTypes(dynamic typeToBeNamed, string objectName)
    {
        var subject = default(AzureQueueNamer);
        var name = default(string);
        var allowedCharacters = new Regex("^[a-zA-Z0-9-]*$", RegexOptions.Compiled);

        ("Given an object of name " + objectName)
            .Context(() =>
                     subject = new AzureQueueNamer());

        "when a name is obtained"
            .Do(() => { name = subject.CreateQueueNameFor(typeToBeNamed); });

        "then the namemust be at least 3 characters and at most 63 characters"
            .Observation(() => Assert.InRange(name.Length, 3, 63));

        "then the name must not begin with a '-'"
            .Observation(() => Assert.False(name.StartsWith("-")));

        "then the name must not end with a '-'"
            .Observation(() => Assert.False(name.EndsWith("-")));

        "then the name must not contain uppercase letters"
            .Observation(() => Assert.False(name.Any(char.IsUpper)));

        "then the name must only contain letters, numbers or the dash character"
            .Observation(() => Assert.True(allowedCharacters.IsMatch(name)));

        "then the name must not have two or more consecutive dash characters"
            .Observation(() => Assert.False(name.Contains("--")));

        "then the name should begin with 'queue'"
            .Observation(() => Assert.True(name.StartsWith("queue")));

        "then the name should not end with 'message'"
            .Observation(() => Assert.False(name.EndsWith("message")));
    }
}