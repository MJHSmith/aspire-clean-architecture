using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedKernel;

namespace Domain.ValueObjects;
public static class NameErrors
{
    internal static Error CannotBeCalledBob => Error.Problem(
        "Name.NotBob",
        $"The name \"Bob\" is invalid.");

    internal static Error InvalidCharacters(string invalidCharacters) => Error.Problem(
        "Name.InvalidCharacters",
        $"Name may only contain valid characters, but it contained \"{invalidCharacters}\".");
}
