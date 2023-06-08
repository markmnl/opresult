[![build-test](https://github.com/markmnl/opresult/actions/workflows/build-test.yml/badge.svg)](https://github.com/markmnl/opresult/actions/workflows/build-test.yml)

# FalconWare - Error Handling

Package provides `OpResult` class to represent the result of an operation successful or not. See original blog post on the Operation Result pattern in C# .NET [here](https://www.codeproject.com/Articles/1022462/Error-Handling-in-SOLID-Csharp-NET-The-Operation-R)

Improve your error handling by enforcing:

* Checking the result of an operation
* Handling success and non-success code paths
* Improving code readability

## Usage

After adding this NuGet package e.g.

```
donet github
```

Handle results of an opertaion checking the result's `WasSuccess` then accessing the result `Value`, for example:

```C#
var pokemonName = "squirtle";
var result = await TryGetPokemonBmiAsync(pokemonName);
if (!result.WasSuccess)
{
    // handle the failure
    _logger.LogError($"Failed to get {pokemonName} BMI: {result.NonSuccessMessage}");
}
else
{
    // do something with the return value
    var bmi = result.Value;
    if (bmi < 18.5)
    {
        Console.WriteLine($"{pokemonName} is underweight!");
    }
    else if (bmi < 24.9)
    {
        Console.WriteLine($"{pokemonName} within normal range");
    }
    else
    {
        Console.WriteLine($"{pokemonName} is overweight!");        
    }
}
```

Implement your operations to return an `OpResult<T>` so callers try the operation and use the result without having to write `try{} catch{}` blocks for operations that can fail - _leaving the onus of catching exceptions in the implementation_, for example:

```C#
public async Task<OpResult<float>> TryGetPokemonBmiAsync(string name)
{
    // query the pokeapi for pokemon with name supplied
    string jsonString;
    try
    {
        var response = await _httpClient.GetAsync($"pokemon/{name}");
        if (response.IsSuccessStatusCode)
        {
            jsonString = await response.Content.ReadAsStringAsync();
        }
        else
        {
            return OpResultFactory.CreateFailure<float>($"Failed getting pokemon '{name}', HTTP status code: {response.StatusCode}");
        }
    }
    catch (HttpRequestException ex) 
    {
        return OpResultFactory.CreateFailure<float>(ex);
    }

    // parse pokemon json
    JObject pokemon;
    try
    {
        pokemon = JObject.Parse(jsonString);
    }
    catch (JsonReaderException jre)
    {
        return OpResultFactory.CreateFailure<float>(jre);
    }

    // try extract height and weight, calc bmi and return that
    try
    {
        var height = pokemon["height"].Value<float>();
        var weight = pokemon["weight"].Value<float>();
        if (height < 1.0f)
        {
            return OpResultFactory.CreateFailure<float>("Failed to parse pokemon - height cannot be less than 1");
        }
        if (weight < 1.0f)
        {
            return OpResultFactory.CreateFailure<float>("Failed to parse pokemon - weight cannot be less than 1");
        }
        var bmi = weight / (height * height);
        return OpResultFactory.CreateSuccess(bmi);
    }
    catch (NullReferenceException nre)
    {
        var msg = $"Failed parse pokemon response height or weight missing: {nre.Message}";
        return OpResultFactory.CreateFailure<float>(msg);
    }
    catch (FormatException fe) 
    {
        var msg = $"Failed parse pokemon response height or weight: {fe.Message}";
        return OpResultFactory.CreateFailure<float>(msg);
    }
}
```
