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
if (result.WasSuccess)
{
    // do something with the return value
    var bmi = result.Value;
    if (bmi < 18.5)
    {
        Console.WriteLine($"{pokemonName} is underweight!");
    }
    else if (bmi < 24.9)
    {
        Console.WriteLine($"{pokemonName} with normal range");
    }
    else
    {
        Console.WriteLine($"{pokemonName} is not overwieght!");        
    }
}
else
{
    // handle the failure
    _logger.LogError($"Failed to get {pokemonName} BMI: {result.NonSuccessMessage}");
}
```

Implement your operations to return an `OpResult<T>` so callers try the operation and use the result without having to write `try{} catch{}` blocks for operations that can fail - leaving the onus of catching exceptions that can/should be handeled on the implementor, for example:

```C#
public async Task<OpResult<float>> TryGetPokemonBmiAsync(string name)
{
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

    var pokemon = JObject.Parse(jsonString);
    var height = pokemon["height"].Value<float>();
    var weight = pokemon["weight"].Value<float>();

    var bmi = weight / (height * height);

    return OpResultFactory.CreateSuccess(bmi);
} 
```
