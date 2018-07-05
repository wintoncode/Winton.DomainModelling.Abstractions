# Winton.DomainModelling.Abstractions

[![Build status](https://ci.appveyor.com/api/projects/status/7mba8m947ed603r1?svg=true)](https://ci.appveyor.com/project/wintoncode/winton-domainmodelling-abstractions/branch/master)
[![Travis Build Status](https://travis-ci.org/wintoncode/Winton.DomainModelling.Abstractions.svg?branch=master)](https://travis-ci.org/wintoncode/Winton.DomainModelling.Abstractions)
[![NuGet version](https://img.shields.io/nuget/v/Winton.DomainModelling.Abstractions.svg)](https://www.nuget.org/packages/Winton.DomainModelling.Abstractions)
[![NuGet version](https://img.shields.io/nuget/vpre/Winton.DomainModelling.Abstractions.svg)](https://www.nuget.org/packages/Winton.DomainModelling.Abstractions)

Abstractions useful for modelling a domain.

## Building blocks

### Entity

A base class to implement entity types, which are defined by their identity rather than their attributes. Implementers are equal if their IDs are equal. Any equatable ID type can be used.

## Exceptions

### DomainException

Represents domain errors. Extensible for any domain-specific error.

### EntityNotFoundException

Extends `DomainException` to indicate that an entity could not be found.

### UnauthorizedException

Extends `DomainException` to indicate that the action being performed is not authorized.