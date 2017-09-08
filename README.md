OpenWeatherMap-Api-Net
======================

This project is a fully asynchronous .NET library for interacting with the great [OpenWeatherMap API](http://openweathermap.org/API).

## Usage examples

```c#
var client = new OpenWeatherMapClient("optionalAppId");
var currentWeather = await client.CurrentWeather.GetByName("London");
Console.WriteLine(currentWeather.Weather.Value);
```

### Supported Platforms

* .NET Standard 1.1

### Copyright and License

Copyright 2014 Joan Caron

Licensed under the MIT License
