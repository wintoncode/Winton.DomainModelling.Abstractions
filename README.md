# Winton.DomainModelling.Abstractions

[![Build status](https://ci.appveyor.com/api/projects/status/7mba8m947ed603r1?svg=true)](https://ci.appveyor.com/project/wintoncode/winton-domainmodelling-abstractions/branch/master)
[![Travis Build Status](https://travis-ci.org/wintoncode/Winton.DomainModelling.Abstractions.svg?branch=master)](https://travis-ci.org/wintoncode/Winton.DomainModelling.Abstractions)
[![NuGet version](https://img.shields.io/nuget/v/Winton.DomainModelling.Abstractions.svg)](https://www.nuget.org/packages/Winton.DomainModelling.Abstractions)
[![NuGet version](https://img.shields.io/nuget/vpre/Winton.DomainModelling.Abstractions.svg)](https://www.nuget.org/packages/Winton.DomainModelling.Abstractions)

Abstractions useful for modelling a domain.

## Building blocks

### `Entity`

A base class to implement entity types, which are defined by their identity rather than their attributes. 
Instances are equal if their IDs are equal. Any equatable ID type can be used.

## Results

`Result<TData>` represents the result of a domain operation that returns data of type `TData`. It is an abstract type with exactly two concretions: `Success` and `Failure`. It is a specialisation of the more generic `Either` type found in functional programming and is inspired by Scott Wlaschin's Railway Oriented Programming in F#.

It should be used whenever a domain operation may fail, but where that failure mode is a known part of the domain model. For example, consider a domain operation that looks up an adult written without the use of `Result`.

```csharp
public Person GetAdult(int id)
{
    Person person = _personRepository.GetPerson(id);
    if (person == null)
    {
        throw new EntityNotFoundException("The person could not be found.");
    }

    if (person.Age < 18)
    {
        throw new NotAnAdultException("This person is not an adult.");
    }

    return person;
}
```

This implementation has two major drawbacks:
1) From a client's perspective, the API is not experessive enough. The method signature gives no indication that it might throw, so the client would need to peek inside to find that out.
2) From an implementer's perspective, the error checking, whilst simple enough in this example, can often grow quite complex. This makes the implementation of the method hard to follow due to the number of conditional branches. We may try factoring out the condition checking blocks into separate methods to solve this problem. This would also allow us to share some of this logic with other parts of the code base. These factored-out methods would then have a signature like `void CheckPersonExists(Person person)`. Again, this signature tells us nothing about the fact that the method might throw an exception. Currently, the compiler is also not able to do the flow analysis necessary to determine that the `person` is not `null` after calling such a method and so we may be left with warnings in the original call site about possible null references, even though we know we've checked for that condition.

These can both be resolved by using a `Result` type and re-writing the method like this:

```csharp
public Result<Person> GetAdult(int id)
{
    // _personRepository.GetPerson now returns Result<Person> and checks that it exists
    return _personRepository.GetPerson(id)
        .Then(CheckAge);
}

private Result<Person> CheckAge(Person person)
{
    return person.Age < 18 ?
        new Success<Person>(person) as Result<Person> :
        new Failure(new Error("Not an adult", "This person is not an adult."));
}
```

Now we have a much more expressive method signature, which indicates that we might get back a `Person`, but we might also recieve an `Error`. The client is forced to deal with the fact that the operation might fail if they want to try and access the `Person`. We have also been able to extract a method called `CheckAge` that could be reused throughout the domain that has the characteristics of a pure function. The implemenation is now easy to understand and simple to test.

If the operation has no data to return then a `Result<Unit>` can be used. `Unit` is a special type that indicates the absence of a value, because `void` is not a valid type in C#.

Some recommendations on using `Result` types: 
* Make all public domain methods return a `Result<TData>`. Most domain operations will have a failure case that the client should be informed about, but even if they don't, by returning `Result` now it can be easily added later without breaking the public API.
* Once an operation is in "result space", keep it there for as long as possible. `Result` has a fluent API to facilitate this. This is similar to how, once one operation becomes `async` it is best to make all surrounding operations `async` too. This can be re-phrased as, don't match on the result until the last possible moment. For example, in a web API this would mean only unwrapping the result in the Controller.

## Errors

A `Failure` result requires an error in order to convey information about why an operation failed. Like exceptions, errors form a hierarchy, with all errors deriving from the base `Error` type. This library defines a few common domain error types, which are listed below, but it is expected that more specific errors will be defined on a per-domain basis.

Some recommendations on designing errors:
* Try not to create custom errors that are too granular. Model them as you would entities and use the language of the domain model to guide their creation. The concept should make sense to a domain expert.
* The title should be the same for all instances of the error. The details are where instance specific information can be provided. If you are creating a custom error, make the title static and only let clients customise the details. See implementations of errors in this library for examples. 
* Don't use them for non-domain errors. Exceptions should still be used for system failures, such as network requests, and programming errors.

### `Error`

Represents domain errors. Extensible for any domain-specific error.

### `NotFoundError`

Extends `Error` to indicate that an entity could not be found.

### `UnauthorizedError`

Extends `Error` to indicate that the action being performed is not authorized.